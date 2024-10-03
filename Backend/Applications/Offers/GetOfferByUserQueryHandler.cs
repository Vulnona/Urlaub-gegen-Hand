using UGH.Domain.Interfaces;
using UGH.Domain.ViewModels;
using UGH.Domain.Core;
using MediatR;

namespace UGH.Application.Offers;

public class GetOfferByUserQueryHandler
    : IRequestHandler<GetOfferByUserQuery, Result<List<OfferDTO>>>
{
    private readonly IOfferRepository _offerRepository;

    public GetOfferByUserQueryHandler(IOfferRepository offerRepository)
    {
        _offerRepository = offerRepository;
    }

    public async Task<Result<List<OfferDTO>>> Handle(
        GetOfferByUserQuery request,
        CancellationToken cancellationToken
    )
    {
        var offers = await _offerRepository.GetUserOffersAsync(request.UserId);

        if (offers == null)
        {
            return Result.Failure<List<OfferDTO>>(
                Errors.General.InvalidOperation("Offer not found for the user.")
            );
        }

        return Result.Success(offers);
    }
}