using UGHApi.Services.UserProvider;
using System.Security.Claims;

public class UserProvider : IUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Guid UserId { get; private set; }

    public UserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        if (_httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(
                c => c.Type == ClaimTypes.NameIdentifier
            );

            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                UserId = userId;
            }
        }
    }
}
