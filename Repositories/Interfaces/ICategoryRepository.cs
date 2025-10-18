using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

/// <summary>
/// Contrat du repository pour l'entité Category.
/// </summary>
public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetActiveAsync();
    Task<Category?> FindByNameAsync(string exactName);
    Task<IEnumerable<Category>> GetChildrenAsync(Guid? parentCategoryId);
}
