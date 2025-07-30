namespace UGHApi.ViewModels.UserComponent;

public class UserC
{
    #pragma warning disable CS8632
    public Guid User_Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public byte[]? ProfilePicture { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string? DeletedUserName { get; set; }
}
