using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task SaveOrderAggregateAsync(Order order, IEnumerable<OrderItem> items, Payment payment, IEnumerable<(Guid ProductId, int Quantity)> productAdjustments);
    Task<Order?> GetOrderWithDetailsAsync(Guid id);
}