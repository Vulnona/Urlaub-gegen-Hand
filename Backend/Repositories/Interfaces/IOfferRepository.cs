using UGH.Domain.Entities;

namespace UGHApi.Repositories.Interfaces
{
    public interface IOfferRepository
    {
        Task<List<Offer>> GetAllOffersAsync(string searchTerm);
        Task<Offer> GetOfferByIdAsync(int offerId);
        Task AddOfferAsync(Offer offer);
        Task RemoveOfferAsync(int offerId);
        Task<OfferApplication> GetOfferApplicationAsync(int offerId, Guid userId);
        Task AddOfferApplicationAsync(OfferApplication application);
        Task<List<OfferApplication>> GetOfferApplicationsByHostAsync(Guid hostId);
    }
}
