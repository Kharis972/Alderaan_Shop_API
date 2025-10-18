using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using alderaan_shop.Services.Interfaces;

namespace alderaan_shop.Services;

/// <summary>
/// Service des demandes de retour.
/// </summary>
public class ReturnRequestService : Service<ReturnRequest>, IReturnRequestService
{
    private readonly IReturnRequestRepository _returnRequestRepository;

    /// <summary>
    /// Initialise le service des demandes de retour.
    /// </summary>
    /// <param name="returnRequestRepository">Repository des demandes de retour.</param>
    /// <param name="logger">Logger typé.</param>
    public ReturnRequestService(IReturnRequestRepository returnRequestRepository, ILogger<ReturnRequestService> logger)
        : base(returnRequestRepository, logger)
    {
        _returnRequestRepository = returnRequestRepository;
    }

    /// <summary>
    /// Retourne les demandes de retour d'un utilisateur.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <exception cref="ArgumentException">Si userId est vide.</exception>
    public Task<IEnumerable<ReturnRequest>> GetByUserAsync(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException("Invalid userId", nameof(userId));
        return _returnRequestRepository.GetByUserAsync(userId);
    }

    /// <summary>
    /// Retourne les demandes de retour d'une commande.
    /// </summary>
    /// <param name="orderId">Identifiant de la commande.</param>
    /// <exception cref="ArgumentException">Si orderId est vide.</exception>
    public Task<IEnumerable<ReturnRequest>> GetByOrderAsync(Guid orderId)
    {
        if (orderId == Guid.Empty) throw new ArgumentException("Invalid orderId", nameof(orderId));
        return _returnRequestRepository.GetByOrderAsync(orderId);
    }

    /// <summary>
    /// Met à jour le statut d'une demande de retour.
    /// </summary>
    /// <param name="returnRequestId">Identifiant de la demande.</param>
    /// <param name="status">Nouveau statut.</param>
    /// <param name="adminNotes">Notes administrateur optionnelles.</param>
    /// <exception cref="ArgumentException">Si returnRequestId ou status est invalide.</exception>
    public Task<bool> UpdateStatusAsync(Guid returnRequestId, string status, string? adminNotes = null)
    {
        if (returnRequestId == Guid.Empty) throw new ArgumentException("Invalid returnRequestId", nameof(returnRequestId));
        if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Status requis", nameof(status));
        return _returnRequestRepository.UpdateStatusAsync(returnRequestId, status, adminNotes);
    }
}