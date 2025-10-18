using alderaan_shop.Models;

namespace alderaan_shop.Services.Interfaces;

/// <summary>
/// Service applicatif pour les réductions.
/// </summary>
public interface IDiscountService : IService<Discount>
{
    /// <summary>
    /// Retourne les réductions actives (toutes portées confondues).
    /// </summary>
    /// <returns>Une séquence de réductions actives.</returns>
    Task<IEnumerable<Discount>> GetActiveAsync();

    /// <summary>
    /// Retourne les réductions valides à l'instant fourni.
    /// </summary>
    /// <param name="nowUtc">Horodatage UTC servant de référence.</param>
    /// <returns>Une séquence de réductions valides.</returns>
    Task<IEnumerable<Discount>> GetValidNowAsync(DateTime nowUtc);

    /// <summary>
    /// Retourne les réductions actives applicables à un produit.
    /// </summary>
    /// <param name="productId">Identifiant du produit.</param>
    /// <returns>La séquence de réductions pour ce produit.</returns>
    Task<IEnumerable<Discount>> GetForProductAsync(Guid productId);

    /// <summary>
    /// Retourne les réductions actives applicables à une catégorie.
    /// </summary>
    /// <param name="categoryId">Identifiant de la catégorie.</param>
    /// <returns>La séquence de réductions pour cette catégorie.</returns>
    Task<IEnumerable<Discount>> GetForCategoryAsync(Guid categoryId);

    /// <summary>
    /// Retourne les réductions globales (ni produit, ni catégorie).
    /// </summary>
    /// <returns>La séquence de réductions globales.</returns>
    Task<IEnumerable<Discount>> GetGlobalAsync();
}