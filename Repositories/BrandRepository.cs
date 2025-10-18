using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité Brand.
/// </summary>
public class BrandRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<Brand>> logger)
    : Repository<Brand>(dbContext, logger), IBrandRepository
{
    public async Task<IEnumerable<Brand>> GetActiveAsync()
    {
        return await DbSet.Where(b => b.IsActive).OrderByDescending(b => b.CreatedAt).ToListAsync();
    }

    public async Task<Brand?> FindByNameAsync(string exactName)
    {
        return await DbSet.FirstOrDefaultAsync(b => b.Name == exactName);
    }

    public async Task<Brand?> GetWithProductsAsync(Guid brandId)
    {
        return await DbContext.Brands
            .Include(b => b.Products)
            .FirstOrDefaultAsync(b => b.Id == brandId);
    }
}
