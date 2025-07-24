using System.ComponentModel.DataAnnotations;

namespace UGHApi.Models
{
    public class CustomMailBody
    {
        [Required]
        public string To { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
    }
}
