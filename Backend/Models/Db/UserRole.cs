using System.ComponentModel.DataAnnotations;

namespace UGH.Domain.Entities;

public class UserRole
{

    [Key]
    public int RoleId { get; set; }
    public required string RoleName { get; set; }
}
