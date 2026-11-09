namespace HQLM_Backend.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public TimeSpan AppointmentTime { get; set; }

        public string Status { get; set; } = "Booked";

        public string? Reason { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Patient Patient { get; set; } = null!;

        public Doctor Doctor { get; set; } = null!;

        public QueueToken? QueueToken { get; set; }
    }
}
