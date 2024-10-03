using MediatR;
using Microsoft.Extensions.Logging;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;

namespace UGH.Application.Offers;

public class DeleteOfferCommandHandler : IRequestHandler<DeleteOfferCommand, Result>
{
    private readonly IOfferRepository _offerRepository;
    private readonly ILogger<DeleteOfferCommandHandler> _logger;

    public DeleteOfferCommandHandler(
        IOfferRepository offerRepository,
        ILogger<DeleteOfferCommandHandler> logger
    )
    {
        _offerRepository = offerRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeleteOfferCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var offer = await _offerRepository.GetOfferByIdAsync(request.OfferId);
            if (offer == null)
            {
                return Result.Failure(Errors.General.NotFound("OfferNotFound", offer));
            }

            await _offerRepository.RemoveOfferAsync(request.OfferId);
            _logger.LogInformation($"Offer with ID {request.OfferId} deleted successfully.");

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(
                Errors.General.InvalidOperation("Something went wrong while deleting offer.")
            );
        }
    }
}
