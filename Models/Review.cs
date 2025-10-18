namespace alderaan_shop.Models;

// Avis client sur un produit.
public class Review
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public Guid UserId { get; init; }
    public int Rating { get; init; } 
    public string Comment { get; init; }
    public bool IsVerifiedPurchase { get; init; }
    public DateTime CreatedAt { get; init; }
    
    // Navigation
    public Product Product { get; set; }
    public User User { get; set; }
    
    private Review() {}
    
    // Crée un avis pour un produit.
    public Review(
        Guid id,
        Guid productId,
        Guid userId,
        int rating,
        Memory<char> comment,
        bool isVerifiedPurchase
    )
    {
        Id = id;
        ProductId = productId;
        UserId = userId;
        Rating = rating;
        Comment = comment.ToString();
        IsVerifiedPurchase = isVerifiedPurchase;
        CreatedAt = DateTime.UtcNow;
    }
}