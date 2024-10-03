using Microsoft.EntityFrameworkCore;
using UGH.Domain.Interfaces;
using UGH.Domain.Entities;
using Mapster;
using UGHApi.ViewModels;

namespace UGH.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly Ugh_Context _context;

    public UserRepository(Ugh_Context context)
    {
        _context = context;
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        return await _context.users.FindAsync(userId);
    }

    public async Task<UserDTO> GetUserDetailsByIdAsync(Guid userId)
    {
        var user = await _context.users
            .Include(u => u.CurrentMembership)
            .Include(u => u.Offers)
            .ThenInclude(o => o.Reviews)
            .FirstOrDefaultAsync(u => u.User_Id == userId);

        if (user == null)
        {
            return null;
        }

        var userDto = user.Adapt<UserDTO>();

        return userDto;
    }

    public async Task<User> GetUserWithRatingByIdAsync(Guid userId)
    {
        return await _context.users
            .Include(u => u.Offers)
            .ThenInclude(o => o.Reviews)
            .FirstOrDefaultAsync(u => u.User_Id == userId);
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
    {
        var users = await _context.users
            .Include(u => u.Offers)
            .ThenInclude(o => o.Reviews)
            .ToListAsync();

        if (users == null)
        {
            return null;
        }

        var userDtoList = users.Adapt<List<UserDTO>>();

        return userDtoList;
    }

    public async Task<User> AddUserAsync(User user)
    {
        try
        {
            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while adding the user.", ex);
        }
    }

    public async Task<User> GetUserWithMembershipAsync(Guid userId)
    {
        return await _context.users
            .Include(u => u.CurrentMembership)
            .FirstOrDefaultAsync(u => u.User_Id == userId);
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await _context.users.FindAsync(userId);
        if (user != null)
        {
            _context.users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.users
            .Include(u => u.CurrentMembership)
            .FirstOrDefaultAsync(u => u.Email_Address == email);
    }
}
