namespace alderaan_shop.Models;

// Entité représentant une réduction applicable à un produit, une catégorie ou globale.
public class Discount
{
    public Guid Id { get; init; }
    public Guid? ProductId { get; init; }
    public Guid? CategoryId { get; init; } 
    public string DiscountType { get; init; }
    public decimal DiscountValue { get; init; }
    public DateTime ValidFrom { get; init; }
    public DateTime ValidUntil { get; init; }
    public bool IsActive { get; set; }
    
    // Navigation
    public Product? Product { get; set; }
    public Category? Category { get; set; }
    
    private Discount() {}
    
    // Crée une réduction applicable sur une portée donnée (produit, catégorie ou globale).
    public Discount(
        Guid id,
        Guid? productId,
        Guid? categoryId,
        Memory<char> discountType,
        decimal discountValue,
        DateTime validFrom,
        DateTime validUntil
    )
    {
        Id = id;
        ProductId = productId;
        CategoryId = categoryId;
        DiscountType = discountType.ToString();
        DiscountValue = discountValue;
        ValidFrom = validFrom;
        ValidUntil = validUntil;
        IsActive = true;
    }
}