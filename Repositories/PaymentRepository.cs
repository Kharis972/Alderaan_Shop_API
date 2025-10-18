using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité Payment.
/// </summary>
public class PaymentRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<Payment>> logger)
    : Repository<Payment>(dbContext, logger), IPaymentRepository
{
    public async Task<Payment?> GetByOrderIdAsync(Guid orderId)
    {
        return await DbSet.FirstOrDefaultAsync(p => p.OrderId == orderId);
    }

    public async Task<IEnumerable<Payment>> GetByStatusAsync(string status)
    {
        return await DbSet.Where(p => p.Status == status).OrderByDescending(p => p.CreatedAt).ToListAsync();
    }
}
