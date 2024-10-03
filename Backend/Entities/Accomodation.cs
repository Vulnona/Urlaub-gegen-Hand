using System.ComponentModel.DataAnnotations;

namespace UGH.Domain.Entities;

public class Accommodation
{
    public int Id { get; set; }
    [Required]
    public string NameAccommodationType { get; set; }
}