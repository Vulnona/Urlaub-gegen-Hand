using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UGH.Domain.ViewModels;
using UGH.Infrastructure.Services;
using UGHApi.Services.UserProvider;
using UGH.Domain.Interfaces;
using UGHApi.Shared;
using UGH.Domain.Entities;
using UGH.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace UGHApi.Controllers;

[Route("api/offer")]
[ApiController]
[Authorize]
public class OfferController : ControllerBase
{
    private readonly Ugh_Context _context;
    private readonly ILogger<OfferController> _logger;
    private readonly EmailService _emailService;
    private readonly IUserProvider _userProvider;
    private readonly OfferRepository _offerRepository;

    public OfferController(Ugh_Context context, ILogger<OfferController> logger,
        EmailService emailService, IUserProvider userProvider, OfferRepository offerRepository) {
        _context = context;
        _logger = logger;
        _emailService = emailService;
        _userProvider = userProvider;
        _offerRepository = offerRepository;
    }

    #region offers
    [HttpGet("get-all-offers")]
    public async Task<IActionResult> GetOffersAsync(string searchTerm, int pageNumber = 1, int pageSize = 10) {
        try {
            var userId = _userProvider.UserId;
            PaginatedList<OfferDTO> paginatedOffers =await _offerRepository.GetOffersAsync(userId, searchTerm, pageNumber, pageSize ,false);
            if (paginatedOffers == null)
            {
                return NotFound();
            }

            return Ok(
                new
                {
                    paginatedOffers.Items,
                    paginatedOffers.TotalCount,
                    paginatedOffers.PageNumber,
                    paginatedOffers.PageSize,
                    paginatedOffers.TotalPages,
                }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error.");
        }
    }
    
    [HttpGet("get-offer-by-user")]
    public async Task<IActionResult> GetOfferAsync(string searchTerm, int pageNumber = 1, int pageSize = 10) {
        try {
            var userId = _userProvider.UserId;
            var offers = await _offerRepository.GetOffersAsync(userId, searchTerm, pageNumber, pageSize, true);

            if (offers == null)           
                return NotFound();           

            return Ok(offers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error.");
        }
    }

    // if UserId == null a censored Offer will be displayed
    [AllowAnonymous]
    [HttpGet("get-offer-by-id/{OfferId:int}")]
    public async Task<IActionResult> GetOffer([Required] int OfferId)
    {
        try {
            var userId = _userProvider.UserId;
            _logger.LogError($"UserId: {userId}");
            var offer = await _offerRepository.GetOfferDetailsByIdAsync(OfferId, userId);            
            if (offer == null)
                return NotFound();

            return Ok(offer);
        }
        catch (Exception ex) {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error.");
        }
    }

    [HttpPost("add-new-offer")]
    public async Task<IActionResult> AddOffer([FromForm] OfferViewModel offerViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = _userProvider.UserId;            
            var user = await _context.users.Include(u => u.CurrentMembership).FirstOrDefaultAsync(u => u.User_Id == userId);
            
            if (user == null)
                return BadRequest("UserNotFound");            

            var offer = new OfferTypeLodging {
                Title = offerViewModel.Title,
                Description = offerViewModel.Description,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                Skills = offerViewModel.Skills,
                UserId = userId,
                Requirements = offerViewModel.AccommodationSuitable,                
                AdditionalLodgingProperties = offerViewModel.Accommodation,
                Location = offerViewModel.Location,
                Status = OfferStatus.Active,
                GroupProperties = "",
                FromDate = DateOnly.FromDateTime(DateTime.Parse(offerViewModel.FromDate)),
                ToDate = DateOnly.FromDateTime(DateTime.Parse(offerViewModel.ToDate))
            };

            if (offerViewModel.Image.Length > 0) {
                using var memoryStream = new MemoryStream();
                await offerViewModel.Image.CopyToAsync(memoryStream);                
                offer.Picture = await _offerRepository.AddPicture(memoryStream.ToArray(), user);
            }
            await _context.offers.AddAsync(offer);
            await _context.SaveChangesAsync();
            _logger.LogInformation("New Offer Added Successfully!");                        

            return Ok("New Offer Added Successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error.");
        }
    }

    // reimplement delete_offer


    [HttpPost("apply-offer")]
    public async Task<IActionResult> ApplyForOffer([Required] int offerId) {
        try {
            Guid userId = _userProvider.UserId;
            User user = await _context.users.FindAsync(userId);
            
            if (user.MembershipId == 0 || !user.IsEmailVerified)
                return BadRequest("NoCurrentMembership");
            
            var offer = await _offerRepository.GetOfferByIdAsync(offerId);
            if (offer == null)
                return BadRequest("OfferNotFound");
            if (offer.UserId == userId)
                return BadRequest("Host Cannot apply for own offer");

            var existingApplication = await _offerRepository.GetOfferApplicationAsync(offer.Id, userId);
            if (existingApplication != null)
                return BadRequest("Application already exists");

            var offerApplication = new OfferApplication {
                OfferId = offer.Id,
                UserId = userId,
                HostId = offer.UserId,
                Status = OfferApplicationStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _offerRepository.AddOfferApplicationAsync(offerApplication);
           
            string body = $"<p>Hallo {offer.User.FirstName ?? ""} {offer.User.LastName ?? ""},</p><br>"
                + $"<p>{user.FirstName} {user.LastName} hat sich auf dein Angebot {offer.Title} beworben.</p>" + "<br><p>Alles Gute wünscht,</p><p>das Team von Urlaub gegen Hand</p>";            
            await _emailService.SendEmailAsync(offer.User.Email_Address, $"Neue Bewerbung für {offer.Title}", body).ConfigureAwait(false);
            _logger.LogInformation("Application submitted successfully.");           
            return Ok("Application submitted successfully, and notification sent to the host.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error.");
        }
    }

    [HttpPut("update-application-status")]
    public async Task<IActionResult> UpdateApplicationStatus([Required] int offerId, [Required] Guid userId, [Required] bool isApproved) {
        var hostId = _userProvider.UserId;
        try {
            var offerApplication = await _offerRepository.GetOfferApplicationAsync(offerId, userId);
            if (offerApplication == null)
                return BadRequest("Offer application not found.");

            if (offerApplication.HostId != hostId)
                return BadRequest("You are not authorized to update the status of this application.");

            offerApplication.Status = isApproved ? OfferApplicationStatus.Approved : OfferApplicationStatus.Rejected;
            offerApplication.UpdatedAt = DateTime.UtcNow;

            _context.offerapplication.Update(offerApplication);
            var isUpdated = await _context.SaveChangesAsync() > 0;
            if (!isUpdated)
                return BadRequest("Failed to update offer application.");

            var user = await _context.users.FindAsync(userId);
            // replace with proper mail template
            if (user != null) {
                string userEmail = user.Email_Address;
                string status = isApproved ? "angenommen" : "abgelehnt";
                string subject = $"Bewerbung auf {offerApplication.Offer.Title} wurde {status}.";
                string body = $"<p>Hallo {user.FirstName ?? ""} {user.LastName ?? ""},</p><br>"
                + $"Deine Bewerbung auf das Angebot {offerApplication.Offer.Title} wurde {status}.</p>" + "<br><p>Alles Gute wünscht,</p><p>das Team von Urlaub gegen Hand</p>";            
            await _emailService.SendEmailAsync(user.Email_Address, subject, body).ConfigureAwait(false);                      
            }

            if (isApproved)
                return Ok("Approved");
            else
                return Ok("Rejected");
        } catch (Exception ex) {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error.");
        }
    }
    [HttpGet("offer-applications")]
    public async Task<IActionResult> GetOfferApplicationsByHost(int pageNumber, int pageSize, bool isHost) {
        try
        {
            var hostId = _userProvider.UserId;
            var offers = await _offerRepository.GetOfferApplicationsByUserAsync(hostId, pageNumber, pageSize, isHost);
            if (offers == null)
                return NotFound();
            return Ok(offers);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                $"Exception occurred while fetching offer applications: {ex.Message} | StackTrace: {ex.StackTrace}"
            );
            return StatusCode(
                500,
                "Internal server error occurred while fetching offer applications."
            );
        }
    }
    #endregion
}
