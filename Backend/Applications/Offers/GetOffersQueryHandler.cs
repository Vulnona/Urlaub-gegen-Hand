using UGH.Domain.ViewModels;
using UGH.Domain.Interfaces;
using UGH.Domain.Core;
using UGHApi.Shared;
using MediatR;

namespace UGH.Application.Offers;

public class GetAllOffersQueryHandler
    : IRequestHandler<GetOffersQuery, Result<PaginatedList<OfferDTO>>>
{
    private readonly IOfferRepository _offerRepository;
    private readonly ILogger<GetAllOffersQueryHandler> _logger;

    public GetAllOffersQueryHandler(
        IOfferRepository offerRepository,
        ILogger<GetAllOffersQueryHandler> logger
    )
    {
        _offerRepository = offerRepository;
        _logger = logger;
    }

    public async Task<Result<PaginatedList<OfferDTO>>> Handle(
        GetOffersQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var paginatedOffers = await _offerRepository.GetAllOfferByUserAsync(
                request.UserId,
                request.SearchTerm,
                request.PageNumber,
                request.PageSize
            );

            return Result.Success(paginatedOffers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure<PaginatedList<OfferDTO>>(Errors.General.UnexpectedError());
        }
    }
}
