namespace alderaan_shop.Models;

public class WishlistItem
{
    public Guid Id { get; init; }
    public Guid WishlistId { get; init; }
    public Guid ProductId { get; init; }
    public DateTime AddedAt { get; init; }
    
    // Navigation
    public Wishlist Wishlist { get; set; }
    public Product Product { get; set; }
    
    private WishlistItem() {}
    
    public WishlistItem(
        Guid id,
        Guid wishlistId,
        Guid productId
    )
    {
        Id = id;
        WishlistId = wishlistId;
        ProductId = productId;
        AddedAt = DateTime.UtcNow;
    }
}