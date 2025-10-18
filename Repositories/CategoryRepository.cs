using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité Category.
/// </summary>
public class CategoryRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<Category>> logger)
    : Repository<Category>(dbContext, logger), ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetActiveAsync()
    {
        return await DbSet.Where(c => c.IsActive).OrderByDescending(c => c.CreatedAt).ToListAsync();
    }

    public async Task<Category?> FindByNameAsync(string exactName)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.Name == exactName);
    }

    public async Task<IEnumerable<Category>> GetChildrenAsync(Guid? parentCategoryId)
    {
        return await DbSet.Where(c => c.ParentCategoryId == parentCategoryId).ToListAsync();
    }
}
