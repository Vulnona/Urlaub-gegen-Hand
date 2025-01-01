using MediatR;
using UGH.Application.Admin;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;

public class VerifyUserCommandHandler : IRequestHandler<VerifyUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<VerifyUserCommandHandler> _logger;

    public VerifyUserCommandHandler(
        IUserRepository userRepository,
        ILogger<VerifyUserCommandHandler> logger
    )
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(VerifyUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);

            if (user == null)
            {
                return Result.Failure(Errors.General.NotFound("User", user));
            }

            if (user.VerificationState == UGH_Enums.VerificationState.Verified)
            {
                return Result.Failure(Errors.General.InvalidOperation("User not verified!"));
            }

            user.VerificationState = UGH_Enums.VerificationState.Verified;
            await _userRepository.UpdateUserAsync(user);

            return Result.Success("User verification completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(
                Errors.General.InvalidOperation("Something went wrong while verifying user.")
            );
        }
    }
}
