using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UGH.Domain.Entities;

public class Picture {    
    public int Id { get; set; }
    public String Hash { get; set; }
    // to allow the same picture in different sizes.
    public int Width { get; set; }
    [Required]
    public byte[] ImageData { get; set; }
    public Guid UserId { get; init; }
    [ForeignKey("UserId")]
    public User Owner { get; set; }
}
