using UGHApi.Entities;
using static UGH.Domain.Core.UGH_Enums;

namespace UGHApi.ViewModels;

public class TransactionDto
{
    public int Id { get; set; }
    public DateTime TransactionDate { get; set; }
    public Money Amount { get; set; }
    public string ShopItemName { get; set; }
    public TransactionStatus Status { get; set; }
    public string CouponStatus { get; set; }
    public string CouponCode { get; set; }
    public string PaymentMethod { get; set; }
}
