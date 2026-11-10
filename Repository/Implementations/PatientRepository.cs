using HQLM_Backend.Data;
using HQLM_Backend.Models;
using HQLM_Backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HQLM_Backend.Repository.Implementations
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HQLMDBContext _context;

        public PatientRepository(HQLMDBContext context)
        {
            _context = context;
        }

        public Task<List<Patient>> GetAllAsync() =>
            _context.Patients.Include(p => p.Users).ToListAsync();

        public Task<Patient?> GetByIdAsync(int id) =>
            _context.Patients
                .Include(p => p.Users)
                .FirstOrDefaultAsync(p => p.PatientId == id);

        public async Task AddAsync(Patient patient) =>
            await _context.Patients.AddAsync(patient);

        public Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Patient patient)
        {
            _context.Patients.Remove(patient);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync() =>
            _context.SaveChangesAsync();
    }
}
