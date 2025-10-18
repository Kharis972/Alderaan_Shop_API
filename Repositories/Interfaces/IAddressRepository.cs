using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

/// <summary>
/// Contrat du repository pour l'entité Address.
/// Hérite du contrat générique pour fournir les opérations CRUD de base.
/// </summary>
public interface IAddressRepository : IRepository<Address>
{
    /// <summary>
    /// Retourne toutes les adresses d'un utilisateur.
    /// </summary>
    Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Retourne l'adresse par défaut d'un utilisateur, si définie.
    /// </summary>
    Task<Address?> GetDefaultForUserAsync(Guid userId);
}
