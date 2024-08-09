using Microsoft.Build.Framework;
using Newtonsoft.Json;

namespace UGHApi.Models
{
    public class Accommodation
    {
        public int Id { get; set; }
        [Required]
        public string NameAccommodationType { get; set; }
    }
}