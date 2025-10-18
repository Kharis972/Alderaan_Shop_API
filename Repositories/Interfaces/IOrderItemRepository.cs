using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

/// <summary>
/// Contrat du repository pour l'entité OrderItem.
/// </summary>
public interface IOrderItemRepository : IRepository<OrderItem>
{
    /// <summary>
    /// Retourne tous les items d'une commande donnée.
    /// </summary>
    Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId);

    /// <summary>
    /// Retourne tous les items associés à un produit donné.
    /// </summary>
    Task<IEnumerable<OrderItem>> GetByProductIdAsync(Guid productId);

    /// <summary>
    /// Retourne les identifiants des produits les plus commandés (par somme des quantités).
    /// </summary>
    Task<IEnumerable<Guid>> GetMostOrderedProductIdsAsync(int top);
}
