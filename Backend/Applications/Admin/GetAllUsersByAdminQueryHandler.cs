using UGH.Domain.Interfaces;
using UGHApi.ViewModels;
using UGH.Domain.Core;
using UGHApi.Shared;
using MediatR;

namespace UGH.Application.Admin;

public class GetAllUsersByAdminQueryHandler
    : IRequestHandler<GetAllUsersByAdminQuery, Result<PaginatedList<UserDTO>>>
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

    public async Task<Result<PaginatedList<UserDTO>>> Handle(
        GetAllUsersByAdminQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var paginatedUsers = await _userRepository.GetAllUsersAsync(
                request.PageNumber,
                request.PageSize
            );

            if (paginatedUsers == null)
            {
                return Result.Failure<PaginatedList<UserDTO>>(
                    Errors.General.NotFound("NoUserFound", paginatedUsers)
                );
            }

            return Result<PaginatedList<UserDTO>>.Success(paginatedUsers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure<PaginatedList<UserDTO>>(
                Errors.General.InvalidOperation("Something went wrong while fetching users")
            );
        }
    }
}
