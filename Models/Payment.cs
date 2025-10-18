namespace alderaan_shop.Models;

// Entité représentant un paiement lié à une commande.
public class Payment
{
    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public decimal Amount { get; init; }
    public string PaymentMethod { get; init; }
    public string Status { get; set; } 
    public string TransactionId { get; init; } 
    public DateTime CreatedAt { get; init; }
    public DateTime? CompletedAt { get; set; }
    
    // Navigation
    public Order Order { get; set; }
    
    private Payment() {}
    
    // Crée un paiement pour une commande donnée.
    public Payment(
        Guid id,
        Guid orderId,
        decimal amount,
        Memory<char> paymentMethod,
        Memory<char> transactionId
    )
    {
        Id = id;
        OrderId = orderId;
        Amount = amount;
        PaymentMethod = paymentMethod.ToString();
        Status = "Pending";
        TransactionId = transactionId.ToString();
        CreatedAt = DateTime.UtcNow;
    }
}