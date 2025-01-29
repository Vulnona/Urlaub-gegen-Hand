using MediatR;
using UGH.Contracts.Authentication;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;
using UGH.Infrastructure.Services;

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
            var userValid = _userService.ValidateUser(request.Email, request.Password);

            if (!userValid.IsValid)
            {
                return Result.Failure<LoginResponse>(
                    Errors.General.InvalidOperation(userValid.ErrorMessage)
                );
            }

            var user = await _userRepository.GetUserByEmailAsync(request.Email);

            var activeMemberships = user
                .UserMemberships.Where(um => um.IsMembershipActive)
                .ToList();

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

            var accessToken = await _tokenService.GenerateJwtToken(
                request.Email,
                user.User_Id,
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
