using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using alderaan_shop.Services.Interfaces;

namespace alderaan_shop.Services;

/// <summary>
/// Service des wishlists.
/// </summary>
public class WishlistService : Service<Wishlist>, IWishlistService
{
    private readonly IWishlistRepository _wishlistRepository;

    public WishlistService(IWishlistRepository wishlistRepository, ILogger<WishlistService> logger)
        : base(wishlistRepository, logger)
    {
        _wishlistRepository = wishlistRepository;
    }

    public Task<Wishlist?> GetByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException("Invalid userId", nameof(userId));
        return _wishlistRepository.GetByUserIdAsync(userId);
    }

    public Task<Wishlist?> GetWithItemsAsync(Guid wishlistId)
    {
        if (wishlistId == Guid.Empty) throw new ArgumentException("Invalid wishlistId", nameof(wishlistId));
        return _wishlistRepository.GetWithItemsAsync(wishlistId);
    }
}