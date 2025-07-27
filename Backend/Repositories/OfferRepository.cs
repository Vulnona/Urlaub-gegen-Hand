using Mapster;
using Microsoft.EntityFrameworkCore;
using UGH.Domain.Entities;
using UGH.Domain.ViewModels;
using UGHApi.Shared;
using UGHApi.ViewModels;
using UGHApi.ViewModels.UserComponent;
using ImageMagick;
using System.Security.Cryptography;
using UGHApi.DATA;

namespace UGHApi.Repositories;

public class OfferRepository
{
    private readonly Ugh_Context _context;    
    public OfferRepository(Ugh_Context context)
    {
        _context = context;
    }

    public async Task<Offer> GetOfferByIdAsync(int offerId)
    {
        return await _context.offers.Include(o => o.User).FirstOrDefaultAsync(o => o.Id == offerId);
    }
    
    public async Task<PaginatedList<OfferDTO>> GetOffersAsync(
        Guid userId,
        string searchTerm = null,
        int pageNumber = 1,
        int pageSize =10,
        bool forUser = false
    ) {    
        IQueryable<OfferTypeLodging> query = _context
            .offertypelodgings.Include(o => o.User).Include(o => o.Picture)
            .Include(o => o.OfferApplications).Include(o => o.Address)            
        .OrderBy(o => o.CreatedAt);

         if (forUser)
             query = query.Where(o => o.UserId == userId && o.Status != OfferStatus.Hidden);
         else
             query = query.Where(o => o.Status == OfferStatus.Active);
        if (!string.IsNullOrEmpty(searchTerm)) {
            query = query.Where(o =>
                o.Title.Contains(searchTerm)
                || o.Skills.Contains(searchTerm)
                || (o.Address != null && o.Address.DisplayName.Contains(searchTerm))
            );
        }

        int totalCount = await query.CountAsync();
        var offers = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        var offerDTOs = offers
            .Select(o => {
                var firstApplication = o.OfferApplications.FirstOrDefault(oa => oa.UserId == userId);                
                return new OfferDTO(o ,o.User, firstApplication);
            }).ToList();

        return PaginatedList<OfferDTO>.Create(offerDTOs, totalCount, pageNumber, pageSize);
    }

    public async Task<OfferApplication> GetOfferApplicationAsync(int offerId, Guid userId)
    {
        return await _context
            .offerapplication.Include(o => o.Offer)
            .FirstOrDefaultAsync(application =>
                application.OfferId == offerId && application.UserId == userId
            );
    }

