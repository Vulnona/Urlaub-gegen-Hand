using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using UGHApi.DATA;

namespace UGHApi.Middleware
{
    public class UserExistenceMiddleware
    {
        private readonly RequestDelegate _next;

        public UserExistenceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Nur prüfen, wenn ein Authorization-Header existiert und Bearer-Token verwendet wird
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                var tokenService = context.RequestServices.GetService(typeof(UGH.Infrastructure.Services.TokenService)) as UGH.Infrastructure.Services.TokenService;
                Guid? userId = null;
                try
                {
                    userId = await tokenService.GetUserIdFromToken(token);
                }
                catch
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Invalid token");
                    return;
                }
                if (userId == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: User does not exist");
                    return;
                }
            }
            await _next(context);
        }
    }
}
