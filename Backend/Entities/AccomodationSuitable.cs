using System.ComponentModel.DataAnnotations;

namespace UGH.Domain.Entities;

public class SuitableAccommodation
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}