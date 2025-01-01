using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;
using UGH.Domain.ViewModels;
using UGHApi.Shared;

namespace UGH.Application.Offers;

public class GetOfferApplicationsByHostQueryHandler
    : IRequestHandler<GetOfferApplicationsByHostQuery, Result<PaginatedList<OfferApplicationDto>>>
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

    public async Task<Result<PaginatedList<OfferApplicationDto>>> Handle(
        GetOfferApplicationsByHostQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var paginatedApplications = await _offerRepository.GetOfferApplicationsByHostAsync(
                request.HostId,
                request.PageNumber,
                request.PageSize
            );

            var applicationDtos = paginatedApplications
                .Items.Select(app => new OfferApplicationDto
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
                        ImageMimeType = app.Offer.ImageMimeType,
                    },
                    User = new UserDto
                    {
                        User_Id = app.User.User_Id,
                        ProfilePicture = app.User.ProfilePicture,
                        FirstName = app.User.FirstName,
                        LastName = app.User.LastName,
                        Facebook_link = app.User.Facebook_link ?? "",
                        DateOfBirth = app.User.DateOfBirth,
                        Gender = app.User.Gender,
                    },
                })
                .ToList();

            return Result.Success(
                PaginatedList<OfferApplicationDto>.Create(
                    applicationDtos,
                    paginatedApplications.TotalCount,
                    request.PageNumber,
                    request.PageSize
                )
            );
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure<PaginatedList<OfferApplicationDto>>(
                Errors.General.InvalidOperation(
                    "Something went wrong during fetching offer applications"
                )
            );
        }
    }
}
