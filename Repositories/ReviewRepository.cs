using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité Review.
/// </summary>
public class ReviewRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<Review>> logger)
    : Repository<Review>(dbContext, logger), IReviewRepository
{
    public async Task<IEnumerable<Review>> GetByProductAsync(Guid productId)
    {
        return await DbSet.Where(r => r.ProductId == productId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetByUserAsync(Guid userId)
    {
        return await DbSet.Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<double> GetAverageRatingAsync(Guid productId)
    {
        return await DbSet.Where(r => r.ProductId == productId)
            .Select(r => (double)r.Rating)
            .DefaultIfEmpty(0.0)
            .AverageAsync();
    }

    public async Task<IEnumerable<Review>> GetRecentByProductAsync(Guid productId, int take = 10)
    {
        if (take <= 0) take = 10;
        return await DbSet.Where(r => r.ProductId == productId)
            .OrderByDescending(r => r.CreatedAt)
            .Take(take)
            .ToListAsync();
    }
}
