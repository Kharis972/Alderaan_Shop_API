namespace alderaan_shop.Models;

// Entité représentant une commande passée par un utilisateur 
public class Order
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public decimal TotalAmount { get; init; }
    public string Status { get; set; } // Pending, Processing, Shipped, Delivered, Cancelled
    public string ShippingAddress { get; init; }
    public string ShippingZipCode { get; init; }
    public string BillingAddress { get; init; }
    public string BillingZipCode { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string TrackingNumber { get; set; }
    
    // Navigation
    public User User { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public Payment Payment { get; set; }
    
    private Order() {}
    
    // Crée une commande utilisateur
    public Order(
        Guid id,
        Guid userId,
        decimal totalAmount,
        Memory<char> shippingAddress,
        Memory<char> shippingZipCode,
        Memory<char> billingAddress,
        Memory<char> billingZipCode
    )
    {
        Id = id;
        UserId = userId;
        TotalAmount = totalAmount;
        Status = "Pending";
        ShippingAddress = shippingAddress.ToString();
        ShippingZipCode = shippingZipCode.ToString();
        BillingAddress = billingAddress.ToString();
        BillingZipCode = billingZipCode.ToString();
        CreatedAt = DateTime.UtcNow;
        TrackingNumber = string.Empty;
    }
}