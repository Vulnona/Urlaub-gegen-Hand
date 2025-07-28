using MediatR;
using UGH.Contracts.Authentication;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;
using UGH.Infrastructure.Services;
using UGH.Domain.Entities;

namespace UGH.Application.Authentication;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly UserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        UserService userService,
        TokenService tokenService,
        IUserRepository userRepository,
        ILogger<LoginCommandHandler> logger
    )
    {
        _userService = userService;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<LoginResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            _logger.LogError($"=== LOGIN COMMAND HANDLER CALLED FOR {request.Email} ===");
            
            var userValid = await _userService.ValidateUser(request.Email, request.Password);

            if (!userValid.IsValid)
            {
                return Result.Failure<LoginResponse>(
                    Errors.General.InvalidOperation(userValid.ErrorMessage)
                );
            }

            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogError($"User not found for email: {request.Email}");
                return Result.Failure<LoginResponse>(
                    Errors.General.NotFound("Email", $"User with email {request.Email} not found.")
                );
            }

            // Check if user is Admin - Admin MUST use 2FA
            if (user.UserRole == UserRoles.Admin && !user.IsTwoFactorEnabled)
            {
                return Result.Failure<LoginResponse>(
                    Errors.General.InvalidOperation("Admin accounts must have 2FA enabled")
                );
            }

            // Check if user has 2FA enabled - redirect to 2FA login
            if (user.IsTwoFactorEnabled)
            {
                var twoFactorToken = _tokenService.GenerateTwoFactorToken(user.User_Id, user.Email_Address);
                return Result.Success(new LoginResponse
                {
                    RequiresTwoFactor = true,
                    Email = request.Email,
                    UserId = user.User_Id,
                    FirstName = user.FirstName,
                    Message = "Two-factor authentication required",
                    TwoFactorToken = twoFactorToken
                });
            }

            // Use explicit query to get active memberships
            var activeMemberships = await _userRepository.GetActiveUserMembershipsAsync(user.User_Id);
            
            _logger.LogError($"=== USER {request.Email} HAS {activeMemberships.Count} ACTIVE MEMBERSHIPS ===");

            if (activeMemberships.Any())
            {
                if (user.CurrentMembership == null)
                {
                    user.SetMembershipId(activeMemberships.First().MembershipID);
                    await _userRepository.UpdateUserAsync(user);
                }
            }
            else
            {
                if (user.CurrentMembership != null)
                {
                    user.SetMembershipId(null);
                    await _userRepository.UpdateUserAsync(user);
                }
            }

            var accessToken = _tokenService.GenerateJwtToken(
                user,
                activeMemberships
            );

            var refreshToken = _tokenService.GenerateRefreshToken();
            _tokenService.StoreRefreshToken(refreshToken, request.Email);

            return Result.Success(
                new LoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Email = request.Email,
                    UserId = user.User_Id,
                    FirstName = user.FirstName,
                }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw;
        }
    }
}
