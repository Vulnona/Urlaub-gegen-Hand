using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UGHApi.Entities;

public class ShopItemConfiguration : IEntityTypeConfiguration<ShopItem>
{
    public void Configure(EntityTypeBuilder<ShopItem> builder)
    {
        builder.HasKey(si => si.Id);

        builder.Property(si => si.Name).IsRequired().HasMaxLength(100);

        builder.OwnsOne(
            si => si.Price,
            price =>
            {
                price
                    .Property(p => p.Amount)
                    .HasColumnName("Price_Amount")
                    .HasPrecision(18, 2)
                    .IsRequired();

                price
                    .Property(p => p.Currency)
                    .HasColumnName("Price_Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            }
        );
    }
}
