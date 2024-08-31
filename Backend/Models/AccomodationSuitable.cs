using Microsoft.Build.Framework;

namespace UGHApi.Models
{
    public class SuitableAccommodation
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}