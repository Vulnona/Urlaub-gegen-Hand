using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UGH.Domain.Entities;

public class Ratings
{
    public int Id { get; set; }
    [Range(0, 5, ErrorMessage = "Please select  number between 1 to 5")]
    public int Rating { get; set; }
    [DataType(DataType.Date)]
    public DateTime SubmissionDate { get; set; }
    public Guid User_Id { get; set; }
    [ForeignKey("User_Id")]
    public User User { get; set; }
    public int OfferId { get; set; }
    [ForeignKey("OfferId")]
    public Offer Offer { get; set; }

}
