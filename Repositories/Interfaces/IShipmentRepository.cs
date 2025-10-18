using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

/// <summary>
/// Contrat du repository pour l'entité Shipment.
/// </summary>
public interface IShipmentRepository : IRepository<Shipment>
{
    Task<Shipment?> GetByOrderIdAsync(Guid orderId);
    Task<Shipment?> GetByTrackingIdAsync(string trackingId);
    Task<bool> UpdateStatusAsync(Guid shipmentId, string status);
}
