using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité Newsletter.
/// </summary>
public class NewsletterRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<Newsletter>> logger)
    : Repository<Newsletter>(dbContext, logger), INewsletterRepository
{
    public async Task<Newsletter?> FindByEmailAsync(string email)
    {
        return await DbSet.FirstOrDefaultAsync(n => n.Email == email);
    }

    public async Task<IEnumerable<Newsletter>> GetSubscribedAsync()
    {
        return await DbSet.Where(n => n.IsSubscribed)
            .OrderByDescending(n => n.SubscribedAt)
            .ToListAsync();
    }
}
