using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité Discount.
/// </summary>
public class DiscountRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<Discount>> logger)
    : Repository<Discount>(dbContext, logger), IDiscountRepository
{
    public async Task<IEnumerable<Discount>> GetActiveAsync()
    {
        return await DbSet.Where(d => d.IsActive)
            .OrderByDescending(d => d.ValidUntil)
            .ToListAsync();
    }

    public async Task<IEnumerable<Discount>> GetValidNowAsync(DateTime nowUtc)
    {
        return await DbSet.Where(d => d.IsActive && d.ValidFrom <= nowUtc && d.ValidUntil >= nowUtc)
            .OrderBy(d => d.ValidUntil)
            .ToListAsync();
    }

    public async Task<IEnumerable<Discount>> GetForProductAsync(Guid productId)
    {
        return await DbSet.Where(d => d.ProductId == productId && d.IsActive)
            .OrderByDescending(d => d.ValidUntil)
            .ToListAsync();
    }

    public async Task<IEnumerable<Discount>> GetForCategoryAsync(Guid categoryId)
    {
        return await DbSet.Where(d => d.CategoryId == categoryId && d.IsActive)
            .OrderByDescending(d => d.ValidUntil)
            .ToListAsync();
    }

    public async Task<IEnumerable<Discount>> GetGlobalAsync()
    {
        return await DbSet.Where(d => d.ProductId == null && d.CategoryId == null && d.IsActive)
            .OrderByDescending(d => d.ValidUntil)
            .ToListAsync();
    }
}
