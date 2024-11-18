using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UGH.Domain.Entities;

public class UserMembership
{
    [Key]
    public int UserMembershipID { get; set; }

    [Required]
    public Guid User_Id { get; set; }

    [Required]
    public int MembershipID { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime Expiration { get; set; }

    [MaxLength(50)]
    public string Status
    {
        get
        {
            if (Expiration <= DateTime.Now)
            {
                return "Expired";
            }
            return "Active";
        }
    }

    public bool IsMembershipActive
    {
        get { return StartDate <= DateTime.Now && Expiration > DateTime.Now; }
    }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [ForeignKey("User_Id")]
    public virtual User User { get; set; }

    [ForeignKey("MembershipID")]
    public virtual Membership Membership { get; set; }
}
