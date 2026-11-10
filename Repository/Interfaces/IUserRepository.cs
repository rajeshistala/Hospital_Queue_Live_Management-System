using HQLM_Backend.Models;

namespace HQLM_Backend.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<Users?> GetByIdAsync(int userId);

        Task<Users?> GetByEmailAsync(string email);

        Task<List<Users>> GetAllAsync();

        Task AddAsync(Users users);

        Task UpdateAsync(Users users);

        Task SaveChangesAsync();
    }
}
