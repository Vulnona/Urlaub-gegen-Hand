using System;
using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    // ggf. weitere erforderliche attribute für die Registrierung
}
