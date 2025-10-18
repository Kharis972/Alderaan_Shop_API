using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using alderaan_shop.Services.Interfaces;

namespace alderaan_shop.Services;

/// <summary>
/// Service des expéditions.
/// </summary>
public class ShipmentService : Service<Shipment>, IShipmentService
{
    private readonly IShipmentRepository _shipmentRepository;

    public ShipmentService(IShipmentRepository shipmentRepository, ILogger<ShipmentService> logger)
        : base(shipmentRepository, logger)
    {
        _shipmentRepository = shipmentRepository;
    }

    public Task<Shipment?> GetByOrderIdAsync(Guid orderId)
    {
        if (orderId == Guid.Empty) throw new ArgumentException("Invalid orderId", nameof(orderId));
        return _shipmentRepository.GetByOrderIdAsync(orderId);
    }

    public Task<Shipment?> GetByTrackingIdAsync(string trackingId)
    {
        if (string.IsNullOrWhiteSpace(trackingId)) throw new ArgumentException("TrackingId requis", nameof(trackingId));
        return _shipmentRepository.GetByTrackingIdAsync(trackingId);
    }

    public Task<bool> UpdateStatusAsync(Guid shipmentId, string status)
    {
        if (shipmentId == Guid.Empty) throw new ArgumentException("Invalid shipmentId", nameof(shipmentId));
        if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Status requis", nameof(status));
        return _shipmentRepository.UpdateStatusAsync(shipmentId, status);
    }
}