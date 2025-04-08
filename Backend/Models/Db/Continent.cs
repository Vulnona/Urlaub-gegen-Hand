using System.ComponentModel.DataAnnotations;

namespace UGH.Domain.Entities;

public class Continent
{
    [Key]
    [Required]
    public int Continent_ID{get;set;}
    public string ContinentName{get;}
}