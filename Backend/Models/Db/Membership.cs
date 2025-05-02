using System.ComponentModel.DataAnnotations;

namespace UGH.Domain.Entities;

public class Membership
{
    [Key]
    public int MembershipID { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public string Description { get; set; }

    public int DurationDays { get; set; }

    [Required]
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}
