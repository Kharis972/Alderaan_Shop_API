using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité Shipment.
/// </summary>
public class ShipmentRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<Shipment>> logger)
    : Repository<Shipment>(dbContext, logger), IShipmentRepository
{
    public async Task<Shipment?> GetByOrderIdAsync(Guid orderId)
    {
        return await DbSet.FirstOrDefaultAsync(s => s.OrderId == orderId);
    }

    public async Task<Shipment?> GetByTrackingIdAsync(string trackingId)
    {
        return await DbSet.FirstOrDefaultAsync(s => s.TrackingNumber == trackingId);
    }

    public async Task<bool> UpdateStatusAsync(Guid shipmentId, string status)
    {
        Shipment? shipment = await DbSet.FirstOrDefaultAsync(s => s.Id == shipmentId);
        if (shipment == null) return false;
        shipment.Status = status;
        if (status == "Delivered")
        {
            shipment.DeliveredAt = DateTime.UtcNow;
        }
        DbSet.Update(shipment);
        await DbContext.SaveChangesAsync();
        return true;
    }
}
