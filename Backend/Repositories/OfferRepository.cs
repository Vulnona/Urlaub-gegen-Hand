using Mapster;
using Microsoft.EntityFrameworkCore;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGH.Domain.ViewModels;
using UGHApi.Shared;
using UGHApi.ViewModels;
using UGHApi.ViewModels.UserComponent;

namespace UGH.Infrastructure.Repositories;

public class OfferRepository : IOfferRepository
{
    private readonly Ugh_Context _context;    
    public OfferRepository(Ugh_Context context)
    {
        _context = context;
    }

    public async Task<List<Offer>> GetAllOffersAsync(string searchTerm, Guid userId)
    {
        IQueryable<Offer> query = _context
            .offers.Include(o => o.User)
            .Include(o => o.Reviews)
            .Include(o => o.OfferApplications.Where(oa => oa.UserId == userId));

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(o =>
                o.Title.Contains(searchTerm)
                || o.skills.Contains(searchTerm)
                || o.Location.Contains(searchTerm)
                || o.state.Contains(searchTerm)
            );
        }

        return await query.ToListAsync();
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
        IQueryable<Offer> query = _context
            .offers.Include(o => o.User)
            .Include(o => o.Reviews)
            .Include(o => o.OfferApplications)
        .OrderBy(o => o.CreatedAt);

         if (forUser)
             query = query.Where(o => o.HostId == userId);
        if (!string.IsNullOrEmpty(searchTerm)) {
            query = query.Where(o =>
                o.Title.Contains(searchTerm)
                || o.skills.Contains(searchTerm)
                || o.Location.Contains(searchTerm)
                || o.state.Contains(searchTerm)
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

    public async Task AddOfferAsync(Offer offer)
    {
        _context.offers.Add(offer);
        await _context.SaveChangesAsync();
    }

    public async Task<OfferApplication> GetOfferApplicationAsync(int offerId, Guid userId)
    {
        return await _context
            .offerapplication.Include(o => o.Offer)
            .FirstOrDefaultAsync(application =>
                application.OfferId == offerId && application.UserId == userId
            );
    }

    public async Task<bool> UpdateOfferApplicationAsync(OfferApplication offerApplication)
    {
        _context.offerapplication.Update(offerApplication);
        return await _context.SaveChangesAsync() > 0;
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

    public async Task AddOfferApplicationAsync(OfferApplication application)
    {
        await _context.offerapplication.AddAsync(application);
        await _context.SaveChangesAsync();
    }

    public async Task<PaginatedList<OfferApplicationDto>> GetOfferApplicationsByHostAsync(
        Guid hostId,
        int pageNumber,
        int pageSize
    )
    {
        try
        {
            IQueryable<OfferApplication> query = _context
                .offerapplication.Include(oa => oa.Offer)
                .Include(oa => oa.User)
                .Where(app => app.HostId == hostId);

            int totalCount = await query.CountAsync();

            var applications = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var applicationDtos = applications
                .Select(app => new OfferApplicationDto
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
                    User = new UserC
                    {
                        User_Id = app.User.User_Id,
                        ProfilePicture = app.User.ProfilePicture,
                        FirstName = app.User.FirstName,
                        LastName = app.User.LastName,
                    },
                })
                .ToList();
            return PaginatedList<OfferApplicationDto>.Create(
                    applicationDtos,
                    totalCount,
                    pageNumber,
                    pageSize
            );
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<OfferDTO> GetOfferDetailsByIdAsync(int offerId, Guid userId)
    {
        var offer = await _context
            .offers.Include(o => o.User)
            .Include(o => o.Reviews)
            .Include(o => o.OfferApplications)
            .FirstOrDefaultAsync(o => o.Id == offerId);

        if (offer == null)
            return null;

        var firstApplication = offer.OfferApplications.FirstOrDefault(oa => oa.UserId == userId);                
        return new OfferDTO(offer ,offer.User, firstApplication);
    }

    public async Task<PaginatedList<ReviewOfferDTO>> GetAllOffersForReviewsAsync(
        string searchTerm,
        int pageNumber,
        int pageSize
    )
    {
        IQueryable<Offer> query = _context.offers.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(o =>
                o.Title.Contains(searchTerm)
                || o.skills.Contains(searchTerm)
                || o.Location.Contains(searchTerm)
                || o.state.Contains(searchTerm)
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
}
