using Microsoft.Extensions.Logging;
using UGH.Domain.ViewModels;
using UGH.Domain.Interfaces;
using UGH.Domain.Core;
using MediatR;

namespace UGH.Application.Offers;

public class GetOfferApplicationsByHostQueryHandler
    : IRequestHandler<GetOfferApplicationsByHostQuery, Result<List<OfferApplicationDto>>>
{
    private readonly IOfferRepository _offerRepository;
    private readonly ILogger<GetOfferApplicationsByHostQueryHandler> _logger;

    public GetOfferApplicationsByHostQueryHandler(
        IOfferRepository offerRepository,
        ILogger<GetOfferApplicationsByHostQueryHandler> logger
    )
    {
        _offerRepository = offerRepository;
        _logger = logger;
    }

    public async Task<Result<List<OfferApplicationDto>>> Handle(
        GetOfferApplicationsByHostQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var applications = await _offerRepository.GetOfferApplicationsByHostAsync(
                request.HostId
            );

            // Map entities to DTOs
            var applicationDtos = applications
                .Select(
                    app =>
                        new OfferApplicationDto
                        {
                            OfferId = app.OfferId,
                            HostId = app.HostId,
                            Status = app.Status,
                            CreatedAt = app.CreatedAt,
                            UpdatedAt = app.UpdatedAt,
                            Offer = new OfferDto
                            {
                                Id = app.Offer.Id,
                                Title = app.Offer.Title,
                                ImageData = app.Offer.ImageData,
                                ImageMimeType = app.Offer.ImageMimeType
                            },
                            User = new UserDto
                            {
                                User_Id = app.User.User_Id,
                                ProfilePicture = app.User.ProfilePicture,
                                FirstName = app.User.FirstName,
                                LastName = app.User.LastName,
                                Facebook_link = app.User.Facebook_link ?? "",
                                DateOfBirth = app.User.DateOfBirth,
                                Gender = app.User.Gender
                            }
                        }
                )
                .ToList();

            return Result.Success(applicationDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure<List<OfferApplicationDto>>(
                Errors.General.InvalidOperation(
                    "Something went wrong during fetching offer applications"
                )
            );
        }
    }
}
