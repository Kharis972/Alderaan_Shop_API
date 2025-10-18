using alderaan_shop.Models;

namespace alderaan_shop.Services.Interfaces;

/// <summary>
/// Service applicatif pour les adresses utilisateur.
/// </summary>
public interface IAddressService : IService<Address>
{
    /// <summary>
    /// Retourne toutes les adresses d'un utilisateur.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <returns>La séquence d'adresses, récentes d'abord.</returns>
    Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Retourne l'adresse par défaut d'un utilisateur, si définie.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <returns>L'adresse par défaut ou null.</returns>
    Task<Address?> GetDefaultForUserAsync(Guid userId);
}