using System.ComponentModel.DataAnnotations;

namespace UGH.Domain.Entities;

public class City
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public int StateId { get; set; }
}
