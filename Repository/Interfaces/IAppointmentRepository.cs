using HQLM_Backend.Models;

namespace HQLM_Backend.Repository.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAllAsync();
        Task<Appointment?> GetByIdAsync(int id);
        Task<List<Appointment>> GetByPatientIdAsync(int patientId);
        Task<List<Appointment>> GetByDoctorIdAsync(int doctorId);
        Task AddAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
        Task SaveChangesAsync();
    }
}
