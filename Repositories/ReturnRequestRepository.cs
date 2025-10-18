using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité ReturnRequest.
/// </summary>
public class ReturnRequestRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<ReturnRequest>> logger)
    : Repository<ReturnRequest>(dbContext, logger), IReturnRequestRepository
{
    public async Task<IEnumerable<ReturnRequest>> GetByUserAsync(Guid userId)
    {
        return await DbSet.Where(r => r.UserId == userId)
            .OrderByDescending(r => r.RequestedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ReturnRequest>> GetByOrderAsync(Guid orderId)
    {
        return await DbSet.Where(r => r.OrderId == orderId)
            .OrderByDescending(r => r.RequestedAt)
            .ToListAsync();
    }

    public async Task<bool> UpdateStatusAsync(Guid returnRequestId, string status, string? adminNotes = null)
    {
        ReturnRequest? rr = await DbSet.FirstOrDefaultAsync(r => r.Id == returnRequestId);
        if (rr == null) return false;
        rr.Status = status;
        rr.AdminNotes = adminNotes ?? rr.AdminNotes;
        rr.ProcessedAt = DateTime.UtcNow;
        DbSet.Update(rr);
        await DbContext.SaveChangesAsync();
        return true;
    }
}
