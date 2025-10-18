using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité OrderItem.
/// Fournit des requêtes ciblées fréquemment utilisées.
/// </summary>
public class OrderItemRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<OrderItem>> logger)
    : Repository<OrderItem>(dbContext, logger), IOrderItemRepository
{
    /// <summary>
    /// Retourne tous les items d'une commande.
    /// </summary>
    public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId)
    {
        return await DbSet.Where(oi => oi.OrderId == orderId).ToListAsync();
    }

    /// <summary>
    /// Retourne tous les items associés à un produit.
    /// </summary>
    public async Task<IEnumerable<OrderItem>> GetByProductIdAsync(Guid productId)
    {
        return await DbSet.Where(oi => oi.ProductId == productId).ToListAsync();
    }

    /// <summary>
    /// Retourne les IDs des produits les plus commandés.
    /// </summary>
    public async Task<IEnumerable<Guid>> GetMostOrderedProductIdsAsync(int top)
    {
        if (top <= 0) top = 5;
        return await DbSet
            .GroupBy(oi => oi.ProductId)
            .OrderByDescending(g => g.Sum(x => x.Quantity))
            .Take(top)
            .Select(g => g.Key)
            .ToListAsync();
    }
}
