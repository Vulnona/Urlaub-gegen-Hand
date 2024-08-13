using System.ComponentModel.DataAnnotations;

namespace UGHApi.Models
{
    public class EmailSend
    {
        [Required]
        public string ToEmail { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
