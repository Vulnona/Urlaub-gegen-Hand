using System.ComponentModel.DataAnnotations;

namespace UGHApi.Models
{
    public class UserRole
    {

        [Key]
        public int RoleId { get; set; }
        public required string RoleName { get; set; }
    }

}
