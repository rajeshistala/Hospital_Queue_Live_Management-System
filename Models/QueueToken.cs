namespace HQLM_Backend.Models
{
    public class QueueToken
    {
        public int QueueTokenId { get; set; }

        public int AppointmentId { get; set; }

        public int TokenNumber { get; set; }

        public DateTime QueueDate { get; set; }

        public string Status { get; set; } = "Waiting";

        public DateTime? CalledAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public Appointment Appointment { get; set; } = null!;
    }
}
