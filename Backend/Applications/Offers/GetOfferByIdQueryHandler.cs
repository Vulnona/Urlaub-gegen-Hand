using MediatR;
using Microsoft.Extensions.Logging;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGH.Domain.ViewModels;

namespace UGH.Application.Offers;

public class GetOfferByIdQueryHandler
: IRequestHandler<GetOfferByIdQuery, Result<OfferResponse>>
{
    private readonly IOfferRepository _offerRepository;
    private readonly ILogger<GetOfferByIdQueryHandler> _logger;

    public GetOfferByIdQueryHandler(
        IOfferRepository offerRepository,
        ILogger<GetOfferByIdQueryHandler> logger
    )
    {
        _offerRepository = offerRepository;
        _logger = logger;
    }

    public async Task<Result<OfferResponse>> Handle(
        GetOfferByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var offer = await _offerRepository.GetOfferDetailsByIdAsync(request.OfferId);

            if (offer == null)
            {
                return Result.Failure<OfferResponse>(
                    Errors.General.NotFound("Offer", request.OfferId)
                );
            }

            return Result.Success(offer);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure<OfferResponse>(Errors.General.UnexpectedError());
        }
    }
}
