using System.ComponentModel.DataAnnotations;

namespace UGHApi.Models
{
    public class Continent
    {
        [Key]
        public int ContinentID{get;}
        public string? ContinentName{get;}
    }
}