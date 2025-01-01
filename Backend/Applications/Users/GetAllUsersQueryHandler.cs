using MediatR;
using UGH.Domain.Interfaces;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGH.Application.Users;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PaginatedList<UserDTO>>
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

    public async Task<PaginatedList<UserDTO>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var paginatedUsers = await _userRepository.GetAllUsersAsync(
                request.PageNumber,
                request.PageSize
            );

            return paginatedUsers;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw;
        }
    }
}
