namespace alderaan_shop.Models;

// Expédition d’une commande (transporteur, suivi et statut).
public class Shipment
{
    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public string Carrier { get; init; } // DHL, UPS, FedEx, Colissimo, etc.
    public string TrackingNumber { get; init; }
    public string Status { get; set; } 
    public DateTime ShippedAt { get; init; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public DateTime? DeliveredAt { get; set; }
    
    // Navigation
    public Order Order { get; set; }
    
    private Shipment() {}
    
    // Crée une expédition pour une commande donnée.
    public Shipment(
        Guid id,
        Guid orderId,
        Memory<char> carrier,
        Memory<char> trackingNumber,
        DateTime? estimatedDeliveryDate
    )
    {
        Id = id;
        OrderId = orderId;
        Carrier = carrier.ToString();
        TrackingNumber = trackingNumber.ToString();
        Status = "Pending";
        ShippedAt = DateTime.UtcNow;
        EstimatedDeliveryDate = estimatedDeliveryDate;
    }
}