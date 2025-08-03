using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static UGH.Domain.Core.UGH_Enums;

namespace UGH.Domain.Entities;

public class Coupon
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Coupon code is required.")]
    public string Code { get; set; }

    [Required(ErrorMessage = "Coupon name is required.")]
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public int Duration { get; set; }
    public Guid CreatedBy { get; set; }

    // Email tracking fields
    public bool IsEmailSent { get; set; } = false;
    public DateTime? EmailSentDate { get; set; }
    public string EmailSentTo { get; set; }

    public int MembershipId { get; set; }
    public Membership Membership { get; set; }

    public Redemption Redemption { get; set; }

    [ForeignKey("CreatedBy")]
    public User CreatedByUser { get; set; }
}
