using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;
using UGH.Domain.ViewModels;
using UGHApi.Shared;

namespace UGH.Application.Offers;

public class GetAllOffersQueryHandler : IRequestHandler<GetOffersQuery, Result>
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

    public async Task<Result> Handle(GetOffersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            PaginatedList<OfferDTO> paginatedOffers;

            if (request.UserId == null || request.UserId == Guid.Empty)
            {
                paginatedOffers = await _offerRepository.GetAllOfferForUnothorizeUserAsync(
                    request.SearchTerm,
                    request.PageNumber,
                    request.PageSize
                );
            }
            else
            {
                paginatedOffers = await _offerRepository.GetAllOfferByUserAsync(
                    request.UserId.Value,
                    request.SearchTerm,
                    request.PageNumber,
                    request.PageSize
                );
            }

            return Result.Success(paginatedOffers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(Errors.General.UnexpectedError());
        }
    }
}
