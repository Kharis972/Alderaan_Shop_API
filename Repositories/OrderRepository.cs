using alderaan_shop.Data;
using alderaan_shop.Helpers.CustomExceptions;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'agrégat Order (Order, OrderItems, Payment).
/// Fournit des opérations transactionnelles et des récupérations avec détails.
/// </summary>
public class OrderRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<Order>> logger)
    : Repository<Order>(dbContext, logger), IOrderRepository
{
    /// <summary>
    /// Sauvegarde une commande complète (commande, items, paiement) et ajuste les stocks dans une transaction.
    /// </summary>
    /// <param name="order">Entité commande principale.</param>
    /// <param name="items">Items de la commande.</param>
    /// <param name="payment">Paiement associé.</param>
    /// <param name="productAdjustments">Liste des ajustements de stock à appliquer (ProductId, Quantity).</param>
    public async Task SaveOrderAggregateAsync(
        Order order,
        IEnumerable<OrderItem> items,
        Payment payment,
        IEnumerable<(Guid ProductId, int Quantity)> productAdjustments)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(payment);
        ArgumentNullException.ThrowIfNull(productAdjustments);

        await using var tx = await DbContext.Database.BeginTransactionAsync();
        try
        {
            await DbContext.Orders.AddAsync(order);
            await DbContext.OrderItems.AddRangeAsync(items);
            await DbContext.Payments.AddAsync(payment);

            // Apply stock adjustments
            foreach ((Guid productId, int quantity) in productAdjustments)
            {
                Product? product = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (product == null)
                    throw new ProductNotFoundException($"Product {productId} not found.");

                if (quantity <= 0)
                    throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be at least 1.");

                if (product.Stock < quantity)
                    throw new InvalidOperationException($"Insufficient stock for product {product.Id}.");

                product.Stock -= quantity;
                product.SoldCount += quantity;
                DbContext.Products.Update(product);
            }

            await DbContext.SaveChangesAsync();
            await tx.CommitAsync();
        }
        catch (DbUpdateException ex)
        {
            await tx.RollbackAsync();
            Logger.LogError(ex, "Database error while saving order aggregate.");
            throw new DatabaseException("Unable to save order due to a database error.");
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Récupère une commande avec son paiement et ses items associés.
    /// </summary>
    /// <param name="id">Identifiant de la commande.</param>
    /// <returns>La commande complète si trouvée, sinon null.</returns>
    public async Task<Order?> GetOrderWithDetailsAsync(Guid id)
    {
        return await DbContext.Orders
            .Include(o => o.Payment)
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}