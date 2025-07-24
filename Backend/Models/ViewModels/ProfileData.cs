using System.ComponentModel.DataAnnotations;

namespace UGH.Domain.ViewModels;
// used for update-profile
public class ProfileData
{
#pragma warning disable CS8632
    public string? Hobbies { get; set; }
    public string? Skills { get; set; }
}
