using MediatR;
using Microsoft.EntityFrameworkCore;
using UGH.Contracts.Confirmation;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGH.Infrastructure.Services;

namespace UGH.Application.Admin;

public class UpdateVerifyStateCommandHandler : IRequestHandler<UpdateVerifyStateCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly UserService _userService;
    private readonly S3Service _s3Service;
    private readonly AdminVerificationMailService _mailService;
    private readonly ILogger<UpdateVerifyStateCommandHandler> _logger;

    public UpdateVerifyStateCommandHandler(
        IUserRepository userRepository,
        UserService userService,
        S3Service s3Service,
        AdminVerificationMailService mailService,
        ILogger<UpdateVerifyStateCommandHandler> logger
    )
    {
        _userRepository = userRepository;
        _s3Service = s3Service;
        _userService = userService;
        _mailService = mailService;
        _logger = logger;
    }

    public async Task<Result> Handle(
        UpdateVerifyStateCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                return Result.Failure(Errors.General.NotFound("UserNotFound", request.UserId));
            }

            user.VerificationState = request.VerificationState;
            await _userRepository.UpdateUserAsync(user);

            if (request.VerificationState == UGH_Enums.VerificationState.VerificationFailed)
            {
                _ = SendVerificationEmailAsync(
                    user,
                    "Verification Failed",
                    delay: TimeSpan.FromMinutes(5),
                    cancellationToken
                );
            }
            else
            {
                _ = SendVerificationEmailAsync(
                    user,
                    "Verified",
                    cancellationToken: cancellationToken
                );
            }
            //Delete from AWS after failed
            var deleteTasks = new List<Task>();

            if (!string.IsNullOrEmpty(user.Link_RS))
                deleteTasks.Add(_s3Service.DeleteFileAsync(ExtractKeyFromUrl(user.Link_RS)));

            if (!string.IsNullOrEmpty(user.Link_VS))
                deleteTasks.Add(_s3Service.DeleteFileAsync(ExtractKeyFromUrl(user.Link_VS)));

            await Task.WhenAll(deleteTasks);

            _userService.DeleteUserInfo(request.UserId);

            return Result.Success("Successfully updated verification state of user.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError($"Database error: {ex.Message} | {ex.StackTrace}");
            return Result.Failure(
                Errors.General.InvalidOperation(
                    "Database error occurred while verifying user state"
                )
            );
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unhandled exception: {ex.Message} | {ex.StackTrace}");
            return Result.Failure(
                Errors.General.InvalidOperation(
                    "Unexpected error occurred while verifying user state"
                )
            );
        }
    }

    private string ExtractKeyFromUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return string.Empty;

        return new Uri(url).LocalPath.TrimStart('/');
    }

    private async Task SendVerificationEmailAsync(
        User user,
        string status,
        TimeSpan? delay = null,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            if (delay.HasValue)
            {
                await Task.Delay(delay.Value, cancellationToken);
            }

            var confirmationRequest = new ConfirmationRequest
            {
                toEmail = user.Email_Address,
                userName = $"{user.FirstName} {user.LastName}",
                status = status,
            };

            await _mailService.SendConfirmationEmailAsync(confirmationRequest);
            _logger.LogInformation(
                $"Verification email '{status}' sent successfully to {user.Email_Address}"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to send verification email: {ex.Message} | {ex.StackTrace}");
        }
    }
}
