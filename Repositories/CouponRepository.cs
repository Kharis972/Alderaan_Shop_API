using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité Coupon.
/// </summary>
public class CouponRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<Coupon>> logger)
    : Repository<Coupon>(dbContext, logger), ICouponRepository
{
    public async Task<Coupon?> FindByCodeAsync(string code)
    {
        return await DbSet.FirstOrDefaultAsync(c => c.Code == code);
    }

    public async Task<IEnumerable<Coupon>> GetActiveAsync()
    {
        return await DbSet.Where(c => c.IsActive)
            .OrderByDescending(c => c.ValidUntil)
            .ToListAsync();
    }

    public async Task<IEnumerable<Coupon>> GetValidNowAsync(DateTime nowUtc)
    {
        return await DbSet.Where(c => c.IsActive && c.ValidFrom <= nowUtc && c.ValidUntil >= nowUtc)
            .OrderBy(c => c.ValidUntil)
            .ToListAsync();
    }
}
