using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product> CheckForProductByIdAsync(Guid id);
    Task<Product> UpdateProductAsync(Product product);
}