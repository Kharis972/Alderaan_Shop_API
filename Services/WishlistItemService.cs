using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using alderaan_shop.Services.Interfaces;

namespace alderaan_shop.Services;

/// <summary>
/// Service des items de wishlist.
/// </summary>
public class WishlistItemService : Service<WishlistItem>, IWishlistItemService
{
    private readonly IWishlistItemRepository _wishlistItemRepository;

    public WishlistItemService(IWishlistItemRepository wishlistItemRepository, ILogger<WishlistItemService> logger)
        : base(wishlistItemRepository, logger)
    {
        _wishlistItemRepository = wishlistItemRepository;
    }

    public Task<IEnumerable<WishlistItem>> GetByWishlistIdAsync(Guid wishlistId)
    {
        if (wishlistId == Guid.Empty) throw new ArgumentException("Invalid wishlistId", nameof(wishlistId));
        return _wishlistItemRepository.GetByWishlistIdAsync(wishlistId);
    }

    public Task<bool> ExistsAsync(Guid wishlistId, Guid productId)
    {
        if (wishlistId == Guid.Empty) throw new ArgumentException("Invalid wishlistId", nameof(wishlistId));
        if (productId == Guid.Empty) throw new ArgumentException("Invalid productId", nameof(productId));
        return _wishlistItemRepository.ExistsAsync(wishlistId, productId);
    }
}