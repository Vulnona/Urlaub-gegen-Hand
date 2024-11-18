using System.Text.RegularExpressions;
using UGH.Contracts.Authentication;
using UGH.Infrastructure.Services;
using UGH.Domain.Interfaces;
using UGH.Domain.Entities;
using UGH.Domain.Core;
using MediatR;

namespace UGH.Application.Authentication;

public class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponse>>
{
    private readonly UserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;
    private readonly PasswordService _passwordService;
    private readonly UGH.Infrastructure.Services.EmailService _emailService;
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(
        UserService userService,
        TokenService tokenService,
        UGH.Infrastructure.Services.EmailService emailService,
        IUserRepository userRepository,
        PasswordService passwordService,
        ILogger<RegisterUserCommandHandler> logger
    )
    {
        _userService = userService;
        _tokenService = tokenService;
        _emailService = emailService;
        _passwordService = passwordService;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<RegisterUserResponse>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            if (await _userRepository.GetUserByEmailAsync(request.Email_Address) is not null)
            {
                return Result.Failure<RegisterUserResponse>(
                    new Error("Error.EmailConflict", "E-Mail Adresse existiert bereits")
                );
            }

            var passwordPattern =
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$";
            if (!Regex.IsMatch(request.Password, passwordPattern))
            {
                return Result.Failure<RegisterUserResponse>(
                    new Error(
                        "Error.InvalidPassword",
                        "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character."
                    )
                );
            }

            if (!ValidateAge(request.DateOfBirth, out string validationMessage))
            {
                return Result.Failure<RegisterUserResponse>(
                    new Error("Error.InvalidDateOfBirth", validationMessage)
                );
            }

            DateTime parsedDateOfBirth = DateTime.Parse(request.DateOfBirth);

            var salt = _passwordService.GenerateSalt();
            var hashPassword = _passwordService.HashPassword(request.Password, salt);

            var newUser = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = DateOnly.FromDateTime(parsedDateOfBirth),
                Gender = request.Gender,
                Street = request.Street,
                HouseNumber = request.HouseNumber,
                PostCode = request.PostCode,
                City = request.City,
                Country = request.Country,
                Email_Address = request.Email_Address,
                IsEmailVerified = false,
                Password = hashPassword,
                SaltKey = salt,
                Facebook_link = request.Facebook_link,
                Link_RS = request.Link_RS,
                Link_VS = request.Link_VS,
                State = request.State,
                VerificationState = UGH_Enums.VerificationState.IsNew
            };

            var savedUser = await _userRepository.AddUserAsync(newUser);


            var defaultUserRole = await _userService.GetDefaultUserRoleAsync();

            if (defaultUserRole != null)
            {
                await _userService.AssignUserRoleAsync(savedUser.User_Id, defaultUserRole.RoleId);
            }

            var verificationToken = _tokenService.GenerateNewEmailVerificator(savedUser.User_Id);
            await _emailService.SendVerificationEmailAsync(
                savedUser.Email_Address,
                verificationToken
            );

            // Return a success result with the created user response
            var response = new RegisterUserResponse
            {
                Email = savedUser.Email_Address,
                UserId = savedUser.User_Id,
                FirstName = savedUser.FirstName
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            // Log the exception
            _logger.LogError(
                $"Exception occurred while registering user: {ex.Message} | StackTrace: {ex.StackTrace}"
            );

            // Return a failure result with an internal server error code and message
            return Result.Failure<RegisterUserResponse>(
                new Error(
                    "Error.UnexpectedError",
                    "An unexpected error occurred while registering the user."
                )
            );
        }
    }

    private bool ValidateAge(string dateOfBirth, out string validationMessage)
    {
        validationMessage = string.Empty;

        if (!DateTime.TryParse(dateOfBirth, out DateTime parsedDateOfBirth))
        {
            validationMessage = "Invalid date format for Date of Birth.";
            return false;
        }

        DateTime today = DateTime.Today;
        DateTime minDateAllowed = today.AddYears(-14);

        if (parsedDateOfBirth > minDateAllowed || parsedDateOfBirth < today.AddYears(-120))
        {
            validationMessage = "Invalid age. User must be between 14 and 120 years old.";
            return false;
        }

        return true;
    }
}
