using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

/// <summary>
/// Contrat du repository pour l'entité Brand.
/// </summary>
public interface IBrandRepository : IRepository<Brand>
{
    Task<IEnumerable<Brand>> GetActiveAsync();
    Task<Brand?> FindByNameAsync(string exactName);
    Task<Brand?> GetWithProductsAsync(Guid brandId);
}
