using UGH.Domain.Core;
using MediatR;
using UGH.Domain.ViewModels;
using UGHApi.Shared;

namespace UGH.Application.Offers;

public class GetOffersQuery : IRequest<Result<PaginatedList<OfferDTO>>>
{
    public string SearchTerm { get; set; }
    public Guid UserId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetOffersQuery(string searchTerm, Guid userId, int pageNumber, int pageSize)
    {
        SearchTerm = searchTerm;
        UserId = userId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
