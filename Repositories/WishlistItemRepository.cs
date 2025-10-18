using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité WishlistItem.
/// </summary>
public class WishlistItemRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<WishlistItem>> logger)
    : Repository<WishlistItem>(dbContext, logger), IWishlistItemRepository
{
    public async Task<IEnumerable<WishlistItem>> GetByWishlistIdAsync(Guid wishlistId)
    {
        return await DbSet.Where(wi => wi.WishlistId == wishlistId).ToListAsync();
    }

    public async Task<bool> ExistsAsync(Guid wishlistId, Guid productId)
    {
        return await DbSet.AnyAsync(wi => wi.WishlistId == wishlistId && wi.ProductId == productId);
    }
}
