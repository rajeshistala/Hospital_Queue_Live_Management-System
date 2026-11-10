using HQLM_Backend.Data;
using HQLM_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace HQLM_Backend.Repository.Implementations
{
    public class UserRepository
    {
        private readonly HQLMDBContext _context;

        public UserRepository(HQLMDBContext context)
        {
            _context = context;
        }

        public async Task<Users?> GetByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<Users?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<Users>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task AddAsync(Users user)
        {
            await _context.Users.AddAsync(user);
        }

        public Task UpdateAsync(Users user)
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
