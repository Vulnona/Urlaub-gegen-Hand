using Microsoft.AspNetCore.Mvc;
using UGH.Domain.ViewModels;

namespace UGH.Infrastructure.Services
{
    public interface IOfferService
    {
        Task<IActionResult> GetOffersAsync(string searchTerm);
        Task<IActionResult> GetOfferAsync(int offerId);
        Task<IActionResult> AddOfferAsync(OfferViewModel offerViewModel, Guid userId);
        Task<IActionResult> DeleteOfferAsync(int offerId);
        Task<IActionResult> ApplyForOfferAsync(int offerId, string email);
        Task<IActionResult> GetOfferApplicationsByHostAsync(Guid hostId, int pageNumber, int pageSize);
    }
}
