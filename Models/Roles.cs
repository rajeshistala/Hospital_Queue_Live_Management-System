using System.ComponentModel.DataAnnotations;

namespace HQLM_Backend.Models
{
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }

        public string RoleName { get; set; } = string.Empty;

        public ICollection<UserRole> UserRoles { get; set; }
            = new List<UserRole>();
    }
}
