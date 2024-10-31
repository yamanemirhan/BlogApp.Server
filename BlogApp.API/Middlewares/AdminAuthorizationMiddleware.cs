namespace BlogApp.API.Middlewares
{
    public class AdminAuthorizationMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var isAdminClaim = context.User.Claims.FirstOrDefault(c => c.Type == "isAdmin")?.Value;

            bool isAdmin = bool.TryParse(isAdminClaim, out var result) && result;

            if (isAdmin)
            {
                await next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Admin rights are required to perform this action.");
            }
        }
    }
}
