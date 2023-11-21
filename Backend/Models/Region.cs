using System.ComponentModel.DataAnnotations;
namespace Backend.Models
{
    public class Region
    {   [Key]
        public int Region_ID {get;}

        public string? RegionName {get;set;}
        public int? ContinentID {get;set;}
        public int? CountryID{get;set;}
    }
}