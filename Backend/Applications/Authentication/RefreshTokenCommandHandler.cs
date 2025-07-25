using MediatR;
using Microsoft.Extensions.Logging;
using UGH.Domain.Core;
using UGH.Infrastructure.Services;
using UGH.Domain.Interfaces;

namespace UGH.Application.Authentication;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result>
{
    private readonly TokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(TokenService tokenService, ILogger<RefreshTokenCommandHandler> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    public RefreshTokenCommandHandler(TokenService tokenService, IUserRepository userRepository, ILogger<RefreshTokenCommandHandler> logger)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
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

            // Hole den User und die Memberships
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return Result.Failure(new Error("Unauthorized", "User not found for refresh token."));
            }
            var activeMemberships = await _userRepository.GetActiveUserMembershipsAsync(user.User_Id);
            var accessToken = await _tokenService.GenerateJwtToken(user, activeMemberships);

            return Result.Success(new { accessToken });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(new Error("Error.UnexpectedError", "An unexpected error occurred."));
        }
    }
}
