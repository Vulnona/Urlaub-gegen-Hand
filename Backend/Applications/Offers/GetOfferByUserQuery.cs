using UGH.Domain.Core;
using MediatR;
using UGH.Domain.ViewModels;
using UGHApi.Shared;

namespace UGH.Application.Offers;

public class GetOfferByUserQuery : IRequest<Result<PaginatedList<OfferDTO>>>
{
    public Guid UserId { get; }
    public string SearchTerm { get; }
    public int PageNumber { get; }
    public int PageSize { get; }

    public GetOfferByUserQuery(Guid userId, string searchTerm,
        int pageNumber ,
        int pageSize )
    {
        UserId = userId;
        SearchTerm = searchTerm;
        PageNumber = pageNumber;
        PageSize = pageSize;

    }
}
