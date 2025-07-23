using UGH.Domain.Interfaces;
using UGH.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using UGHApi.DATA;

namespace UGHApi.Repositories;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly Ugh_Context _context;

    public UserProfileRepository(Ugh_Context context)
    {
        _context = context;
    }

    public async Task<UserProfile> GetUserProfileByUserIdAsync(Guid userId)
    {
        return await _context.userprofiles.FirstOrDefaultAsync(p => p.User_Id == userId);
    }

    public async Task AddUserProfileAsync(UserProfile profile)
    {
        await _context.userprofiles.AddAsync(profile);
    }

    public async Task UpdateUserProfileAsync(UserProfile profile)
    {
        _context.userprofiles.Update(profile);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