    public async Task RemoveOfferAsync(int offerId)
    {
        var offer = await _context.offers.FindAsync(offerId);
        if (offer != null)
        {
            _context.offers.Remove(offer);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddOfferApplicationAsync(OfferApplication application) {
        await _context.offerapplication.AddAsync(application);
        await _context.SaveChangesAsync();
    }

    private async Task<bool> hasReview(int OfferId, bool isHost, Guid Guest, Guid Host){
        try {
            Guid reviewer, reviewed;
            if (isHost) {
                reviewer = Host;
                reviewed = Guest;
            } else {
                reviewer = Guest;
                reviewed = Host;
            }                                
            Review review = await _context.reviews.FirstOrDefaultAsync(r => r.OfferId == OfferId && r.ReviewerId == reviewer && r.ReviewedId == reviewed);
            if(review != null)
                return true;
            else
                return false;
        }
        catch {throw;}
    }
    public async Task<PaginatedList<OfferApplicationDto>> GetOfferApplicationsByUserAsync(Guid requestingUserId, int pageNumber, int pageSize, bool isHost) {
        try {
            IQueryable<OfferApplication> query = _context
                .offerapplication.Include(oa => oa.Offer).ThenInclude(Offer => Offer.Picture)
                .Include(oa => oa.User).Include(oa => oa.Host);
            if (isHost)
                query = query.Where(app => app.HostId == requestingUserId);
            else
                query = query.Where(app => app.UserId == requestingUserId);

            int totalCount = await query.CountAsync();
            var applications = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            List<OfferApplicationDto> applicationDtos = new List<OfferApplicationDto>();
            // select would be more performant but would need a way to call an async-function
            foreach(OfferApplication app in applications) {
                OfferApplicationDto o = new OfferApplicationDto {
                     OfferId = app.OfferId,
                     HostId = app.HostId,
                     Status = app.Status,
                     CreatedAt = app.CreatedAt.ToString("dd.MM.yyyy"),
                     HasReview = await hasReview(app.Offer.Id, isHost, app.User.User_Id, app.Host.User_Id),
                     OfferTitle = app.Offer.Title,
                     User = new UserC {
                         User_Id = isHost ? app.User.User_Id : app.Host.User_Id,
                         ProfilePicture = isHost ? app.User.ProfilePicture : app.Host.ProfilePicture,
                         FirstName = isHost ? app.User.FirstName : app.Host.FirstName,
                         LastName = isHost ? app.User.LastName : app.Host.LastName,
                     }
                };
                applicationDtos.Add(o);
            }
            return PaginatedList<OfferApplicationDto>.Create(applicationDtos, totalCount, pageNumber, pageSize);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<OfferDTO> GetOfferDetailsByIdAsync(int offerId, Guid userId)
    {
        var offer = await _context
            .offertypelodgings.Include(o => o.User)
            .Include(o => o.OfferApplications).Include(o => o.Picture).Include(o => o.Address)
            .FirstOrDefaultAsync(o => o.Id == offerId);

        if (offer == null)
            return null;
        bool applicationsExist = (offer.OfferApplications.FirstOrDefault() != null);
        if (userId != default(Guid)) {
            var applicationOfRequestingUser = offer.OfferApplications.FirstOrDefault(oa => oa.UserId == userId);
            return new OfferDTO(offer ,offer.User, applicationOfRequestingUser, applicationsExist);
        } else
            return new OfferDTO(offer , null, null, applicationsExist);
            
    }

    public async Task<PaginatedList<ReviewOfferDTO>> GetAllOffersForReviewsAsync(
        string searchTerm,
        int pageNumber,
        int pageSize
    )
    {
        IQueryable<OfferTypeLodging> query = _context.offertypelodgings.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(o =>
                o.Title.Contains(searchTerm)
                || o.Skills.Contains(searchTerm)
                || (o.Address != null && o.Address.DisplayName.Contains(searchTerm))
            );
        }

        query = query.OrderByDescending(o => o.CreatedAt);

        int totalCount = await query.CountAsync();

        var offers = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        var reviewOffersDto = offers.Adapt<List<ReviewOfferDTO>>();

        return PaginatedList<ReviewOfferDTO>.Create(
            reviewOffersDto,
            totalCount,
            pageNumber,
            pageSize
        );
    }

    // todo: generalize for different formats and other pictures (like profile pic)
    public async Task<Picture>AddPicture(byte[] data, User user){
            using (MagickImage image = new MagickImage(data)) {
                image.Thumbnail(new MagickGeometry(400));
                var format = MagickFormat.Jpg;
                var stream = new MemoryStream();
                image.Write(stream, format);
                stream.Position = 0;
                byte[] hashBytes = MD5.Create().ComputeHash(stream);
                String hash = BitConverter.ToString(hashBytes).Replace("-", "");
                Picture alreadyExisting = await _context.pictures.FirstOrDefaultAsync(p => p.Owner == user && p.Hash == hash);
                // a method for cleaning up unused pictures is still missing
                if (alreadyExisting != null)
                    return alreadyExisting;
                Picture p = new Picture{
                    ImageData = stream.ToArray(),
                    Width = 100,
                    Hash = hash,
                    Owner = user
                };
                await _context.pictures.AddAsync(p);
                await _context.SaveChangesAsync();
                return p;
            }

    }
}
