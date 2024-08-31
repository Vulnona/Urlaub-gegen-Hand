using System.ComponentModel.DataAnnotations;

namespace UGHApi.Models;

public class ConfirmationReq
{
    [Required]
    public string toEmail { get; set; }
    public string userName { get; set; }
    public string status { get; set; }
}
