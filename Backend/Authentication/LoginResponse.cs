#nullable enable

namespace UGH.Contracts.Authentication;

public class LoginResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string Email { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public bool RequiresTwoFactor { get; set; } = false;
    public string Message { get; set; }
    public string? TwoFactorToken { get; set; }
}
