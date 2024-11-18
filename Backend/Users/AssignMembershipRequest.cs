namespace UGHApi.Users;

public class AssignMembershipRequest
{
    public Guid UserId { get; set; }
    public int MembershipId { get; set; }
}
