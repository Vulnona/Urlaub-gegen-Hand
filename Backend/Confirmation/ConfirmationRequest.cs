using System.ComponentModel.DataAnnotations;

namespace UGH.Contracts.Confirmation;

public class ConfirmationRequest
{
    [Required]
    public string toEmail { get; set; }
    public string userName { get; set; }
    public string status { get; set; }
}
