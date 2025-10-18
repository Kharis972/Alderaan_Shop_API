using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité Cart.
/// </summary>
public class CartRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<Cart>> logger)
    : Repository<Cart>(dbContext, logger), ICartRepository
{
    /// <summary>
    /// Retourne le panier d'un utilisateur donné.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <returns>Le panier de l'utilisateur ou null si aucun n'existe.</returns>
    public async Task<Cart?> GetByUserIdAsync(Guid userId)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.UserId == userId);
    }

    /// <summary>
    /// Retourne un panier avec ses éléments (CartItems) chargés.
    /// </summary>
    /// <param name="cartId">Identifiant du panier.</param>
    /// <returns>Le panier avec ses éléments ou null si introuvable.</returns>
    public async Task<Cart?> GetWithItemsAsync(Guid cartId)
    {
        return await DbContext.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.Id == cartId);
    }
}
