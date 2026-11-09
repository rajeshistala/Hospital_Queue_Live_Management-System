using Microsoft.Graph.Models;
using StackExchange.Redis;
using System.Data;

namespace HQLM_Backend.Models
{
    public class UserRole
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public Users Users { get; set; } = null!;

        public Role Roles { get; set; } = null!;
    }
}
