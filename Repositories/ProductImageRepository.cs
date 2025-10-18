using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité ProductImage.
/// </summary>
public class ProductImageRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<ProductImage>> logger)
    : Repository<ProductImage>(dbContext, logger), IProductImageRepository
{
    public async Task<IEnumerable<ProductImage>> GetByProductIdAsync(Guid productId)
    {
        return await DbSet.Where(pi => pi.ProductId == productId)
            .OrderBy(pi => pi.DisplayOrder)
            .ToListAsync();
    }

    public async Task<ProductImage?> GetPrimaryAsync(Guid productId)
    {
        return await DbSet.FirstOrDefaultAsync(pi => pi.ProductId == productId && pi.IsPrimary);
    }
}
