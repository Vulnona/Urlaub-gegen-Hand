using UGHApi.ViewModels;
using UGHApi.Shared;
using MediatR;

namespace UGH.Application.Users;

public class GetAllUsersQuery : IRequest<PaginatedList<UserDTO>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetAllUsersQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
