namespace UGH.Domain.Entities;

public class UserRoleMapping
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int RoleId { get; set; }

    // Navigation properties
    public User User { get; set; }
    public UserRole Role { get; set; }
}
