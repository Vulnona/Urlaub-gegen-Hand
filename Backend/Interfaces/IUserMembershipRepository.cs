using UGH.Domain.Entities;

namespace UGH.Domain.Interfaces;

public interface IUserMembershipRepository
{
    Task<UserMembership> GetUserMembershipByIdAsync(int id);
    Task<IEnumerable<UserMembership>> GetUserMembershipsByUserIdAsync(Guid userId);
    Task<UserMembership> AddUserMembershipAsync(UserMembership userMembership);
    Task<UserMembership> UpdateUserMembershipAsync(UserMembership userMembership);
    Task DeleteUserMembershipAsync(int id);
}
