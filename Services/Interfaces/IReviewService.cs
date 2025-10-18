using alderaan_shop.Models;

namespace alderaan_shop.Services.Interfaces;

/// <summary>
/// Service applicatif pour les avis produits.
/// Fournit des méthodes de lecture et d'agrégation sur les avis.
/// </summary>
public interface IReviewService : IService<Review>
{
    /// <summary>
    /// Récupère les avis pour un produit donné.
    /// </summary>
    /// <param name="productId">Identifiant du produit.</param>
    /// <returns>La liste des avis, récents d'abord.</returns>
    Task<IEnumerable<Review>> GetByProductAsync(Guid productId);

    /// <summary>
    /// Récupère les avis rédigés par un utilisateur.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <returns>La liste des avis.</returns>
    Task<IEnumerable<Review>> GetByUserAsync(Guid userId);

    /// <summary>
    /// Calcule la note moyenne d'un produit.
    /// </summary>
    /// <param name="productId">Identifiant du produit.</param>
    /// <returns>La note moyenne (0.0 si aucun avis).</returns>
    Task<double> GetAverageRatingAsync(Guid productId);

    /// <summary>
    /// Retourne les avis les plus récents pour un produit.
    /// </summary>
    /// <param name="productId">Identifiant du produit.</param>
    /// <param name="take">Nombre d'éléments à retourner (défaut 10).</param>
    /// <returns>La liste des avis récents.</returns>
    Task<IEnumerable<Review>> GetRecentByProductAsync(Guid productId, int take = 10);
}