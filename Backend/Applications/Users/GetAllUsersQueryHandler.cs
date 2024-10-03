using UGH.Domain.Interfaces;
using MediatR;
using UGHApi.ViewModels;

namespace UGH.Application.Users;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDTO>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetAllUsersQueryHandler> _logger;

    public GetAllUsersQueryHandler(
        IUserRepository userRepository,
        ILogger<GetAllUsersQueryHandler> logger
    )
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<UserDTO>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var users = await _userRepository.GetAllUsersAsync();

            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw;
        }
    }
}
