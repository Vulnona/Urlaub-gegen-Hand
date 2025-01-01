using UGHApi.ViewModels;
using UGH.Domain.Core;
using UGHApi.Shared;
using MediatR;

namespace UGHApi.Applications.Reviews;

public class GetAllOffersForReviewsAdminQuery : IRequest<Result<PaginatedList<ReviewOfferDTO>>>
{
    public string SearchTerm { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetAllOffersForReviewsAdminQuery(string searchTerm, int pageNumber, int pageSize)
    {
        SearchTerm = searchTerm;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

