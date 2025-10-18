namespace alderaan_shop.Models;

// Représente un élément (ligne) d'une commande: un produit, sa quantité et les prix associés.
public class OrderItem
{
    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal TotalPrice { get; init; }
    public Order Order { get; set; }
    public Product Product { get; set; }
    
    private OrderItem() {}
    
    // Crée une nouvelle ligne de commande.
    /// <param name="id">Identifiant unique de la ligne.</param>
    /// <param name="orderId">Identifiant de la commande parente.</param>
    /// <param name="productId">Identifiant du produit concerné.</param>
    /// <param name="quantity">Quantité commandée (doit être &gt; 0).</param>
    /// <param name="unitPrice">Prix unitaire du produit au moment de l'achat.</param>
    public OrderItem(
        Guid id,
        Guid orderId,
        Guid productId,
        int quantity,
        decimal unitPrice
    )
    {
        Id = id;
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        // Le prix total est dérivé de la quantité multipliée par le prix unitaire.
        TotalPrice = quantity * unitPrice;
    }
}