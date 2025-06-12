using System.Security.Claims;

namespace velora.api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string RetrieveEmailFromPrincipal(this ClaimsPrincipal user)
        {
            return user?.FindFirstValue(ClaimTypes.Email);
        }
    }
}
