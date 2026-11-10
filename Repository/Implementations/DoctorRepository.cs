using HQLM_Backend.Data;
using HQLM_Backend.Models;
using HQLM_Backend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HQLM_Backend.Repository.Implementations
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HQLMDBContext _context;

        public DoctorRepository(HQLMDBContext context)
        {
            _context = context;
        }

        public Task<List<Doctor>> GetAllAsync() =>
            _context.Doctors.Include(d => d.Users).ToListAsync();

        public Task<Doctor?> GetByIdAsync(int id) =>
            _context.Doctors
                .Include(d => d.Users)
                .FirstOrDefaultAsync(d => d.DoctorId == id);

        public async Task AddAsync(Doctor doctor) =>
            await _context.Doctors.AddAsync(doctor);

        public Task UpdateAsync(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Doctor doctor)
        {
            _context.Doctors.Remove(doctor);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync() =>
            _context.SaveChangesAsync();
    }
}
