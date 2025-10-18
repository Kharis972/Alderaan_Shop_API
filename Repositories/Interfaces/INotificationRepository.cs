using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

/// <summary>
/// Contrat du repository pour l'entité Notification.
/// </summary>
public interface INotificationRepository : IRepository<Notification>
{
    Task<IEnumerable<Notification>> GetByUserAsync(Guid userId, int take = 50);
    Task<IEnumerable<Notification>> GetUnreadByUserAsync(Guid userId);
    Task<bool> MarkAsReadAsync(Guid notificationId);
}
