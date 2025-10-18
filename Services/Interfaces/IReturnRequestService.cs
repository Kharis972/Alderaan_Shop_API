using alderaan_shop.Models;

namespace alderaan_shop.Services.Interfaces;

/// <summary>
/// Service applicatif pour les demandes de retour.
/// </summary>
public interface IReturnRequestService : IService<ReturnRequest>
{
    /// <summary>
    /// Retourne les demandes de retour d'un utilisateur.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <returns>Une séquence de demandes, ordonnées par date décroissante.</returns>
    Task<IEnumerable<ReturnRequest>> GetByUserAsync(Guid userId);

    /// <summary>
    /// Retourne les demandes de retour pour une commande donnée.
    /// </summary>
    /// <param name="orderId">Identifiant de la commande.</param>
    /// <returns>Une séquence de demandes de retour.</returns>
    Task<IEnumerable<ReturnRequest>> GetByOrderAsync(Guid orderId);

    /// <summary>
    /// Met à jour le statut d'une demande de retour.
    /// </summary>
    /// <param name="returnRequestId">Identifiant de la demande.</param>
    /// <param name="status">Nouveau statut (Pending, Approved, Rejected, Refunded).</param>
    /// <param name="adminNotes">Notes administrateur optionnelles.</param>
    /// <returns>True si la mise à jour a été effectuée, sinon false.</returns>
    Task<bool> UpdateStatusAsync(Guid returnRequestId, string status, string? adminNotes = null);
}