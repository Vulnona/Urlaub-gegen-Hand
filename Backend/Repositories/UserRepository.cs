using Mapster;
using Microsoft.EntityFrameworkCore;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGHApi.Services.AWS;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGH.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly Ugh_Context _context;
    private readonly IUrlBuilderService _urlBuilderService;

    public UserRepository(Ugh_Context context, IUrlBuilderService urlBuilderService)
    {
        _context = context;
        _urlBuilderService = urlBuilderService;
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        return await _context.users.FindAsync(userId);
    }

    public async Task<User> GetUserForMembershipByIdAsync(Guid userId)
    {
        return await _context
            .users.Include(u => u.CurrentMembership)
            .Include(u => u.UserMemberships)
            .FirstOrDefaultAsync(u => u.User_Id == userId);
    }

    public async Task<UserDTO> GetUserDetailsByIdAsync(Guid userId)
    {
        var user = await _context
            .users.Include(u => u.CurrentMembership)
            .Include(u => u.UserMemberships)
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

    public async Task AddUserMembership(UserMembership userMembership)
    {
        await _context.usermembership.AddAsync(userMembership);
    }

    public async Task<User> GetUserWithRatingByIdAsync(Guid userId)
    {
        return await _context
            .users.Include(u => u.Offers)
            .ThenInclude(o => o.Reviews)
            .FirstOrDefaultAsync(u => u.User_Id == userId);
    }

    public async Task<PaginatedList<UserDTO>> GetAllUsersAsync(int pageNumber, int pageSize)
    {
        IQueryable<User> query = _context
            .users.Include(u => u.Offers)
            .ThenInclude(o => o.Reviews)
            .OrderBy(u => u.VerificationState)
            .AsQueryable();

        int totalCount = await query.CountAsync();

        var users = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        if (users == null || !users.Any())
        {
            return null;
        }

        TypeAdapterConfig<User, UserDTO>
            .NewConfig()
            .Map(dest => dest.Hobbies, src => SplitAndTrim(src.Hobbies))
            .Map(dest => dest.Skills, src => SplitAndTrim(src.Skills))
            .Map(dest => dest.Link_RS, src => _urlBuilderService.BuildAWSFileUrl(src.Link_RS))
            .Map(dest => dest.Link_VS, src => _urlBuilderService.BuildAWSFileUrl(src.Link_VS));

        var userDtoList = users.Adapt<List<UserDTO>>();

        return PaginatedList<UserDTO>.Create(userDtoList, totalCount, pageNumber, pageSize);
    }

    private static List<string?> SplitAndTrim(string? input)
    {
        return input?.Split(',').Select(h => h.Trim()).ToList() ?? new List<string?>();
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
        return await _context
            .users.Include(u => u.CurrentMembership)
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
        return await _context
            .users.Include(u => u.UserMemberships)
            .Include(u => u.CurrentMembership)
            .FirstOrDefaultAsync(u => u.Email_Address == email);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
