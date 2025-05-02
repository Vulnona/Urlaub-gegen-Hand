using Microsoft.EntityFrameworkCore;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;

namespace UGHApi.Repositories;

public class MembershipRepository : IMembershipRepository
{
    private readonly Ugh_Context _context;

    public MembershipRepository(Ugh_Context context)
    {
        _context = context;
    }

    public async Task<List<Membership>> GetAllMembershipsAsync()
    {
        return await _context.memberships.ToListAsync();
    }

    public async Task<Membership> GetMembershipByDurationDaysAsync(int durationDays)
    {
        return await _context.memberships.FirstOrDefaultAsync(m => m.DurationDays == durationDays);
    }

    public async Task<Membership> GetMembershipByIdAsync(int membershipId)
    {
        return await _context.memberships.FindAsync(membershipId);
    }

    public async Task<Membership> AddMembershipAsync(Membership membership)
    {
        await _context.memberships.AddAsync(membership);
        await _context.SaveChangesAsync();
        return membership;
    }

    public async Task<Membership> UpdateMembershipAsync(Membership membership)
    {
        _context.memberships.Update(membership);
        await _context.SaveChangesAsync();
        return membership;
    }

    public async Task<bool> DeleteMembershipAsync(int membershipId)
    {
        var membership = await GetMembershipByIdAsync(membershipId);
        if (membership == null)
            return false;

        _context.memberships.Remove(membership);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Membership>> GetActiveMembershipsAsync()
    {
        return await _context.memberships.ToListAsync();
    }
}
