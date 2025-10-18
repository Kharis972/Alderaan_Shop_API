using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

/// <summary>
/// Contrat du repository pour l'entité ProductImage.
/// </summary>
public interface IProductImageRepository : IRepository<ProductImage>
{
    Task<IEnumerable<ProductImage>> GetByProductIdAsync(Guid productId);
    Task<ProductImage?> GetPrimaryAsync(Guid productId);
}
