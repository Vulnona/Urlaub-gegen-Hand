using System.ComponentModel.DataAnnotations;

namespace UGHApi.Models
{
    public class Continent
    {
        [Key]
        public int Continent_ID{get;set;}
        public string? ContinentName{get;}
    }
}