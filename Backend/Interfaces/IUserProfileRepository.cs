using UGH.Domain.Entities;

namespace UGH.Domain.Interfaces;

public interface IUserProfileRepository
{
    Task<UserProfile> GetUserProfileByUserIdAsync(Guid userId);
    Task AddUserProfileAsync(UserProfile profile);
    Task UpdateUserProfileAsync(UserProfile profile);
    Task SaveChangesAsync();
}
