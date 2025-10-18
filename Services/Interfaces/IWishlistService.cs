using alderaan_shop.Models;

namespace alderaan_shop.Services.Interfaces;

/// <summary>
/// Service applicatif pour les wishlists.
/// </summary>
public interface IWishlistService : IService<Wishlist>
{
    /// <summary>
    /// Récupère la wishlist d'un utilisateur.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <returns>La wishlist si elle existe, sinon null.</returns>
    Task<Wishlist?> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Récupère une wishlist avec ses items associés chargés.
    /// </summary>
    /// <param name="wishlistId">Identifiant de la wishlist.</param>
    /// <returns>La wishlist avec ses items, sinon null.</returns>
    Task<Wishlist?> GetWithItemsAsync(Guid wishlistId);
}