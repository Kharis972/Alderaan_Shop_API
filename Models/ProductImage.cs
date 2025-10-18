namespace alderaan_shop.Models;

/// <summary>
/// Image associée à un produit avec ordre d’affichage.
/// </summary>
public class ProductImage
{
    /// <summary>Identifiant de l'image.</summary>
    public Guid Id { get; init; }
    /// <summary>Identifiant du produit.</summary>
    public Guid ProductId { get; init; }
    /// <summary>URL de l'image.</summary>
    public string ImageUrl { get; init; }
    /// <summary>Ordre d'affichage (plus petit = plus haut).</summary>
    public int DisplayOrder { get; set; }
    /// <summary>Indique si c'est l'image principale.</summary>
    public bool IsPrimary { get; set; }
    /// <summary>Date de création.</summary>
    public DateTime CreatedAt { get; init; }
    
    // Navigation
    public Product Product { get; set; }
    
    private ProductImage() {}
    
    /// <summary>
    /// Crée une image de produit.
    /// </summary>
    public ProductImage(
        Guid id,
        Guid productId,
        Memory<char> imageUrl,
        int displayOrder,
        bool isPrimary
    )
    {
        Id = id;
        ProductId = productId;
        ImageUrl = imageUrl.ToString();
        DisplayOrder = displayOrder;
        IsPrimary = isPrimary;
        CreatedAt = DateTime.UtcNow;
    }
}