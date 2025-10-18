using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité SearchHistory.
/// </summary>
public class SearchHistoryRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<SearchHistory>> logger)
    : Repository<SearchHistory>(dbContext, logger), ISearchHistoryRepository
{
    public async Task<IEnumerable<SearchHistory>> GetByUserAsync(Guid userId, int take = 50)
    {
        if (take <= 0) take = 50;
        return await DbSet.Where(s => s.UserId == userId)
            .OrderByDescending(s => s.SearchedAt)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IEnumerable<(string Term, int Count)>> GetTopTermsAsync(int top = 10)
    {
        if (top <= 0) top = 10;
        return await DbSet
            .GroupBy(s => s.SearchQuery)
            .OrderByDescending(g => g.Count())
            .Take(top)
            .Select(g => new ValueTuple<string, int>(g.Key, g.Count()))
            .ToListAsync();
    }

    public async Task AddTermAsync(Guid userId, string term)
    {
        SearchHistory entry = new SearchHistory(Guid.NewGuid(), userId, term.ToCharArray().AsMemory(), 0);
        await DbSet.AddAsync(entry);
        await DbContext.SaveChangesAsync();
    }
}
