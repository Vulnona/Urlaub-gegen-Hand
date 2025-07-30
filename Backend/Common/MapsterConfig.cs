using Mapster;
using UGH.Domain.Entities;
using UGHApi.Entities;
using UGHApi.Extensions;
using UGHApi.ViewModels;

public static class MapsterConfig
{
#pragma warning disable CS8632
    public static void RegisterMappings()
    {
        TypeAdapterConfig<User, UserDTO>
            .NewConfig()
            .Map(dest => dest.Hobbies, src => SplitAndTrim(src.Hobbies))
            .Map(dest => dest.Skills, src => SplitAndTrim(src.Skills))
            .Map(dest => dest.Link_RS, src => src.Link_RS) // Will be overridden in repository
            .Map(dest => dest.Link_VS, src => src.Link_VS); // Will be overridden in repository

        TypeAdapterConfig<Coupon, CouponDto>
            .NewConfig()
            .Map(dest => dest.Duration, src => src.Duration.ToDays())
            .Map(dest => dest.CreatedBy, src => src.CreatedByUser.Email_Address);

        TypeAdapterConfig<Transaction, TransactionDto>
            .NewConfig()
            .Map(dest => dest.CouponCode, src => src.Coupon != null ? src.Coupon.Code : null)
            .Map(dest => dest.ShopItemName, src => src.ShopItem != null ? src.ShopItem.Name : null)
            .Map(dest => dest.TransactionDate, src => src.TransactionDate)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(
                dest => dest.CouponStatus,
                src =>
                    src.Coupon == null ? "Failed"
                    : src.Coupon.Redemption == null ? "Available"
                    : "Redeemed"
            )
            .Map(dest => dest.Status, src => src.Status);

        // Review to ReviewDto mapping
        TypeAdapterConfig<Review, ReviewDto>
            .NewConfig()
            .Map(dest => dest.Reviewer, src => src.Reviewer != null ? new UGHApi.ViewModels.UserComponent.UserC
            {
                User_Id = src.Reviewer.User_Id,
                FirstName = src.Reviewer.FirstName,
                LastName = src.Reviewer.LastName,
                ProfilePicture = src.Reviewer.ProfilePicture,
                IsDeleted = false,
                DeletedUserName = null
            } : null)
            .Map(dest => dest.Reviewed, src => src.Reviewed != null ? new UGHApi.ViewModels.UserComponent.UserC
            {
                User_Id = src.Reviewed.User_Id,
                FirstName = src.Reviewed.FirstName,
                LastName = src.Reviewed.LastName,
                ProfilePicture = src.Reviewed.ProfilePicture,
                IsDeleted = false,
                DeletedUserName = null
            } : null)
            .Map(dest => dest.Offer, src => src.Offer != null ? new UGH.Domain.Entities.Offer
            {
                Id = src.Offer.Id,
                Title = src.Offer.Title,
                Description = src.Offer.Description,
                UserId = src.Offer.UserId
            } : null);
    }

    private static List<string?> SplitAndTrim(string? input)
    {
#pragma warning disable CS8632
        return input?.Split(',').Select(h => h.Trim()).ToList() ?? new List<string?>();
    }
}
