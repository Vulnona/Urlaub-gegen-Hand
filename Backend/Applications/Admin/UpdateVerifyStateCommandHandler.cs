using Microsoft.EntityFrameworkCore;
using UGH.Infrastructure.Services;
using UGH.Contracts.Confirmation;
using UGH.Domain.Interfaces;
using UGH.Domain.Core;
using MediatR;

namespace UGH.Application.Admin;

#pragma warning disable CS4014
public class UpdateVerifyStateCommandHandler : IRequestHandler<UpdateVerifyStateCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly UserService _userService;
    private readonly AdminVerificationMailService _mailService;
    private readonly ILogger<UpdateVerifyStateCommandHandler> _logger;

    public UpdateVerifyStateCommandHandler(
        IUserRepository userRepository,
        UserService userService,
        AdminVerificationMailService mailService,
        ILogger<UpdateVerifyStateCommandHandler> logger
    )
    {
        _userRepository = userRepository;
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
                return Result.Failure(Errors.General.NotFound("UserNotFound", user));
            }

            user.VerificationState = request.VerificationState;
            await _userRepository.UpdateUserAsync(user);

            if (request.VerificationState == UGH_Enums.VerificationState.VerificationFailed)
            {
                _userService.DeleteUserInfo(request.UserId);

                if (request.VerificationState == UGH_Enums.VerificationState.VerificationFailed)
                {
                    Task.Run(async () =>
                    {
                        await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
                        var confirmationRequest = new ConfirmationRequest
                        {
                            toEmail = user.Email_Address,
                            userName = $"{user.FirstName} {user.LastName}",
                            status = "Verification Failed"
                        };
                        await _mailService.SendConfirmationEmailAsync(confirmationRequest);
                    });
                }
                else
                {
                    var confirmationRequest = new ConfirmationRequest
                    {
                        toEmail = user.Email_Address,
                        userName = $"{user.FirstName} {user.LastName}",
                        status = "Verified"
                    };

                    Task.Run(async () =>
                        {
                            await _mailService.SendConfirmationEmailAsync(confirmationRequest);
                            _logger.LogInformation(
                                "Confirmation email sent successfully to the user."
                            );
                        })
                        .ConfigureAwait(false);
                }
            }

            return Result.Success("Successfully updated verification state of user.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogInformation($"Database error: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(
                Errors.General.InvalidOperation($"Somthing went wrong while verifying user state")
            );
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(
                Errors.General.InvalidOperation($"Somthing went wrong while verifying user state")
            );
        }
    }
}
