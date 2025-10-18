using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

/// <summary>
/// Contrat du repository pour l'entité ReturnRequest.
/// </summary>
public interface IReturnRequestRepository : IRepository<ReturnRequest>
{
    Task<IEnumerable<ReturnRequest>> GetByUserAsync(Guid userId);
    Task<IEnumerable<ReturnRequest>> GetByOrderAsync(Guid orderId);
    Task<bool> UpdateStatusAsync(Guid returnRequestId, string status, string? adminNotes = null);
}
