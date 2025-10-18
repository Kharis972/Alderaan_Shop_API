using alderaan_shop.Models;

namespace alderaan_shop.Services.Interfaces;

/// <summary>
/// Service applicatif pour les expéditions.
/// </summary>
public interface IShipmentService : IService<Shipment>
{
    /// <summary>
    /// Récupère une expédition par identifiant de commande.
    /// </summary>
    /// <param name="orderId">Identifiant de la commande.</param>
    /// <returns>L'expédition si elle existe, sinon null.</returns>
    Task<Shipment?> GetByOrderIdAsync(Guid orderId);

    /// <summary>
    /// Récupère une expédition par numéro de suivi.
    /// </summary>
    /// <param name="trackingId">Numéro de suivi du transporteur.</param>
    /// <returns>L'expédition si elle existe, sinon null.</returns>
    Task<Shipment?> GetByTrackingIdAsync(string trackingId);

    /// <summary>
    /// Met à jour le statut d'une expédition.
    /// </summary>
    /// <param name="shipmentId">Identifiant de l'expédition.</param>
    /// <param name="status">Nouveau statut (Pending, InTransit, Delivered, Failed).</param>
    /// <returns>True si la mise à jour a réussi, sinon false.</returns>
    Task<bool> UpdateStatusAsync(Guid shipmentId, string status);
}