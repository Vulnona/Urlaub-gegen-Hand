using MediatR;
using Microsoft.Extensions.Logging;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;

namespace UGH.Application.Offers;

public class AddOfferCommandHandler : IRequestHandler<AddOfferCommand, Result>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AddOfferCommandHandler> _logger;

    public AddOfferCommandHandler(
        IOfferRepository offerRepository,
        IUserRepository userRepository,
        ILogger<AddOfferCommandHandler> logger
    )
    {
        _offerRepository = offerRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(AddOfferCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var offerViewModel = request.OfferViewModel;
            var user = await _userRepository.GetUserWithMembershipAsync(offerViewModel.User_Id);

            if (user == null)
            {
                return Result.Failure(Errors.General.NotFound("UserNotFound", user));
            }

            if (user.CurrentMembership == null)
            {
                return Result.Failure(
                    Errors.General.NotFound("MembershipNotFound", user.CurrentMembership)
                );
            }

            bool isLocationProvided = !string.IsNullOrWhiteSpace(offerViewModel.Location);
            bool isCountryStateCityProvided =
                !string.IsNullOrWhiteSpace(offerViewModel.Country)
                && !string.IsNullOrWhiteSpace(offerViewModel.State)
                && !string.IsNullOrWhiteSpace(offerViewModel.City);

            if (!(isLocationProvided ^ isCountryStateCityProvided))
            {
                return Result.Failure(Errors.General.NotFound("UserNotFound", user));
            }

            var offer = new Offer
            {
                Title = offerViewModel.Title,
                Description = offerViewModel.Description,
                Location = offerViewModel.Location,
                Contact = offerViewModel.Contact,
                Accomodation = offerViewModel.Accommodation,
                accomodationsuitable = offerViewModel.AccommodationSuitable,
                skills = offerViewModel.Skills,
                HostId = offerViewModel.User_Id,
                country = offerViewModel.Country,
                state = offerViewModel.State,
                city = offerViewModel.City,
            };

            if (offerViewModel.Image.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await offerViewModel.Image.CopyToAsync(memoryStream);
                offer.ImageData = memoryStream.ToArray();
                offer.ImageMimeType = offerViewModel.Image.ContentType;
            }

            await _offerRepository.AddOfferAsync(offer);
            _logger.LogInformation("New Offer Added Successfully!");

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return Result.Failure(Errors.General.InvalidOperation("Something went wrong"));
        }
    }
}
