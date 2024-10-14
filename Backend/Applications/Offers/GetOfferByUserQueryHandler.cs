using UGH.Domain.Interfaces;
using UGH.Domain.ViewModels;
using UGH.Domain.Core;
using MediatR;
using UGHApi.Shared;

namespace UGH.Application.Offers;

public class GetOfferByUserQueryHandler
    : IRequestHandler<GetOfferByUserQuery, Result<PaginatedList<OfferDTO>>>
{
    private readonly IOfferRepository _offerRepository;

    public GetOfferByUserQueryHandler(IOfferRepository offerRepository)
    {
        _offerRepository = offerRepository;
    }

    public async Task<Result<PaginatedList<OfferDTO>>> Handle(
        GetOfferByUserQuery request,
        CancellationToken cancellationToken
    )
    {
        var offers = await _offerRepository.GetUserOffersAsync(request.UserId, request.PageNumber, request.PageSize, request.SearchTerm);

        if (offers == null)
        {
            return Result.Failure<PaginatedList<OfferDTO>>(
                Errors.General.InvalidOperation("Offer not found for the user.")
            );
        }

        return Result.Success(offers);
    }
}