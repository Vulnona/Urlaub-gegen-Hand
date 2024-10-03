using MediatR;
using Microsoft.Extensions.Logging;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;
using UGH.Infrastructure.Services;

namespace UGH.Application.Authentication;

public class ResendVerificationEmailCommandHandler : IRequestHandler<ResendVerificationEmailCommand, Result>
{
    private readonly UserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;
    private readonly EmailService _emailService;
    private readonly ILogger<ResendVerificationEmailCommandHandler> _logger;

    public ResendVerificationEmailCommandHandler(
        UserService userService,
        TokenService tokenService,
        IUserRepository userRepository,
        EmailService emailService,
        ILogger<ResendVerificationEmailCommandHandler> logger)
    {
        _userService = userService;
        _tokenService = tokenService;
        _emailService = emailService;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(ResendVerificationEmailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Email) || !_emailService.IsValidEmail(request.Email))
            {
                return Result.Failure(new Error("InvalidEmail", "Invalid email address."));
            }

            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure(new Error("UserNotFound", "User not found."));
            }

            var verificationToken = _tokenService.GenerateNewEmailVerificator(user.User_Id);

            var emailSent = await _emailService.SendVerificationEmailAsync(request.Email, verificationToken);
            if (!emailSent)
            {
                return Result.Failure(new Error("EmailSendFailed", "Failed to send verification email."));
            }

            return Result.Success("Verification email sent successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(new Error("UnexpectedError", "An unexpected error occurred while sending the verification email."));
        }
    }
}
