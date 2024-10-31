using BlogApp.Application.Interfaces;

namespace BlogApp.API.Middlewares
{
    public class TokenValidationMiddleware(RequestDelegate next, ITokenBlacklistService tokenBlacklistService)
    {

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token) && tokenBlacklistService.IsBlacklisted(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token has been invalidated.");
                return;
            }

            await next(context);
        }
    }
}
