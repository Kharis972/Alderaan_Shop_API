using alderaan_shop.Models;

namespace alderaan_shop.Services.Interfaces;

/// <summary>
/// Service applicatif pour la newsletter.
/// Fournit des opérations de lecture dédiées (recherche par email, liste des abonnés actifs).
/// </summary>
public interface INewsletterService : IService<Newsletter>
{
    /// <summary>
    /// Retourne l'abonnement à la newsletter pour une adresse email exacte.
    /// </summary>
    /// <param name="email">Adresse email à rechercher.</param>
    /// <returns>L'entrée de newsletter si elle existe, sinon null.</returns>
    Task<Newsletter?> FindByEmailAsync(string email);

    /// <summary>
    /// Liste des abonnements encore actifs (IsSubscribed = true), ordonnés par date d'abonnement.
    /// </summary>
    /// <returns>Une séquence d'abonnements actifs.</returns>
    Task<IEnumerable<Newsletter>> GetSubscribedAsync();
}