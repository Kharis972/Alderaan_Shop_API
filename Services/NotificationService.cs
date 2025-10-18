using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using alderaan_shop.Services.Interfaces;

namespace alderaan_shop.Services;

/// <summary>
/// Service de notifications.
/// </summary>
public class NotificationService : Service<Notification>, INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    /// <summary>
    /// Initialise le service des notifications.
    /// </summary>
    /// <param name="notificationRepository">Repository des notifications.</param>
    /// <param name="logger">Logger typé.</param>
    public NotificationService(INotificationRepository notificationRepository, ILogger<NotificationService> logger)
        : base(notificationRepository, logger)
    {
        _notificationRepository = notificationRepository;
    }

    /// <summary>
    /// Retourne les notifications d'un utilisateur (les plus récentes d'abord).
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <param name="take">Nombre maximum d'éléments (défaut 50).</param>
    /// <exception cref="ArgumentException">Si userId est vide.</exception>
    public Task<IEnumerable<Notification>> GetByUserAsync(Guid userId, int take = 50)
    {
        if (userId == Guid.Empty) throw new ArgumentException("Invalid userId", nameof(userId));
        return _notificationRepository.GetByUserAsync(userId, take);
    }

    /// <summary>
    /// Retourne les notifications non lues d'un utilisateur.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <exception cref="ArgumentException">Si userId est vide.</exception>
    public Task<IEnumerable<Notification>> GetUnreadByUserAsync(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException("Invalid userId", nameof(userId));
        return _notificationRepository.GetUnreadByUserAsync(userId);
    }

    /// <summary>
    /// Marque une notification comme lue.
    /// </summary>
    /// <param name="notificationId">Identifiant de la notification.</param>
    /// <exception cref="ArgumentException">Si notificationId est vide.</exception>
    public Task<bool> MarkAsReadAsync(Guid notificationId)
    {
        if (notificationId == Guid.Empty) throw new ArgumentException("Invalid notificationId", nameof(notificationId));
        return _notificationRepository.MarkAsReadAsync(notificationId);
    }
}