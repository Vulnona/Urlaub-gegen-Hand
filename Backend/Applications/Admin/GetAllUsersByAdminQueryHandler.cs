using UGH.Domain.Interfaces;
using UGH.Domain.Core;
using MediatR;
using UGHApi.ViewModels;

namespace UGH.Application.Admin;

public class GetAllUsersByAdminQueryHandler
    : IRequestHandler<GetAllUsersByAdminQuery, Result<IEnumerable<UserDTO>>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetAllUsersByAdminQueryHandler> _logger;

    public GetAllUsersByAdminQueryHandler(
        IUserRepository userRepository,
        ILogger<GetAllUsersByAdminQueryHandler> logger
    )
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<UserDTO>>> Handle(
        GetAllUsersByAdminQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var users = await _userRepository.GetAllUsersAsync();

            if (users == null || !users.Any())
            {
                return Result.Failure<IEnumerable<UserDTO>>(
                    Errors.General.NotFound("NoUserFound", users)
                );
            }

            return Result<IEnumerable<UserDTO>>.Success(users);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure<IEnumerable<UserDTO>>(
                Errors.General.InvalidOperation("Something went wrong while fetching users")
            );
        }
    }
}
