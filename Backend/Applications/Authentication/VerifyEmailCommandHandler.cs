using UGH.Infrastructure.Services;
using UGH.Domain.Core;
using MediatR;

namespace UGH.Application.Authentication;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Result>
{
    private readonly UserService _userService;
    private readonly ILogger<VerifyEmailCommandHandler> _logger;

    public VerifyEmailCommandHandler(
        UserService userService,
        ILogger<VerifyEmailCommandHandler> logger
    )
    {
        _userService = userService;
        _logger = logger;
    }

    public async Task<Result> Handle(
        VerifyEmailCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var result = await _userService.VerifyEmailAddressAsync(request.Token);

            if (result.IsFailure)
            {
                _logger.LogError(
                    $"Email verification failed: {result.Error.Code} - {result.Error.Message}"
                );

                return result;
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                $"Exception occurred while verifying email: {ex.Message} | StackTrace: {ex.StackTrace}"
            );

            return Result.Failure(
                new Error(
                    "Error.UnexpectedError",
                    "An unexpected error occurred while verifying the email."
                )
            );
        }
    }
}
