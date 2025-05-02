using MediatR;
using UGH.Domain.Core;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGH.Application.Admin;

public class GetAllUsersByAdminQuery : IRequest<Result<PaginatedList<UserDTO>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortBy { get; set; } = null;
    public string SortDirection { get; set; } = "asc";
    public string SearchTerm { get; set; } = null;
}
