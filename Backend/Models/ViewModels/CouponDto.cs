namespace UGHApi.ViewModels;

public class CouponDto
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public int Duration { get; set; }
    public string CreatedBy { get; set; }
    public bool IsRedeemed { get; set; }
    public string RedeemedBy { get; set; } = string.Empty;
}
