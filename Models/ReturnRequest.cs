namespace alderaan_shop.Models;


// Demande de retour/remboursement liée à une commande.
public class ReturnRequest
{
    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public Guid UserId { get; init; }
    public string Reason { get; init; }
    public string Status { get; set; } 
    public DateTime RequestedAt { get; init; }
    public DateTime? ProcessedAt { get; set; }
    public string? AdminNotes { get; set; }
    
    // Navigation
    public Order Order { get; set; }
    public User User { get; set; }
    
    private ReturnRequest() {}
    
    // Crée une demande de retour pour une commande donnée.
    public ReturnRequest(
        Guid id,
        Guid orderId,
        Guid userId,
        Memory<char> reason
    )
    {
        Id = id;
        OrderId = orderId;
        UserId = userId;
        Reason = reason.ToString();
        Status = "Pending";
        RequestedAt = DateTime.UtcNow;
    }
}