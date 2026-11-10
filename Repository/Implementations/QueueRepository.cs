using HQLM_Backend.Data;
using HQLM_Backend.Models;
using HQLM_Backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HQLM_Backend.Repository.Implementations
{
    public class QueueRepository : IQueueRepository
    {
        private readonly HQLMDBContext _context;

        public QueueRepository(HQLMDBContext context)
        {
            _context = context;
        }

        public Task<List<QueueToken>> GetDailyQueueAsync(DateTime date) =>
            _context.QueueTokens
                .Include(q => q.Appointment)
                .Where(q => q.QueueDate.Date == date.Date)
                .OrderBy(q => q.TokenNumber)
                .ToListAsync();

        public Task<QueueToken?> GetByIdAsync(int id) =>
            _context.QueueTokens
                .Include(q => q.Appointment)
                .FirstOrDefaultAsync(q => q.QueueTokenId == id);

        public async Task AddAsync(QueueToken token) =>
            await _context.QueueTokens.AddAsync(token);

        public Task UpdateAsync(QueueToken token)
        {
            _context.QueueTokens.Update(token);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync() =>
            _context.SaveChangesAsync();
    }
}
