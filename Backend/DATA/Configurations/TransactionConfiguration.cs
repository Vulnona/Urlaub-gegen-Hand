using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UGHApi.Entities;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TransactionDate).IsRequired();

        builder.Property(t => t.TransactionId).IsRequired().HasMaxLength(50);

        builder.OwnsOne(
            t => t.Amount,
            amount =>
            {
                amount
                    .Property(a => a.Amount)
                    .HasColumnName("Amount_Value")
                    .HasPrecision(18, 2)
                    .IsRequired();

                amount
                    .Property(a => a.Currency)
                    .HasColumnName("Amount_Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            }
        );

        builder.HasOne(t => t.ShopItem).WithMany().HasForeignKey(t => t.ShopItemId).IsRequired();

        builder.HasOne(t => t.Coupon).WithMany().HasForeignKey(t => t.CouponId).IsRequired(false);
    }
}
