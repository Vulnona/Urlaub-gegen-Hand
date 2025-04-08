using System.ComponentModel.DataAnnotations;


namespace UGH.Domain.Entities;

public class Region
{   [Key]
    public int Region_ID {get;set;}

    public string RegionName {get;set;}
  
}