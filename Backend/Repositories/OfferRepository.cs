using Mapster;
using Microsoft.EntityFrameworkCore;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGH.Domain.ViewModels;
using UGHApi.Shared;
using UGHApi.ViewModels;

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

    public async Task<PaginatedList<OfferDTO>> GetUserOffersAsync(
        Guid userId,
        int pageNumber = 1,
        int pageSize = 10,
        string searchTerm = null
    )
    {
        IQueryable<Offer> query = _context
            .offers.Include(o => o.Reviews)
            .Include(o => o.OfferApplications)
            .Where(o => o.HostId == userId);

        if (!string.IsNullOrEmpty(searchTerm))
        {
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
            .Select(o =>
            {
                var firstApplication = o.OfferApplications.FirstOrDefault(oa =>
                    oa.UserId == userId
                );
                string appliedStatus;

                if (firstApplication == null)
                {
                    appliedStatus = "CanApply";
                }
                else
                {
                    appliedStatus = firstApplication.Status switch
                    {
                        OfferApplicationStatus.Pending => "Applied",
                        OfferApplicationStatus.Approved => "Approved",
                        OfferApplicationStatus.Rejected => "Rejected",
                        _ => "Unknown",
                    };
                }

                return new OfferDTO
                {
                    Id = o.Id,
                    ImageData = o.ImageData,
                    Title = o.Title,
                    Accomodation = o.Accomodation,
                    Accomodationsuitable = o.accomodationsuitable,
                    Skills = o.skills,
                    HostId = o.HostId,
                    AverageRating = o.AverageRating,
                    Location = o.Location ?? "",
                    Region = o.state ?? "",
                    AppliedStatus = appliedStatus,
                };
            })
            .ToList();

        return PaginatedList<OfferDTO>.Create(offerDTOs, totalCount, pageNumber, pageSize);
    }

    public async Task<PaginatedList<OfferDTO>> GetAllOfferByUserAsync(
        Guid userId,
        string searchTerm,
        int pageNumber,
        int pageSize
    )
    {
        IQueryable<Offer> query = _context
            .offers.Include(o => o.User)
            .Include(o => o.Reviews)
            .Include(o => o.OfferApplications);

        if (!string.IsNullOrEmpty(searchTerm))
        {
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
            .Select(o =>
            {
                var firstApplication = o.OfferApplications.FirstOrDefault(oa =>
                    oa.UserId == userId
                );
                string appliedStatus =
                    firstApplication == null
                        ? "CanApply"
                        : firstApplication.Status switch
                        {
                            OfferApplicationStatus.Pending => "Applied",
                            OfferApplicationStatus.Approved => "Approved",
                            OfferApplicationStatus.Rejected => "Rejected",
                            _ => "Unknown",
                        };

                return new OfferDTO
                {
                    Id = o.Id,
                    ImageData = o.ImageData,
                    Title = o.Title,
                    Accomodation = o.Accomodation,
                    Accomodationsuitable = o.accomodationsuitable,
                    Skills = o.skills,
                    HostId = o.HostId,
                    HostName = $"{o.User.FirstName} {o.User.LastName}",
                    AverageRating = o.AverageRating,
                    Location = o.Location ?? "",
                    Region = o.state ?? "",
                    AppliedStatus = appliedStatus,
                };
            })
            .ToList();

        return PaginatedList<OfferDTO>.Create(offerDTOs, totalCount, pageNumber, pageSize);
    }

    public async Task<PaginatedList<OfferDTO>> GetAllOfferForUnothorizeUserAsync(
        string searchTerm,
        int pageNumber,
        int pageSize
    )
    {
        IQueryable<Offer> query = _context.offers.Include(o => o.User).Include(o => o.Reviews);

        if (!string.IsNullOrEmpty(searchTerm))
        {
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
            .Select(o => new OfferDTO
            {
                Id = o.Id,
                ImageData = o.ImageData,
                Title = o.Title,
                Accomodation = o.Accomodation,
                Accomodationsuitable = o.accomodationsuitable,
                Skills = o.skills,
                HostId = null,
                HostName = o.User != null ? $"{o.User.FirstName} {o.User.LastName}" : "Unknown",
                AverageRating = o.AverageRating,
                Location = o.Location ?? "",
                Region = o.state ?? "",
                AppliedStatus = null,
            })
            .ToList();

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

    public async Task<PaginatedList<OfferApplication>> GetOfferApplicationsByHostAsync(
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

            return PaginatedList<OfferApplication>.Create(
                applications,
                totalCount,
                pageNumber,
                pageSize
            );
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<OfferResponse> GetOfferDetailsByIdAsync(int offerId)
    {
        var offer = await _context
            .offers.Include(o => o.User)
            .Include(o => o.Reviews)
            .FirstOrDefaultAsync(o => o.Id == offerId);

        if (offer == null)
        {
            return null;
        }

        var offerResponse = new OfferResponse
        {
            Id = offer.Id,
            Title = offer.Title,
            Description = offer.Description,
            Location = offer.Location,
            ImageData = offer.ImageData,
            ImageMimeType = offer.ImageMimeType,
            Contact = offer.Contact,
            Accomodation = offer.Accomodation,
            AccomodationSuitable = offer.accomodationsuitable,
            Skills = offer.skills,
            Country = offer.country,
            State = offer.state,
            City = offer.city,
            AverageRating = offer.AverageRating,
            User = new UserResponse
            {
                User_Id = offer.User.User_Id,
                FirstName = offer.User.FirstName,
                LastName = offer.User.LastName,
            },
        };

        return offerResponse;
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
