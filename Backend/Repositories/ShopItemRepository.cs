using Microsoft.EntityFrameworkCore;
using UGHApi.Entities;
using UGH.Domain.Interfaces;
using UGHApi.Shared;
using UGHApi.DATA;

namespace UGHApi.Repositories;

public class ShopItemRepository : IShopItemRepository
{
    private readonly Ugh_Context _context;

    public ShopItemRepository(Ugh_Context context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(ShopItem shopItem)
    {
        _context.shopitems.Add(shopItem);
        await _context.SaveChangesAsync();
        return shopItem.Id;
    }

    public async Task UpdateAsync(ShopItem shopItem)
    {
        var existingItem = await _context.shopitems.FindAsync(shopItem.Id);

        if (existingItem == null)
            throw new Exception("Item not found");

        existingItem.Name = shopItem.Name;
        existingItem.Price = shopItem.Price;
        existingItem.Description = shopItem.Description;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var shopItem = await _context.shopitems.FindAsync(id);

        if (shopItem == null)
            throw new Exception("ShopItem not found");

        _context.shopitems.Remove(shopItem);
        await _context.SaveChangesAsync();
    }

    public async Task<ShopItem> GetByIdAsync(int id)
    {
        return await _context.shopitems.FindAsync(id);
    }

    public async Task<PaginatedList<ShopItem>> GetAllAsync(int pageNumber, int pageSize)
    {
        try
        {
            IQueryable<ShopItem> query = _context.shopitems;

            int totalCount = await query.CountAsync();

            var shopItems = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return PaginatedList<ShopItem>.Create(shopItems, totalCount, pageNumber, pageSize);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
