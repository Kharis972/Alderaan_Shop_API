using alderaan_shop.DTOs.Order;

namespace alderaan_shop.Services.Interfaces;

/// <summary>
/// Contrat du service de commande.
/// Orchestration de la création de commande, gestion du stock et persistance transactionnelle.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Crée une commande, décrémente les stocks, persiste la transaction et retourne l'identifiant et l'état de paiement.
    /// </summary>
    Task<(Guid OrderId, string PaymentStatus)> PlaceOrderAsync(CreateOrderDTO dto);
}