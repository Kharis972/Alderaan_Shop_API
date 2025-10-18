using alderaan_shop.Models;

namespace alderaan_shop.Services.Interfaces;

/// <summary>
/// Service applicatif pour l'historique de recherche.
/// </summary>
public interface ISearchHistoryService : IService<SearchHistory>
{
    /// <summary>
    /// Retourne les dernières recherches d'un utilisateur.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <param name="take">Nombre maximum d'éléments à retourner (défaut 50).</param>
    /// <returns>Une séquence d'entrées d'historique.</returns>
    Task<IEnumerable<SearchHistory>> GetByUserAsync(Guid userId, int take = 50);

    /// <summary>
    /// Retourne les termes les plus recherchés sur une fenêtre simple (count total).
    /// </summary>
    /// <param name="top">Nombre de termes à remonter (défaut 10).</param>
    /// <returns>Une séquence (Terme, Compteur).</returns>
    Task<IEnumerable<(string Term, int Count)>> GetTopTermsAsync(int top = 10);

    /// <summary>
    /// Ajoute un terme dans l'historique d'un utilisateur.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <param name="term">Terme recherché.</param>
    /// <returns>Tâche asynchrone complétée.</returns>
    Task AddTermAsync(Guid userId, string term);
}