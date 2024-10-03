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

    public int DurationMonths { get; set; }

    [Required]
    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
