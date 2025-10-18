using alderaan_shop.Models;

namespace alderaan_shop.Services.Interfaces;

/// <summary>
/// Service applicatif pour les items de wishlist.
/// </summary>
public interface IWishlistItemService : IService<WishlistItem>
{
    /// <summary>
    /// Retourne les items d'une wishlist.
    /// </summary>
    /// <param name="wishlistId">Identifiant de la wishlist.</param>
    /// <returns>La séquence d'items.</returns>
    Task<IEnumerable<WishlistItem>> GetByWishlistIdAsync(Guid wishlistId);

    /// <summary>
    /// Indique si un produit est déjà présent dans une wishlist.
    /// </summary>
    /// <param name="wishlistId">Identifiant de la wishlist.</param>
    /// <param name="productId">Identifiant du produit.</param>
    /// <returns>True si présent, sinon false.</returns>
    Task<bool> ExistsAsync(Guid wishlistId, Guid productId);
}