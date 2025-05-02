using MediatR;
using Microsoft.Extensions.Logging;
using UGH.Domain.Core;
using UGH.Infrastructure.Services;

namespace UGH.Application.Authentication;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result>
{
    private readonly TokenService _tokenService;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(TokenService tokenService, ILogger<RefreshTokenCommandHandler> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<Result> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!_tokenService.TryGetUserEmail(request.RefreshToken, out var email))
            {
                return Result.Failure(new Error("Unauthorized", "Invalid refresh token."));
            }

            var accessToken = await _tokenService.GenerateJwtToken(email, Guid.NewGuid());

            return Result.Success(new { accessToken });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(new Error("Error.UnexpectedError", "An unexpected error occurred."));
        }
    }
}
