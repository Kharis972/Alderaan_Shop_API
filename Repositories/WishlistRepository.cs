using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité Wishlist.
/// </summary>
public class WishlistRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<Wishlist>> logger)
    : Repository<Wishlist>(dbContext, logger), IWishlistRepository
{
    public async Task<Wishlist?> GetByUserIdAsync(Guid userId)
    {
        return await DbSet.FirstOrDefaultAsync(w => w.UserId == userId);
    }

    public async Task<Wishlist?> GetWithItemsAsync(Guid wishlistId)
    {
        return await DbContext.Wishlists
            .Include(w => w.WishlistItems)
            .FirstOrDefaultAsync(w => w.Id == wishlistId);
    }
}
