using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

/// <summary>
/// Contrat du repository pour l'entité CartItem.
/// </summary>
public interface ICartItemRepository : IRepository<CartItem>
{
    Task<IEnumerable<CartItem>> GetByCartIdAsync(Guid cartId);
    Task<IEnumerable<CartItem>> GetByProductIdAsync(Guid productId);
}
