using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

/// <summary>
/// Repository concret pour l'entité Notification.
/// </summary>
public class NotificationRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<Notification>> logger)
    : Repository<Notification>(dbContext, logger), INotificationRepository
{
    public async Task<IEnumerable<Notification>> GetByUserAsync(Guid userId, int take = 50)
    {
        if (take <= 0) take = 50;
        return await DbSet.Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetUnreadByUserAsync(Guid userId)
    {
        return await DbSet.Where(n => n.UserId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> MarkAsReadAsync(Guid notificationId)
    {
        Notification? notif = await DbSet.FirstOrDefaultAsync(n => n.Id == notificationId);
        if (notif == null) return false;
        if (!notif.IsRead)
        {
            notif.IsRead = true;
            notif.ReadAt = DateTime.UtcNow;
            DbSet.Update(notif);
            await DbContext.SaveChangesAsync();
        }
        return true;
    }
}
