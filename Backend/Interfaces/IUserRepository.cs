using UGH.Domain.Entities;
using UGHApi.Repositories;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGH.Domain.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(Guid userId);
    Task<UserDTO> GetUserDetailsByIdAsync(Guid userId);
    Task<PaginatedList<UserDTO>> GetAllUsersAsync(UserQueryParameters parameters);
    Task<User> AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid userId);
    Task SaveChangesAsync();
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserWithMembershipAsync(Guid userId);
    Task<User> GetUserWithRatingByIdAsync(Guid userId);
    Task AddUserMembership(UserMembership userMembership);
    Task<User> GetUserForMembershipByIdAsync(Guid userId);
}
