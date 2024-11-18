using UGH.Domain.Interfaces;
using UGH.Domain.Entities;
using MediatR;

namespace UGHApi.Applications.Admin;

public class AssignMembershipCommandHandler : IRequestHandler<AssignMembershipCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly Repositories.Interfaces.IMembershipRepository _membershipRepository;

    public AssignMembershipCommandHandler(IUserRepository userRepository, Repositories.Interfaces.IMembershipRepository membershipRepository)
    {
        _userRepository = userRepository;
        _membershipRepository = membershipRepository;
    }

    public async Task<bool> Handle(AssignMembershipCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserForMembershipByIdAsync(request.UserId);

        if (user == null) return false;

        var existingActiveMembership = user.UserMemberships.Where(um => um.IsMembershipActive).FirstOrDefault();
        var newMembershipStartDate = existingActiveMembership != null
            ? existingActiveMembership.Expiration
            : DateTime.Now;

        var membership = await _membershipRepository.GetMembershipByIdAsync(request.MembershipId);

        if (membership == null) return false;

        if (existingActiveMembership == null)
        {
            user.SetMembershipId(membership.MembershipID);
            await _userRepository.UpdateUserAsync(user);
        }

        var userMembership = new UserMembership
        {
            User_Id = request.UserId,
            MembershipID = request.MembershipId,
            StartDate = newMembershipStartDate,
            Expiration = newMembershipStartDate.AddDays(membership.DurationDays),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        await _userRepository.AddUserMembership(userMembership);
        await _userRepository.SaveChangesAsync();

        return true;
    }
}
