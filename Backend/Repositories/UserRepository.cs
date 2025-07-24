using Mapster;
using Microsoft.EntityFrameworkCore;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGHApi.Repositories;
using UGHApi.Services.AWS;
using UGHApi.Shared;
using UGHApi.ViewModels;
using UGHApi.ViewModels.UserComponent;
using UGHApi.DATA;
using Microsoft.Extensions.Logging;

namespace UGH.Infrastructure.Repositories;

#pragma warning disable CS8632

public class UserRepository : IUserRepository
{
    private readonly Ugh_Context _context;
    private readonly IUrlBuilderService _urlBuilderService;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(Ugh_Context context, IUrlBuilderService urlBuilderService, ILogger<UserRepository> logger)
    {
        _context = context;
        _urlBuilderService = urlBuilderService;
        _logger = logger;
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

    public async Task DeactivateExpiredMembershipsAsync()
    {
        // TODO: Implement logic to deactivate expired memberships
        await Task.CompletedTask;
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

        userDto.MembershipEndDate = user.UserMemberships
        .OrderBy(m => m.CreatedAt)
        .FirstOrDefault()?.Expiration;

        return userDto;
    }

    public async Task AddUserMembership(UserMembership userMembership)
    {
        await _context.usermembership.AddAsync(userMembership);
    }

    public async Task<User> GetUserWithRatingByIdAsync(Guid userId)
    {
        return await _context
            .users.Include(u => u.UserMemberships)
            .Include(u => u.Offers)
            .ThenInclude(o => o.Reviews)
            .FirstOrDefaultAsync(u => u.User_Id == userId);
    }

    public async Task<PaginatedList<UserDTO>> GetAllUsersAsync(UserQueryParameters parameters)
    {
        IQueryable<User> query = _context
            .users
            .Include(u => u.UserMemberships)
            .Include(u => u.Offers)
                .ThenInclude(o => o.Reviews)
            .Where(u => u.UserRole != UserRoles.Admin)
            .AsQueryable();

        if (!string.IsNullOrEmpty(parameters.SearchTerm))
        {
            query = query.Where(u => u.Email_Address.Contains(parameters.SearchTerm));
        }

        if (!string.IsNullOrEmpty(parameters.SortBy))
        {
            var propertyInfo = typeof(User).GetProperty(parameters.SortBy);
            if (propertyInfo != null)
            {
                query =
                    parameters.SortDirection.ToLower() == "desc"
                        ? query.OrderByDescending(e => EF.Property<object>(e, parameters.SortBy))
                        : query.OrderBy(e => EF.Property<object>(e, parameters.SortBy));
            }
        }

        int totalCount = await query.CountAsync();

        var users = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
        .ToListAsync();

        if (users == null || !users.Any())
        {
            return null;
        }
        TypeAdapterConfig<User, UserDTO>
            .NewConfig()
            .Map(dest => dest.Hobbies, src => SplitAndTrim(src.Hobbies))
            .Map(dest => dest.Skills, src => SplitAndTrim(src.Skills))
            .Map(dest => dest.Link_RS, src => _urlBuilderService.BuildAWSFileUrl(src.Link_RS))
            .Map(dest => dest.Link_VS, src => _urlBuilderService.BuildAWSFileUrl(src.Link_VS))
            ; 

        var userDtoList = users.Select(user =>
        {
            var dto = user.Adapt<UserDTO>();
            dto.MembershipEndDate = user.UserMemberships
                .Where(m => m.IsMembershipActive)
                .OrderBy(m => m.CreatedAt)
                .FirstOrDefault()?.Expiration;
            return dto;
        }).ToList();


        return PaginatedList<UserDTO>.Create(
            userDtoList,
            totalCount,
            parameters.PageNumber,
            parameters.PageSize
        );
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
            var transactions = _context.transaction.Where(t => t.UserId == user.User_Id);
            var coupons = _context.coupons.Where(t => t.CreatedBy == user.User_Id);

            _context.transaction.RemoveRange(transactions);
            _context.coupons.RemoveRange(coupons);

            _context.users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context
            .users
            .Include(u => u.UserMemberships)
                .ThenInclude(um => um.Membership)
            .Include(u => u.CurrentMembership)
            .FirstOrDefaultAsync(u => u.Email_Address == email);
    }

    public async Task<List<UserMembership>> GetActiveUserMembershipsAsync(Guid userId)
    {
        var now = DateTime.Now; // Deutsche lokale Zeit verwenden
        
        // Debug: Direkter SQL Query zur Sicherheit
        var rawQuery = $"SELECT * FROM usermembership WHERE User_Id = '{userId}' AND StartDate <= '{now:yyyy-MM-dd HH:mm:ss}' AND Expiration > '{now:yyyy-MM-dd HH:mm:ss}'";
        _logger.LogError($"=== DEBUG SQL: {rawQuery} ===");
        _logger.LogError($"=== CURRENT UTC TIME: {now:yyyy-MM-dd HH:mm:ss} ===");
        
        var result = await _context.Set<UserMembership>()
            .Include(um => um.Membership)
            .Where(um => um.User_Id == userId && um.StartDate <= now && um.Expiration > now)
            .ToListAsync();
            
        _logger.LogError($"=== FOUND {result.Count} ACTIVE MEMBERSHIPS ===");
        
        return result;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
