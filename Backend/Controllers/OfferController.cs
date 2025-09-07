using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UGH.Domain.ViewModels;
using UGH.Infrastructure.Services;
using UGHApi.Services.UserProvider;
using UGH.Domain.Interfaces;
using UGHApi.Shared;
using UGH.Domain.Entities;
using UGHApi.Repositories;
using Microsoft.EntityFrameworkCore;
using UGHApi.DATA;

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
    [HttpGet("debug-offers")]
    [AllowAnonymous]
    public async Task<IActionResult> DebugOffers() {
        try {
            var allOffers = await _context.offertypelodgings
                .Include(o => o.User)
                .Include(o => o.Address)
                .ToListAsync();
            
            var result = allOffers.Select(o => new {
                Id = o.Id,
                Title = o.Title,
                Status = o.Status,
                UserId = o.UserId,
                UserName = o.User?.FirstName + " " + o.User?.LastName,
                Address = o.Address?.DisplayName,
                CreatedAt = o.CreatedAt
            }).ToList();
            
            _logger.LogInformation("DebugOffers found {Count} offers", result.Count);
            return Ok(new { totalCount = result.Count, offers = result });
        }
        catch (Exception ex) {
            _logger.LogError($"DebugOffers exception: {ex.Message}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("test-simple")]
    [AllowAnonymous]
    public async Task<IActionResult> TestSimpleOffers()
    {
        try
        {
            // Einfachste mögliche Query - nur die Offers ohne Includes
            var offers = await _context.offertypelodgings
                .Take(5)
                .Select(o => new { 
                    o.Id, 
                    o.Title, 
                    o.GroupSize,
                    o.Mobility,
                    o.Status,
                    o.OfferType
                })
                .ToListAsync();
            
            return Ok(offers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"TestSimpleOffers exception: {ex.Message}");
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpGet("test-pictures")]
    [AllowAnonymous]
    public async Task<IActionResult> TestPicturesInclude()
    {
        try
        {
            // Test nur Pictures Include
            var offers = await _context.offertypelodgings
                .Include(o => o.Pictures)
                .Take(2)
                .Select(o => new { 
                    o.Id, 
                    o.Title,
                    PictureCount = o.Pictures.Count()
                })
                .ToListAsync();
            
            return Ok(offers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"TestPicturesInclude exception: {ex.Message}");
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpGet("get-all-offers")]
    public async Task<IActionResult> GetOffersAsync(
        string searchTerm, 
        int pageNumber = 1, 
        int pageSize = 10, 
        [FromQuery] string includeInactive = "false",
        [FromQuery] string sortBy = "newest",
        [FromQuery] double? latitude = null,
        [FromQuery] double? longitude = null,
        [FromQuery] double? radiusKm = null
    ) {
        try {
            var userId = _userProvider.UserId;
            
            // Convert string to bool
            bool includeInactiveBool = includeInactive?.ToLower() == "true";
            
            PaginatedList<OfferDTO> paginatedOffers = await _offerRepository.GetOffersAsync(
                userId, 
                searchTerm, 
                pageNumber, 
                pageSize, 
                false, 
                includeInactiveBool,
                sortBy,
                latitude,
                longitude,
                radiusKm
            );
            if (paginatedOffers == null)
            {
                _logger.LogWarning("GetOffersAsync returned null");
                return NotFound();
            }

            _logger.LogInformation("GetOffersAsync returning {ItemCount} items, total: {TotalCount}", paginatedOffers.Items?.Count ?? 0, paginatedOffers.TotalCount);

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
    public async Task<IActionResult> GetOfferAsync(string searchTerm, int pageNumber = 1, int pageSize = 10, [FromQuery] string sortBy = "newest", [FromQuery] string includeInactive = "false") {
        try {
            var userId = _userProvider.UserId;
            _logger.LogInformation("GetOfferAsync (user offers) called - userId: {UserId}, searchTerm: {SearchTerm}, pageNumber: {PageNumber}, sortBy: {SortBy}, includeInactive: {IncludeInactive}", userId, searchTerm, pageNumber, sortBy, includeInactive);
            bool includeInactiveBool = includeInactive?.ToLower() == "true";
            var paginatedOffers = await _offerRepository.GetOffersAsync(userId, searchTerm, pageNumber, pageSize, true, includeInactiveBool, sortBy);
            if (paginatedOffers == null) {
                _logger.LogWarning("GetOfferAsync (user offers) returned null");
                return NotFound();           
            }
            _logger.LogInformation("GetOfferAsync (user offers) returning {ItemCount} items, total: {TotalCount}", paginatedOffers.Items?.Count ?? 0, paginatedOffers.TotalCount);
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

    // if UserId is default a censored Offer will be displayed
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

    // url for the preview picture
    [AllowAnonymous]
    [HttpGet("get-preview-picture/{OfferId:int}")]
    public async Task<IActionResult> GetPreviewPicture([Required] int OfferId){
        try {
        var offer = await _context.offers.Include(o => o.Pictures).FirstOrDefaultAsync(o => o.Id == OfferId);
        if (offer != null)
            return File(offer.Pictures.FirstOrDefault().ImageData, "image/jpeg");
        else
            return BadRequest("OfferNotFound");
        } catch (Exception ex) {
            _logger.LogError($"Exception occurred fetching preview picture: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error.");
        } 
    }
    
    // Multi-Bild-Unterstützung: Bilder werden über Images Array im DTO bereitgestellt

    public class OfferMeta{
        public int Id {get; set;}
        public String Title {get; set;}
        public String Description {get; set;}
    };

    [HttpPut("put-offer")]
    public async Task<IActionResult> PutOffer([FromForm] OfferViewModel offerViewModel)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        Picture[] pictures = new Picture[8];
        OfferTypeLodging offer;
        try
        {
            var userId = _userProvider.UserId;
            // Prüfe maximale Anzahl aktiver Angebote
            var activeCount = await _context.offers.CountAsync(o => o.UserId == userId && o.Status == OfferStatus.Active);
            if (offerViewModel.OfferId == -1 && activeCount >= 12) {
                return BadRequest("Du hast bereits die maximale Anzahl (12) aktiver Angebote. Bitte deaktiviere ein Angebot, bevor du ein neues erstellst.");
            }
            
            if (offerViewModel.OfferId == -1) {
                var recentDuplicate = await _context.offers
                    .Where(o => o.UserId == userId && 
                                o.Title == offerViewModel.Title &&
                                o.Description == offerViewModel.Description &&
                                o.CreatedAt >= DateOnly.FromDateTime(DateTime.UtcNow.AddMinutes(-5)))
                    .FirstOrDefaultAsync();

                if (recentDuplicate != null) {
                    return BadRequest("Ein identisches Angebot wurde bereits vor kurzem erstellt. Bitte warte einen Moment oder ändere Titel/Beschreibung.");
                }
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.users.Include(u => u.CurrentMembership).FirstOrDefaultAsync(u => u.User_Id == userId);
            
            if (user == null)
                return BadRequest("UserNotFound");            

            int offerId = offerViewModel.OfferId ?? -1;            
            if (offerId != -1) {
                offer = await _context.offertypelodgings
                    .Include(o => o.OfferApplications)
                    .Include(o => o.Pictures) 
                    .FirstOrDefaultAsync(o => o.Id == offerId);
                if (offer == null)
                    return BadRequest("OfferNotFound");
                if (offer.UserId != userId)
                    return StatusCode(403, "Forbidden.");
                var firstApplication = offer.OfferApplications.FirstOrDefault();
                if (firstApplication != null)
                    return StatusCode(412, "Can't modify an offer if applications exists.");
            } else {                                    
                offer = new OfferTypeLodging {
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    UserId = userId
                };
            }
            
            offer.Title = offerViewModel.Title;
            offer.Description = offerViewModel.Description;
            offer.Skills = offerViewModel.Skills;
            offer.Requirements = offerViewModel.AccommodationSuitable;
            offer.AdditionalLodgingProperties = offerViewModel.Accommodation;
            
            // Create Address entity from geographic location data
            _logger.LogInformation("Creating address with DisplayName: '{DisplayName}', Lat: {Lat}, Lon: {Lon}", 
                offerViewModel.DisplayName, offerViewModel.Latitude, offerViewModel.Longitude);
            
            var address = new Address
            {
                Latitude = offerViewModel.Latitude,
                Longitude = offerViewModel.Longitude,
                DisplayName = offerViewModel.DisplayName
            };
            
            // Add address to context 
            await _context.addresses.AddAsync(address);
            offer.Address = address;
            
            offer.Status = OfferStatus.Active;
            offer.GroupProperties = "";
            offer.FromDate = DateOnly.FromDateTime(DateTime.Parse(offerViewModel.FromDate));
            offer.ToDate = DateOnly.FromDateTime(DateTime.Parse(offerViewModel.ToDate));

            // Multi-Picture - Process images before saving the offer
            if (offerViewModel.Images != null && offerViewModel.Images.Length > 0) {
                // For existing offers: Add new images to the existing list
                if (offer.Pictures == null) {
                    offer.Pictures = new List<Picture>();
                }
                
                bool hasValidImages = offer.Pictures.Count > 0; // Already existing images are valid
                List<Exception> imageErrors = new List<Exception>();
                
                for (int i = 0; i < offerViewModel.Images.Length && i < 8; i++) {
                    var imageFile = offerViewModel.Images[i];
                    if (imageFile != null && imageFile.Length > 0) {
                        try {
                            using var memoryStream = new MemoryStream();
                            await imageFile.CopyToAsync(memoryStream);
                            
                            // Verwende die neue AddPicture-Methode mit OfferId
                            var picture = await _offerRepository.AddPicture(memoryStream.ToArray(), user, offerId == -1 ? null : offerId);
                            
                            // Navigation Properties korrekt setzen
                            picture.Offer = offer;
                            if (i == 0) {
                                offer.Pictures.Add(picture);
                                await _context.pictures.AddAsync(picture);
                            }
                            else  
                                pictures[i] = picture;                            
                            
                            hasValidImages = true;
                            
                            _logger.LogInformation($"Successfully processed image {i} for offer {offerId}");
                        } catch (Exception ex) {
                            _logger.LogError($"Failed to process image {i}: {ex.Message}");
                            imageErrors.Add(ex);
                            // Continue with other images instead of failing completely
                        }
                    }
                }
                
                // Log all image errors
                if (imageErrors.Count > 0) {
                    _logger.LogWarning($"Failed to process {imageErrors.Count} images: {string.Join(", ", imageErrors.Select(e => e.Message))}");
                }
                
                // Validate that at least one valid image was processed
                if (!hasValidImages && offerId == -1) {
                    await transaction.RollbackAsync();
                    return BadRequest("Mindestens ein gültiges Bild ist erforderlich");
                }
            } else if (offerId == -1) {
                await transaction.RollbackAsync();
                return BadRequest("Mindestens ein Bild ist erforderlich");
            }
            
            // Process existing images (for updates)
            if (offerId != -1 && offerViewModel.ExistingImages != null && offerViewModel.ExistingImages.Length > 0) {
                // Ensure the existing images are correctly assigned
                foreach (var existingImageId in offerViewModel.ExistingImages) {
                    if (int.TryParse(existingImageId, out int pictureId)) {
                        var existingPicture = offer.Pictures?.FirstOrDefault(p => p.Id == pictureId);
                        if (existingPicture != null) {
                            // Stelle sicher, dass die OfferId korrekt gesetzt ist
                            existingPicture.OfferId = offerId;
                        }
                    }
                }
            }
            
            // Save the offer after processing images
            if(offerId == -1) {
                await _context.offers.AddAsync(offer);
            }
            
            // Save all in a transaction
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            _logger.LogInformation("New Offer Added Successfully!");                        
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error.");
        }
        // pictures[0] is always null
        try {
            for (int i = 1; i<8; i++){
                if (pictures[i] == null)
                    continue;
                offer.Pictures.Add(pictures[i]);
                await _context.pictures.AddAsync(pictures[i]);
            }
            await _context.SaveChangesAsync();
            return Ok("New Offer Added Successfully!");
        } catch (Exception ex) {
            _logger.LogError($"Adding additional pictures failed: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error.");
        }
        
    }

    [HttpPut("close-offer/{OfferId:int}")]
    public async Task<IActionResult> CloseOffer([Required] int offerId)
    {
        try {
            Guid userId = _userProvider.UserId;
            User user = await _context.users.FindAsync(userId);
            var offer = await _offerRepository.GetOfferByIdAsync(offerId);
            if (offer.UserId == userId) {
                if (offer.Status == OfferStatus.Active){
                    offer.Status = OfferStatus.Closed;
                    await _context.SaveChangesAsync();
                    return Ok();
                } else {
                    return StatusCode(400, "Offer not active");
                }
            } else
                return StatusCode(403, "Forbidden.");
        } catch (Exception ex)
        {
            _logger.LogError($"Exception occurred closing offer: {ex.Message} | StackTrace: {ex.StackTrace}");
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
            if (offer.Status == OfferStatus.Closed)
                return BadRequest("Offer is closed.");
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
                + $"<p>{user.FirstName} {user.LastName} hat sich auf dein Angebot {offer.Title} beworben.</p>" + "<br><p>Alles Gute w�nscht,</p><p>das Team von Urlaub gegen Hand</p>";            
            await _emailService.SendEmailAsync(offer.User.Email_Address, $"Neue Bewerbung f�r {offer.Title}", body).ConfigureAwait(false);
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
                + $"Deine Bewerbung auf das Angebot {offerApplication.Offer.Title} wurde {status}.</p>" + "<br><p>Alles Gute w�nscht,</p><p>das Team von Urlaub gegen Hand</p>";            
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

    [HttpPost("deactivate-all-by-user")]
    public async Task<IActionResult> DeactivateAllByUser()
    {
        try
        {
            var userId = _userProvider.UserId;
            var offers = await _context.offers.Where(o => o.UserId == userId && o.Status == OfferStatus.Active).ToListAsync();
            foreach (var offer in offers)
            {
                offer.Status = OfferStatus.Closed;
            }
            await _context.SaveChangesAsync();
            return Ok(new { deactivated = offers.Count });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred in DeactivateAllByUser: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error.");
        }
    }

    [HttpPut("reactivate/{OfferId:int}")]
    public async Task<IActionResult> ReactivateOffer([Required] int OfferId)
    {
        try
        {
            var userId = _userProvider.UserId;
            var offer = await _context.offers.FirstOrDefaultAsync(o => o.Id == OfferId && o.UserId == userId);
            if (offer == null)
                return NotFound("Offer not found or not owned by user.");
            if (offer.Status != OfferStatus.Closed)
                return BadRequest("Offer is not closed.");
            if (offer.ToDate < DateOnly.FromDateTime(DateTime.UtcNow))
                return BadRequest("Offer period has already ended. Cannot reactivate.");
            offer.Status = OfferStatus.Active;
            await _context.SaveChangesAsync();
            return Ok("Offer reactivated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred in ReactivateOffer: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error.");
        }
    }
    #endregion

    [HttpDelete("delete-picture/{pictureId}")]
    public async Task<IActionResult> DeletePicture(int pictureId)
    {
        try
        {
            var userId = _userProvider.UserId;
            var picture = await _context.pictures.FindAsync(pictureId);
            if (picture == null)
                return NotFound();
            // Nur Besitzer darf löschen
            if (picture.UserId != userId)
                return Forbid();
            _context.pictures.Remove(picture);
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred deleting picture: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("debug-pictures/{offerId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> DebugPictures(int offerId)
    {
        try {
            var offer = await _context.offertypelodgings
                .Include(o => o.Pictures)
                .FirstOrDefaultAsync(o => o.Id == offerId);
            
            if (offer == null) {
                return NotFound($"Offer {offerId} not found");
            }
            
            var pictureInfo = new List<object>();
            if (offer.Pictures != null) {
                pictureInfo = offer.Pictures.Select(p => new {
                    Id = p.Id,
                    Hash = p.Hash,
                    OfferId = p.OfferId,
                    UserId = p.UserId,
                    Width = p.Width,
                    ImageDataLength = p.ImageData?.Length ?? 0,
                    HasValidOfferId = p.OfferId == offerId
                }).Cast<object>().ToList();
            }
            
            var result = new {
                OfferId = offer.Id,
                OfferTitle = offer.Title,
                TotalPictures = pictureInfo.Count,
                Pictures = pictureInfo,
                HasPictures = offer.Pictures?.Any() ?? false,
                PicturesLoaded = offer.Pictures != null
            };
            
            _logger.LogInformation($"DebugPictures for offer {offerId}: {pictureInfo.Count} pictures found");
            return Ok(result);
        }
        catch (Exception ex) {
            _logger.LogError($"DebugPictures exception: {ex.Message}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    [HttpPost("fix-pictures/{offerId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> FixPictures(int offerId)
    {
        try {
            await _offerRepository.EnsureOfferPicturesConsistencyAsync(offerId);
            return Ok($"Pictures for offer {offerId} have been fixed");
        }
        catch (Exception ex) {
            _logger.LogError($"FixPictures exception: {ex.Message}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
