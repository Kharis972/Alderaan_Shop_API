namespace alderaan_shop.Models;

/// <summary>
/// Liste de souhaits (favoris) d’un utilisateur.
/// </summary>
public class Wishlist
{
    /// <summary>Identifiant unique de la wishlist.</summary>
    public Guid Id { get; init; }
    /// <summary>Identifiant de l'utilisateur propriétaire.</summary>
    public Guid UserId { get; init; }
    /// <summary>Date de création.</summary>
    public DateTime CreatedAt { get; init; }
    /// <summary>Date de dernière mise à jour.</summary>
    public DateTime UpdatedAt { get; set; }
    
    // Navigation
    public User User { get; set; }
    public ICollection<WishlistItem> WishlistItems { get; set; }
    
    private Wishlist() {}
    
    /// <summary>
    /// Crée une wishlist vide pour un utilisateur.
    /// </summary>
    public Wishlist(Guid id, Guid userId)
    {
        Id = id;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}