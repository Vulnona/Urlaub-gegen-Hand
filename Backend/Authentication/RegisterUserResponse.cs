namespace UGH.Contracts.Authentication;

public class RegisterUserResponse
{
    public string Email { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
}
