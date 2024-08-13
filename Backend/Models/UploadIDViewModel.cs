using System.ComponentModel.DataAnnotations;

namespace UGHApi.Models
{
    public class UploadIDViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Link_VS { get; set; }

        [Required]
        public string Link_RS { get; set; }
    }
}
