using UGH.Domain.ViewModels;
using UGH.Domain.Entities;
using UGHApi.Shared;

namespace UGH.Domain.Interfaces;

public interface IOfferRepository
{
    Task<List<Offer>> GetAllOffersAsync(string searchTerm, Guid userId);
    Task<PaginatedList<OfferDTO>> GetAllOfferByUserAsync(
        Guid userId,
        string searchTerm,
        int pageNumber,
        int pageSize
    ); 
    Task<PaginatedList<OfferDTO>> GetAllOfferForUnothorizeUserAsync(
        string searchTerm,
        int pageNumber,
        int pageSize
    );
    Task<Offer> GetOfferByIdAsync(int offerId);
    Task AddOfferAsync(Offer offer);
    Task RemoveOfferAsync(int offerId);
    Task<OfferApplication> GetOfferApplicationAsync(int offerId, Guid userId);
    Task<PaginatedList<OfferDTO>> GetUserOffersAsync(Guid userId, int pageNumber = 1, int pageSize = 10, string searchTerm = null);
    Task AddOfferApplicationAsync(OfferApplication application);
    Task<List<OfferApplication>> GetOfferApplicationsByHostAsync(Guid hostId);
    Task<bool> UpdateOfferApplicationAsync(OfferApplication offerApplication);
    Task<OfferResponse> GetOfferDetailsByIdAsync(int offerId);
}
