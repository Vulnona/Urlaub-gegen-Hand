using Microsoft.EntityFrameworkCore;
using UGHApi.DATA;

namespace UGHApi.Services.BackgroundTasks;

public class MembershipService
{
    private readonly Ugh_Context _context;

    public MembershipService(Ugh_Context context)
    {
        _context = context;
    }

    public void UpdateExpiredMemberships()
    {
        //try task.run here.

        // Get all expired memberships
        var expiredMemberships = _context
            .usermembership.Include(u => u.User)
            .Where(um => um.Expiration <= DateTime.UtcNow)
            .ToList();

        foreach (var membership in expiredMemberships)
        {
            if (membership.User != null)
            {
                membership.User.SetMembershipId(2);
            }

            //Mail Code here.
        }

        _context.SaveChanges();
    }
}
