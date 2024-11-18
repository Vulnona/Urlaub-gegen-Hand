using UGH.Domain.Core;
using MediatR;
using UGHApi.ViewModels;
using UGHApi.Shared;

namespace UGH.Application.Admin;

public class GetAllUsersByAdminQuery : IRequest<Result<PaginatedList<UserDTO>>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetAllUsersByAdminQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

