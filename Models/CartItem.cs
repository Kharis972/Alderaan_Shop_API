namespace alderaan_shop.Models;

// Élément d’un panier, associant un produit à une quantité.
public class CartItem
{
    public Guid Id { get; init; }
    public Guid CartId { get; init; }
    public Guid ProductId { get; init; }
    public int Quantity { get; set; }
    public DateTime AddedAt { get; init; }
    
    // Navigation
    public Cart Cart { get; set; }
    public Product Product { get; set; }
    
    private CartItem() {}
    
    // Crée une ligne de panier pour un produit donné.
    public CartItem(
        Guid id,
        Guid cartId,
        Guid productId,
        int quantity
    )
    {
        Id = id;
        CartId = cartId;
        ProductId = productId;
        Quantity = quantity;
        AddedAt = DateTime.UtcNow;
    }
}