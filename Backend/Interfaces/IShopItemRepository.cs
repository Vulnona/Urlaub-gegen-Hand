using UGHApi.Entities;
using UGHApi.Shared;

namespace UGH.Domain.Interfaces;

public interface IShopItemRepository
{
    Task<int> CreateAsync(ShopItem shopItem);
    Task UpdateAsync(ShopItem shopItem);
    Task DeleteAsync(int id);
    Task<ShopItem> GetByIdAsync(int id);
    Task<PaginatedList<ShopItem>> GetAllAsync(int pageNumber, int pageSize);
}
