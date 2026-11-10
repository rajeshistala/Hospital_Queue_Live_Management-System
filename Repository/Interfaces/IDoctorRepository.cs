using HQLM_Backend.Models;

namespace HQLM_Backend.Repository.Interfaces
{
    public interface IDoctorRepository
    {
        Task<List<Doctor>> GetAllAsync();
        Task<Doctor?> GetByIdAsync(int id);
        Task AddAsync(Doctor doctor);
        Task UpdateAsync(Doctor doctor);
        Task DeleteAsync(Doctor doctor);
        Task SaveChangesAsync();
    }
}
