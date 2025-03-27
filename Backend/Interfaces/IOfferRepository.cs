using UGH.Domain.Entities;
using UGH.Domain.ViewModels;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGH.Domain.Interfaces;
#nullable enable
public interface IOfferRepository
{
    Task<List<Offer>> GetAllOffersAsync(string searchTerm, Guid userId);
    Task<PaginatedList<OfferDTO>> GetOffersAsync(
        Guid userId,
        string? searchTerm,
        int pageNumber,
        int pageSize,
        bool forUser
    );

    Task<PaginatedList<ReviewOfferDTO>> GetAllOffersForReviewsAsync(
        string searchTerm,
        int pageNumber,
        int pageSize
    );
    Task<Offer> GetOfferByIdAsync(int offerId);
    Task AddOfferAsync(Offer offer);
    Task RemoveOfferAsync(int offerId);
    Task<OfferApplication> GetOfferApplicationAsync(int offerId, Guid userId);
    Task AddOfferApplicationAsync(OfferApplication application);
    Task<PaginatedList<OfferApplication>> GetOfferApplicationsByHostAsync(
        Guid hostId,
        int pageNumber,
        int pageSize
    );
    Task<bool> UpdateOfferApplicationAsync(OfferApplication offerApplication);
    Task<OfferDTO> GetOfferDetailsByIdAsync(int offerId, Guid userId);
}
#nullable disable
