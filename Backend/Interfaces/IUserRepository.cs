using UGH.Domain.Entities;
using UGHApi.ViewModels;

namespace UGH.Domain.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(Guid userId);
    Task<UserDTO> GetUserDetailsByIdAsync(Guid userId);
    Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    Task<User> AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid userId);
    Task SaveChangesAsync();
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserWithMembershipAsync(Guid userId);
    Task<User> GetUserWithRatingByIdAsync(Guid userId);
}
