using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

/// <summary>
/// Contrat du repository pour l'entité Wishlist.
/// </summary>
public interface IWishlistRepository : IRepository<Wishlist>
{
    Task<Wishlist?> GetByUserIdAsync(Guid userId);
    Task<Wishlist?> GetWithItemsAsync(Guid wishlistId);
}
