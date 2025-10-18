using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using alderaan_shop.Services.Interfaces;

namespace alderaan_shop.Services;

/// <summary>
/// Service d'adresses: délègue aux repositories et ajoute validations basiques.
/// </summary>
public class AddressService : Service<Address>, IAddressService
{
    private readonly IAddressRepository _addressRepository;

    /// <summary>
    /// Initialise le service d'adresses.
    /// </summary>
    /// <param name="addressRepository">Repository d'accès aux adresses.</param>
    /// <param name="logger">Logger typé.</param>
    public AddressService(IAddressRepository addressRepository, ILogger<AddressService> logger)
        : base(addressRepository, logger)
    {
        _addressRepository = addressRepository;
    }

    /// <summary>
    /// Retourne toutes les adresses d'un utilisateur.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <returns>Les adresses de l'utilisateur, triées par date décroissante.</returns>
    /// <exception cref="ArgumentException">Si userId est vide.</exception>
    public async Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException("Invalid userId", nameof(userId));
        return await _addressRepository.GetByUserIdAsync(userId);
    }

    /// <summary>
    /// Retourne l'adresse par défaut d'un utilisateur.
    /// </summary>
    /// <param name="userId">Identifiant de l'utilisateur.</param>
    /// <returns>L'adresse par défaut si définie, sinon null.</returns>
    /// <exception cref="ArgumentException">Si userId est vide.</exception>
    public async Task<Address?> GetDefaultForUserAsync(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException("Invalid userId", nameof(userId));
        return await _addressRepository.GetDefaultForUserAsync(userId);
    }
}