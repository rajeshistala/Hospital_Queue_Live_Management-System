using HQLM_Backend.Models;

namespace HQLM_Backend.Repository.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetByUserIdAsync(int userId);
        Task<Notification?> GetByIdAsync(int id);
        Task AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task SaveChangesAsync();

    }
}
