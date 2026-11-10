using HQLM_Backend.Models;

namespace HQLM_Backend.Repository.Interfaces
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetAllAsync();
        Task<Patient?> GetByIdAsync(int id);
        Task AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(Patient patient);
        Task SaveChangesAsync();
    }
}
