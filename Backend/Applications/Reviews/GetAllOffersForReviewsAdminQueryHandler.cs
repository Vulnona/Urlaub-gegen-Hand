using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGHApi.Applications.Reviews;

public class GetAllOffersForReviewsAdminQueryHandler
    : IRequestHandler<GetAllOffersForReviewsAdminQuery, Result<PaginatedList<ReviewOfferDTO>>>
{
    private readonly IOfferRepository _offerRepository;
    private readonly ILogger<GetAllOffersForReviewsAdminQueryHandler> _logger;

    public GetAllOffersForReviewsAdminQueryHandler(
        IOfferRepository offerRepository,
        ILogger<GetAllOffersForReviewsAdminQueryHandler> logger
    )
    {
        _offerRepository = offerRepository;
        _logger = logger;
    }

    public async Task<Result<PaginatedList<ReviewOfferDTO>>> Handle(
        GetAllOffersForReviewsAdminQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var offers = await _offerRepository.GetAllOffersForReviewsAsync(
                request.SearchTerm,
                request.PageNumber,
                request.PageSize
            );

            return Result.Success(offers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching offers for admin review.");
            return Result.Failure<PaginatedList<ReviewOfferDTO>>(
                Errors.General.InvalidOperation("An error occurred while fetching the offers.")
            );
        }
    }
}
