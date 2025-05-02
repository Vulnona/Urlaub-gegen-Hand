using Microsoft.EntityFrameworkCore;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;

namespace UGHApi.Repositories;

public class UserMembershipRepository : IUserMembershipRepository
{
    private readonly Ugh_Context _context;

    public UserMembershipRepository(Ugh_Context context)
    {
        _context = context;
    }

    public async Task<UserMembership> GetUserMembershipByIdAsync(int id)
    {
        return await _context.usermembership.FindAsync(id);
    }

    public async Task<IEnumerable<UserMembership>> GetUserMembershipsByUserIdAsync(Guid userId)
    {
        return await _context.usermembership.Where(um => um.User_Id == userId).ToListAsync();
    }

    public async Task<UserMembership> AddUserMembershipAsync(UserMembership userMembership)
    {
        await _context.usermembership.AddAsync(userMembership);
        return userMembership;
    }

    public async Task<UserMembership> UpdateUserMembershipAsync(UserMembership userMembership)
    {
        _context.usermembership.Update(userMembership);
        await _context.SaveChangesAsync();
        return userMembership;
    }

    public async Task DeleteUserMembershipAsync(int id)
    {
        var userMembership = await GetUserMembershipByIdAsync(id);
        if (userMembership != null)
        {
            _context.usermembership.Remove(userMembership);
            await _context.SaveChangesAsync();
        }
    }
}
