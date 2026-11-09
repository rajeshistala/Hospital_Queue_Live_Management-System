using Microsoft.Graph.Models;

namespace HQLM_Backend.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Users Users { get; set; } = null!;
    }
}
