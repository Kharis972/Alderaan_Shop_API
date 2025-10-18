namespace alderaan_shop.Models;

// Entité représentant un panier d'achat utilisateur.
public class Cart
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation
    public User User { get; set; }
    public ICollection<CartItem> CartItems { get; set; }
    
    private Cart() {}
    
    // Crée un panier vide pour un utilisateur.
    public Cart(Guid id, Guid userId)
    {
        Id = id;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}