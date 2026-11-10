namespace HQLM_Backend.Models
{
    public class UserRole
    {
        public int UserRoleId { get; set; }

        public int UserId { get; set; }
        public int RoleId { get; set; }

        public Users Users { get; set; } = null!;
        public Roles Roles { get; set; } = null!;
    }
}