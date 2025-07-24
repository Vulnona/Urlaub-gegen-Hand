using static UGH.Domain.Core.UGH_Enums;

namespace UGHApi.Entities;

public class ShopItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CouponDuration Duration { get; set; }
    public Money Price { get; set; }
    public string Description { get; set; }
}
