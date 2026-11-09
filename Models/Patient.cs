using Microsoft.Exchange.WebServices.Data;
using Microsoft.Graph.Models;

namespace HQLM_Backend.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        public int UserId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? BloodGroup { get; set; }

        public Users Users { get; set; } = null!;

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}
