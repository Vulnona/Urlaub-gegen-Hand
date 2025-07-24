using UGH.Domain.Entities;

namespace UGH.Domain.Interfaces;

public interface IMembershipRepository
{
    Task<List<Membership>> GetAllMembershipsAsync();
    Task<Membership> GetMembershipByDurationDaysAsync(int durationDays);
    Task<Membership> GetMembershipByIdAsync(int membershipId);
    Task<Membership> AddMembershipAsync(Membership membership);
    Task<Membership> UpdateMembershipAsync(Membership membership);
    Task<bool> DeleteMembershipAsync(int membershipId);
    Task<IEnumerable<Membership>> GetActiveMembershipsAsync();
}
