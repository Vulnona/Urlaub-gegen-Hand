using MediatR;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;

namespace UGHApi.Applications.Memberships;

public class PurchaseMembershipCommandHandler
    : IRequestHandler<PurchaseMembershipCommand, Result<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMembershipRepository _membershipRepository;

    public PurchaseMembershipCommandHandler(
        UGH.Domain.Interfaces.IUserRepository userRepository,
        IMembershipRepository membershipRepository
    )
    {
        _userRepository = userRepository;
        _membershipRepository = membershipRepository;
    }

    public async Task<Result<bool>> Handle(
        PurchaseMembershipCommand request,
        CancellationToken cancellationToken
    )
    {
        var newMembership = await _membershipRepository.GetMembershipByIdAsync(
            request.MembershipId
        );
        var user = await _userRepository.GetUserForMembershipByIdAsync(request.UserId);

        if (user == null)
        {
            return Result.Failure<bool>(Errors.General.NotFound("User", user));
        }

        DateTime newStartDate;

        if (user.UserMemberships.Any(m => m.IsMembershipActive))
        {
            var activeMembership = user.UserMemberships.First(m => m.IsMembershipActive);
            newStartDate = activeMembership.Expiration;
        }
        else
        {
            newStartDate = DateTime.UtcNow;
        }

        var newUserMembership = new UserMembership
        {
            User_Id = user.User_Id,
            MembershipID = newMembership.MembershipID,
            StartDate = newStartDate,
            Expiration = newStartDate.AddMonths(newMembership.DurationDays),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        await _userRepository.AddUserMembership(newUserMembership);
        await _userRepository.SaveChangesAsync();

        if (user.CurrentMembership == null)
        {
            user.CurrentMembership = newMembership;
            await _userRepository.UpdateUserAsync(user);
        }

        return Result.Success(true);
    }
}
