using UGH.Domain.Entities;
using static UGH.Domain.Core.UGH_Enums;

namespace UGHApi.Entities;

public class Transaction
{
    public int Id { get; set; }

    public DateTime TransactionDate { get; set; }

    public Money Amount { get; set; }

    public int ShopItemId { get; set; }
    public ShopItem ShopItem { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public string TransactionId { get; set; }

    public string PaymentMethod { get; set; } = null;

    public TransactionStatus Status { get; set; }

    public int? CouponId { get; set; }
    public Coupon Coupon { get; set; }

    public void UpdateTransactionStatus(TransactionStatus status)
    {
        Status = status;
    }

    public void AssignCouponId(int couponId)
    {
        CouponId = couponId;
    }

    public void SetPaymentMethod(string paymentMenthod)
    {
        PaymentMethod = paymentMenthod;
    }
}
