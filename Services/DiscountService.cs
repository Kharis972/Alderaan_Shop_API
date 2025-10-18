using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using alderaan_shop.Services.Interfaces;

namespace alderaan_shop.Services;

/// <summary>
/// Service des réductions: expose des méthodes de lecture ciblées.
/// </summary>
public class DiscountService : Service<Discount>, IDiscountService
{
    private readonly IDiscountRepository _discountRepository;

    /// <summary>
    /// Initialise le service des réductions.
    /// </summary>
    /// <param name="discountRepository">Repository des réductions.</param>
    /// <param name="logger">Logger typé.</param>
    public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger)
        : base(discountRepository, logger)
    {
        _discountRepository = discountRepository;
    }

    /// <summary>
    /// Retourne les réductions actives (toutes portées confondues).
    /// </summary>
    public Task<IEnumerable<Discount>> GetActiveAsync() => _discountRepository.GetActiveAsync();

    /// <summary>
    /// Retourne les réductions valides à l'instant fourni.
    /// </summary>
    /// <param name="nowUtc">Horodatage UTC de référence.</param>
    public Task<IEnumerable<Discount>> GetValidNowAsync(DateTime nowUtc) => _discountRepository.GetValidNowAsync(nowUtc);

    /// <summary>
    /// Retourne les réductions actives d'un produit.
    /// </summary>
    /// <param name="productId">Identifiant du produit.</param>
    /// <exception cref="ArgumentException">Si productId est vide.</exception>
    public Task<IEnumerable<Discount>> GetForProductAsync(Guid productId)
    {
        if (productId == Guid.Empty) throw new ArgumentException("Invalid productId", nameof(productId));
        return _discountRepository.GetForProductAsync(productId);
    }

    /// <summary>
    /// Retourne les réductions actives d'une catégorie.
    /// </summary>
    /// <param name="categoryId">Identifiant de la catégorie.</param>
    /// <exception cref="ArgumentException">Si categoryId est vide.</exception>
    public Task<IEnumerable<Discount>> GetForCategoryAsync(Guid categoryId)
    {
        if (categoryId == Guid.Empty) throw new ArgumentException("Invalid categoryId", nameof(categoryId));
        return _discountRepository.GetForCategoryAsync(categoryId);
    }

    /// <summary>
    /// Retourne les réductions globales.
    /// </summary>
    public Task<IEnumerable<Discount>> GetGlobalAsync() => _discountRepository.GetGlobalAsync();
}