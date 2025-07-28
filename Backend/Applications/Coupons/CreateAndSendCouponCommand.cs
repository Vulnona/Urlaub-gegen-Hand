#nullable enable
using MediatR;
using UGH.Domain.Core;

namespace UGHApi.Applications.Coupons;

public class CreateAndSendCouponCommand : IRequest<Result>
{
    public string Email { get; }
    public int MembershipId { get; }
    public string? Name { get; }

    public CreateAndSendCouponCommand(string email, int membershipId, string? name = null)
    {
        Email = email;
        MembershipId = membershipId;
        Name = name;
    }
} 