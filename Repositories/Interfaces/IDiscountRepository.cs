using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

/// <summary>
/// Contrat du repository pour l'entité Discount.
/// </summary>
public interface IDiscountRepository : IRepository<Discount>
{
    Task<IEnumerable<Discount>> GetActiveAsync();
    Task<IEnumerable<Discount>> GetValidNowAsync(DateTime nowUtc);
    Task<IEnumerable<Discount>> GetForProductAsync(Guid productId);
    Task<IEnumerable<Discount>> GetForCategoryAsync(Guid categoryId);
    Task<IEnumerable<Discount>> GetGlobalAsync();
}
