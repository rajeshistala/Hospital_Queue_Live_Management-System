using HQLM_Backend.Models;

namespace HQLM_Backend.Repository.Interfaces
{
    public interface IQueueRepository
    {
        Task<List<QueueToken>> GetDailyQueueAsync(DateTime date);
        Task<QueueToken?> GetByIdAsync(int id);
        Task AddAsync(QueueToken token);
        Task UpdateAsync(QueueToken token);
        Task SaveChangesAsync();
    }
}
