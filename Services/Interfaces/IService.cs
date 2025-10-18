namespace alderaan_shop.Services.Interfaces;

/// <summary>
/// Contrat générique des services applicatifs.
/// Définit des opérations de lecture communes (par Id, pagination) avec contraintes de validation.
/// Les implémentations doivent encapsuler la logique métier et déléguer l'accès aux données à un repository.
/// </summary>
/// <typeparam name="T">Type d'entité gérée par le service.</typeparam>
public interface IService<T> where T : class
{
    /// <summary>
    /// Récupère une entité par identifiant unique.
    /// </summary>
    /// <typeparam name="TKey">Type de l'identifiant (Guid ou int).</typeparam>
    /// <param name="id">Identifiant unique.</param>
    /// <returns>L'entité correspondante ou null si introuvable.</returns>
    Task<T?> GetEntityByIdAsync<TKey>(TKey id) where TKey : notnull;

    /// <summary>
    /// Récupère une page d'entités.
    /// </summary>
    /// <param name="index">Index de page (0-based).</param>
    /// <param name="entityNumber">Nombre d'entités par page (>= 2).</param>
    /// <returns>La séquence d'entités correspondant à la page demandée.</returns>
    Task<IEnumerable<T>> GetAllEntitiesWithPagingAsync(int index, int entityNumber);
}