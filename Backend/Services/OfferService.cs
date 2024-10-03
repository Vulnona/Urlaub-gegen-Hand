using Microsoft.AspNetCore.Mvc;
using UGH.Domain.Entities;
using UGH.Domain.ViewModels;
using UGHApi.Repositories.Interfaces;
using UGHApi.Services.Interfaces;

namespace UGHApi.Services;

public class OfferService : IOfferService
{
    private readonly IOfferRepository _offerRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<OfferService> _logger;
    private readonly EmailService _emailService;

    public OfferService(IOfferRepository offerRepository, ILogger<OfferService> logger, EmailService emailService, IUserRepository userRepository)
    {
        _offerRepository = offerRepository;
        _logger = logger;
        _emailService = emailService;
        _userRepository = userRepository;
    }

    public async Task<IActionResult> GetOffersAsync(string searchTerm)
    {
        try
        {
            var result = await _offerRepository.GetAllOffersAsync(searchTerm);
            if (result == null || result.Count == 0)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return new StatusCodeResult(500);
        }
    }

    public async Task<IActionResult> GetOfferAsync(int offerId)
    {
        try
        {
            var offer = await _offerRepository.GetOfferByIdAsync(offerId);
            if (offer == null)
                return new NotFoundResult();
            return new OkObjectResult(offer);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return new StatusCodeResult(500);
        }
    }

    public async Task<IActionResult> AddOfferAsync(OfferViewModel offerViewModel)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(offerViewModel.User_Id);
            if (user == null)
                return new BadRequestObjectResult("User not found.");

            if (user.CurrentMembership == null)
                return new BadRequestObjectResult("User is not authorized to add an offer.");

            bool isLocationProvided = !string.IsNullOrWhiteSpace(offerViewModel.Location);
            bool isCountryStateCityProvided = !string.IsNullOrWhiteSpace(offerViewModel.Country) &&
                                               !string.IsNullOrWhiteSpace(offerViewModel.State) &&
                                               !string.IsNullOrWhiteSpace(offerViewModel.City);

            if (!(isLocationProvided ^ isCountryStateCityProvided))
            {
                return new BadRequestObjectResult("You must provide either a Location or all of Country, State, and City.");
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
                User_Id = offerViewModel.User_Id,
                country = offerViewModel.Country,
                state = offerViewModel.State,
                city = offerViewModel.City,
            };

            if (offerViewModel.Image != null && offerViewModel.Image.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await offerViewModel.Image.CopyToAsync(memoryStream);
                offer.ImageData = memoryStream.ToArray();
                offer.ImageMimeType = offerViewModel.Image.ContentType;
            }

            await _offerRepository.AddOfferAsync(offer);
            _logger.LogInformation("New Offer Added Successfully!");
            return new OkResult();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return new StatusCodeResult(500);
        }
    }

    public async Task<IActionResult> DeleteOfferAsync(int offerId)
    {
        try
        {
            var offer = await _offerRepository.GetOfferByIdAsync(offerId);
            if (offer == null)
                return new NotFoundObjectResult("Offer not found.");

            await _offerRepository.RemoveOfferAsync(offerId);
            return new OkObjectResult(offer);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return new StatusCodeResult(500);
        }
    }

    public async Task<IActionResult> ApplyForOfferAsync(int offerId, string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return new BadRequestObjectResult("Email is required.");
        }

        try
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return new NotFoundObjectResult("User with the provided email does not exist.");

            if (user.CurrentMembership == null || !user.IsEmailVerified)
                return new BadRequestObjectResult("User is not authorized to apply for an offer.");

            var offer = await _offerRepository.GetOfferByIdAsync(offerId);
            if (offer == null)
                return new NotFoundObjectResult($"Offer not found with the provided offerId: {offerId}");

            if (offer.User_Id == user.User_Id)
                return new BadRequestObjectResult("Cannot apply for your own offer");

            var existingApplication = await _offerRepository.GetOfferApplicationAsync(offerId, user.User_Id);
            if (existingApplication != null)
                return new ConflictObjectResult("An application by this user for this offer already exists.");

            var offerApplication = new OfferApplication
            {
                OfferId = offer.Id,
                UserId = user.User_Id,
                HostId = offer.User_Id,
                Status = OfferApplicationStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _offerRepository.AddOfferApplicationAsync(offerApplication);

            string hostEmail = offer.User.Email_Address;
            string subject = "New Application for Your Offer";
            string body = $"<p>Dear {offer.User.FirstName} {offer.User.LastName},</p>" +
                          $"<p>Your offer has received a new application from {user.FirstName} {user.LastName}.</p>" +
                          "<p>Thank you for using our service!</p>";

            await _emailService.SendEmailAsync(hostEmail, subject, body);

            return new OkObjectResult("Application submitted successfully, and notification sent to the host.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return new StatusCodeResult(500);
        }
    }

    public async Task<IActionResult> GetOfferApplicationsByHostAsync(int hostId)
    {
        try
        {
            var pendingApplications = await _offerRepository.GetOfferApplicationsByHostAsync(hostId);
            if (pendingApplications == null || pendingApplications.Count == 0)
            {
                return new NotFoundObjectResult("No pending applications found for the provided Host ID.");
            }
            return new OkObjectResult(pendingApplications);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred while fetching offer applications: {ex.Message} | StackTrace: {ex.StackTrace}");
            return new StatusCodeResult(500);
        }
    }
}
