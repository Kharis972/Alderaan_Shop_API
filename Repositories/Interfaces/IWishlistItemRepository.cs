using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

/// <summary>
/// Contrat du repository pour l'entité WishlistItem.
/// </summary>
public interface IWishlistItemRepository : IRepository<WishlistItem>
{
    Task<IEnumerable<WishlistItem>> GetByWishlistIdAsync(Guid wishlistId);
    Task<bool> ExistsAsync(Guid wishlistId, Guid productId);
}
