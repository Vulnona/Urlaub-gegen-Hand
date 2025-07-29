using Microsoft.AspNetCore.Http;
using UGH.Domain.Interfaces;
using UGH.Domain.Entities;

namespace UGH.Middleware;

public class Admin2FAMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<Admin2FAMiddleware> _logger;

    public Admin2FAMiddleware(RequestDelegate next, ILogger<Admin2FAMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IUserRepository userRepository)
    {
        // Skip middleware for authentication endpoints to avoid infinite loops
        var path = context.Request.Path.Value?.ToLower();
        if (path != null && (
            path.Contains("/api/authenticate/login") ||
            path.Contains("/api/authenticate/login-2fa") ||
            path.Contains("/api/authenticate/2fa/setup") ||
            path.Contains("/api/authenticate/2fa/verify-setup") ||
            path.Contains("/api/authenticate/change-password") ||
            path.Contains("/api/authenticate/register")
        ))
        {
            await _next(context);
            return;
        }

        // Get user from JWT token
        var userEmail = context.User?.Claims?.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(userEmail))
        {
            userEmail = context.User?.Claims?.FirstOrDefault(x => x.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
        }

        if (!string.IsNullOrEmpty(userEmail))
        {
            try
            {
                var user = await userRepository.GetUserByEmailAsync(userEmail);
                if (user != null && user.UserRole == UserRoles.Admin)
                {
                    if (!user.IsTwoFactorEnabled)
                    {
                        _logger.LogWarning($"Admin account {userEmail} accessed without 2FA enabled");
                        
                        // Return 403 Forbidden for admin accounts without 2FA
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync("{\"error\":\"Admin accounts must have 2FA enabled. Please complete 2FA setup.\"}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Admin2FAMiddleware: {ex.Message}");
                // Continue with request even if middleware fails
            }
        }

        await _next(context);
    }
} 