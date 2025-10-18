using alderaan_shop.Models;

namespace alderaan_shop.Services.Interfaces;

/// <summary>
/// Service applicatif pour les notifications utilisateur.
/// Expose des méthodes pour récupérer et marquer comme lues les notifications.
/// </summary>
public interface INotificationService : IService<Notification>
{
    /// <summary>
    /// Retourne les notifications d'un utilisateur, les plus récentes en premier.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <param name="take">Nombre maximum d'éléments à retourner (par défaut 50).</param>
    /// <returns>Une séquence de notifications.</returns>
    Task<IEnumerable<Notification>> GetByUserAsync(Guid userId, int take = 50);

    /// <summary>
    /// Retourne uniquement les notifications non lues d'un utilisateur.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <returns>Une séquence de notifications non lues.</returns>
    Task<IEnumerable<Notification>> GetUnreadByUserAsync(Guid userId);

    /// <summary>
    /// Marque une notification comme lue.
    /// </summary>
    /// <param name="notificationId">Identifiant de la notification.</param>
    /// <returns>True si la notification a été marquée lue, sinon false.</returns>
    Task<bool> MarkAsReadAsync(Guid notificationId);
}