using HQLM_Backend.Data;
using HQLM_Backend.Models;
using HQLM_Backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HQLM_Backend.Repository.Implementations
{
    public class NotificationRepository: INotificationRepository
    {
        private readonly HQLMDBContext _context;

        public NotificationRepository(HQLMDBContext context)
        {
            _context = context;
        }

        public Task<List<Notification>> GetByUserIdAsync(int userId) =>
            _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

        public Task<Notification?> GetByIdAsync(int id) =>
            _context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == id);

        public async Task AddAsync(Notification notification) =>
            await _context.Notifications.AddAsync(notification);

        public Task UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync() =>
            _context.SaveChangesAsync();
    }
}
