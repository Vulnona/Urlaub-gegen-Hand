using MediatR;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGH.Application.Users;

public class GetAllUsersQuery : IRequest<PaginatedList<UserDTO>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; } = "asc";
    public string? SearchTerm { get; set; }
}
