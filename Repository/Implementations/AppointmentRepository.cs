using HQLM_Backend.Data;
using HQLM_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace HQLM_Backend.Repository.Implementations
{
    public class AppointmentRepository
    {
        private readonly HQLMDBContext _context;

        public AppointmentRepository(HQLMDBContext context)
        {
            _context = context;
        }

        public Task<List<Appointment>> GetAllAsync() =>
            _context.Appointments
                .Include(a => a.Patient).ThenInclude(p => p.Users)
                .Include(a => a.Doctor).ThenInclude(d => d.Users)
                .ToListAsync();

        public Task<Appointment?> GetByIdAsync(int id) =>
            _context.Appointments
                .Include(a => a.Patient).ThenInclude(p => p.Users)
                .Include(a => a.Doctor).ThenInclude(d => d.Users)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

        public Task<List<Appointment>> GetByPatientIdAsync(int patientId) =>
            _context.Appointments
                .Include(a => a.Doctor).ThenInclude(d => d.Users)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();

        public Task<List<Appointment>> GetByDoctorIdAsync(int doctorId) =>
            _context.Appointments
                .Include(a => a.Patient).ThenInclude(p => p.Users)
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();

        public async Task AddAsync(Appointment appointment) =>
            await _context.Appointments.AddAsync(appointment);

        public Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync() =>
            _context.SaveChangesAsync();
    }
}
