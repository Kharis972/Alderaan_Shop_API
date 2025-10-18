using alderaan_shop.Data;
using alderaan_shop.Helpers.CustomExceptions;
using alderaan_shop.Repositories.Interfaces;

namespace alderaan_shop.Services;

/// <summary>
/// Service applicatif générique pour les entités de domaine.
/// - Encapsule l'accès au repository générique <see cref="IRepository{T}"/>.
/// - Fournit des opérations courantes (lecture par Id, pagination) avec validations et gestion d'erreurs cohérentes.
/// - Centralise le logging des erreurs afin d'homogénéiser les messages côté API.
/// </summary>
/// <typeparam name="T">Type d'entité métier gérée par le service.</typeparam>
public class Service<T> where T : class
{
    /// <summary>
    /// Repository générique sous-jacent utilisé pour accéder aux données.
    /// </summary>
    protected IRepository<T> _repository;

    /// <summary>
    /// Logger typé du service.
    /// </summary>
    protected ILogger<Service<T>> Logger;

    /// <summary>
    /// Initialise une nouvelle instance du service générique.
    /// </summary>
    /// <param name="repository">Repository générique injecté pour l'accès aux données.</param>
    /// <param name="logger">Logger typé pour le suivi des opérations.</param>
    public Service(IRepository<T> repository, ILogger<Service<T>> logger)
    {
        _repository = repository;
        Logger = logger;
    }

    /// <summary>
    /// Récupère une entité par son identifiant unique.
    /// </summary>
    /// <typeparam name="TKey">Type de l'identifiant (autorisé: Guid, int).</typeparam>
    /// <param name="id">Identifiant unique de l'entité.</param>
    /// <returns>L'entité trouvée ou null si aucune correspondance.</returns>
    /// <exception cref="BadSqlRequestException">L'identifiant fourni n'est pas d'un type autorisé.</exception>
    /// <exception cref="DatabaseException">Erreur lors de l'accès à la base de données.</exception>
    /// <exception cref="InternalServerException">Erreur interne inattendue.</exception>
    public async Task<T?> GetEntityByIdAsync<TKey>(TKey id) where TKey : notnull
    {
        try
        {
            if (id is int or Guid)
                return await _repository.GetEntityByIdAsync(id);
            throw new BadSqlRequestException("Identifiant Unique invalide !");
        }
        catch (Exception e) when (e is DatabaseException or BadSqlRequestException)
        {
            // on relaie les exceptions métier/DB telles quelles
            throw;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Une erreur est survenue !");
            throw new InternalServerException("Une erreur interne est survenue !");
        }
    }

    /// <summary>
    /// Récupère une page d'entités depuis la source de données.
    /// </summary>
    /// <param name="index">Index de page (0-based).</param>
    /// <param name="entityNumber">Taille de page minimale (>= 2).</param>
    /// <returns>La séquence d'entités pour la page demandée.</returns>
    /// <exception cref="BadSqlRequestException">Paramètres de pagination invalides.</exception>
    /// <exception cref="DatabaseException">Erreur lors de l'accès à la base de données.</exception>
    /// <exception cref="InternalServerException">Erreur interne inattendue.</exception>
    public async Task<IEnumerable<T>> GetAllEntitiesWithPagingAsync(int index, int entityNumber)
    {
        try
        {
            if (index >= 0 && entityNumber >= 2)
                return await _repository.GetAllEntitiesWithPagingAsync(index, entityNumber);
            throw new BadSqlRequestException("La portée du message est incorecte.");
        }
        catch (Exception e) when (e is DatabaseException or BadSqlRequestException)
        {
            throw;
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Une erreur est survenue !");
            throw new InternalServerException("Une erreur interne est survenue !");
        }
    }
}