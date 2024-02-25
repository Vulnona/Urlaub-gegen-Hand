using UGHModels;

namespace UGHApi.Models
{
    public class UserRoleMapping
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        // Navigation properties
        public User User { get; set; }
        public UserRole Role { get; set; }
    }
}
