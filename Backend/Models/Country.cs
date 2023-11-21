using System.ComponentModel.DataAnnotations;

namespace UGHApi.Models
{
    public class Country
    {
        [Key]
        public int Country_ID{get;}
        public string? CountryName {get;set;}
        public int? Region_ID{get;set;}

    }
}