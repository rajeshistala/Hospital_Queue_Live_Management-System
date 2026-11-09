using Microsoft.Exchange.WebServices.Data;
using Microsoft.Graph.Models;

namespace HQLM_Backend.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        public int UserId { get; set; }

        public string Specialization { get; set; } = string.Empty;

        public string? Qualification { get; set; }

        public int ExperienceYears { get; set; }

        public decimal ConsultationFee { get; set; }

        public string? Bio { get; set; }

        public bool IsAvailable { get; set; } = true;

        public Users Users { get; set; } = null!;

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}
