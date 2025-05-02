namespace UGHApi.ViewModels;

public class MembershipDto
{
    public int MembershipID { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int DurationMonths { get; set; }

    public decimal Price { get; set; }
}
