using System.ComponentModel.DataAnnotations;

namespace UGH.Domain.Entities;

public class Country
{
    [Key]
    [Required]
    public int Country_ID{get;set;}
    public string CountryName {get;set;}
    public int? Region_ID{get;set;}

}