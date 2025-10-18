using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité Address.
/// Hérite du repository générique pour les opérations CRUD de base.
/// </summary>
public class AddressRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<Address>> logger)
    : Repository<Address>(dbContext, logger), IAddressRepository
{
    /// <summary>
    /// Retourne toutes les adresses d'un utilisateur.
    /// </summary>
    public async Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId)
    {
        return await DbSet.Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Retourne l'adresse par défaut d'un utilisateur, si définie.
    /// </summary>
    public async Task<Address?> GetDefaultForUserAsync(Guid userId)
    {
        return await DbSet.FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault);
    }
}
