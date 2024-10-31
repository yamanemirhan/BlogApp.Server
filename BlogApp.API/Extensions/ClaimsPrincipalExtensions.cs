using System.Security.Claims;

namespace BlogApp.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {

            var userIdClaim = user.FindFirst("userId") ?? user.FindFirst("sub");
            Console.WriteLine("userIdClaim: " + int.Parse(userIdClaim.Value));
            if (userIdClaim == null)
            {
                throw new InvalidOperationException("User ID claim not found.");
            }

            return int.Parse(userIdClaim.Value);
        }
    }
}
