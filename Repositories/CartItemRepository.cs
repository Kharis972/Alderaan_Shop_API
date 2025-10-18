using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité CartItem.
/// </summary>
public class CartItemRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<CartItem>> logger)
    : Repository<CartItem>(dbContext, logger), ICartItemRepository
{
    public async Task<IEnumerable<CartItem>> GetByCartIdAsync(Guid cartId)
    {
        return await DbSet.Where(ci => ci.CartId == cartId).ToListAsync();
    }

    public async Task<IEnumerable<CartItem>> GetByProductIdAsync(Guid productId)
    {
        return await DbSet.Where(ci => ci.ProductId == productId).ToListAsync();
    }
}
