using System.ComponentModel.DataAnnotations;
namespace UGHApi.Models
{
    public class Region
    {   [Key]
        public int Region_ID {get;set;}

        public string RegionName {get;set;}
      
    }
}