using System.ComponentModel.DataAnnotations;

namespace UGHApi.Models
{
    public class UploadIdRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Link_VS { get; set; }

        [Required]
        public string Link_RS { get; set; }
    }
}
