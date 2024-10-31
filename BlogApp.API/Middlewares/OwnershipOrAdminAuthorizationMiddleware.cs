using BlogApp.Application.Interfaces;
using BlogApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace BlogApp.API.Middlewares
{
    public class OwnershipOrAdminAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public OwnershipOrAdminAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var isAdminClaim = context.User.Claims.FirstOrDefault(c => c.Type == "isAdmin")?.Value;
            bool isAdmin = bool.TryParse(isAdminClaim, out var result) && result;

            if (int.TryParse(userIdClaim, out var userId) && (isAdmin || await IsOwnerOfResourceAsync(context, userId, serviceProvider)))
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("You are not authorized to perform this action.");
            }
        }

        private async Task<bool> IsOwnerOfResourceAsync(HttpContext context, int userId, IServiceProvider serviceProvider)
        {
            string resourceId = null;
            IResourceOwnershipService ownershipService = null;

            if (context.Request.Path.StartsWithSegments("/api/post"))
            {
                Console.WriteLine("POST CHECK");
                resourceId = context.Request.RouteValues["postId"]?.ToString() ??
                           context.Request.RouteValues["id"]?.ToString(); 
                ownershipService = serviceProvider.GetService<IPostService>() as IResourceOwnershipService;
            }
            else if (context.Request.Path.StartsWithSegments("/api/comment"))
            {
                Console.WriteLine("COMMENT CHECK");
                resourceId = context.Request.RouteValues["commentId"]?.ToString() ??
                           context.Request.RouteValues["id"]?.ToString(); 
                ownershipService = serviceProvider.GetService<ICommentService>() as IResourceOwnershipService;
            }

            if (string.IsNullOrEmpty(resourceId) || ownershipService == null)
            {
                return false;
            }

            if (int.TryParse(resourceId, out int parsedResourceId))
            {
                return await ownershipService.IsOwnerAsync(parsedResourceId, userId);
            }

            return false;
        }
    }
}
