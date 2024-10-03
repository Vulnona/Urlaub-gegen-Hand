using UGH.Domain.Interfaces;
using UGH.Domain.Core;
using MediatR;
using UGHApi.ViewModels;

namespace UGH.Application.Users;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDTO>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(
        IUserRepository userRepository,
        ILogger<GetUserByIdQueryHandler> logger
    )
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<UserDTO>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var user = await _userRepository.GetUserDetailsByIdAsync(request.UserId);

            if (user == null)
            {
                return Result.Failure<UserDTO>(Errors.General.NotFound("UserNotFound", user));
            }

            return Result.Success(user);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure<UserDTO>(
                Errors.General.InvalidOperation($"Internal server error: {ex.Message}")
            );
        }
    }
}
