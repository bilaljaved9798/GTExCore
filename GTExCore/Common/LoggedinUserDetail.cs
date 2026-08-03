using System.Security.Claims;

namespace GTExCore.Common
{
    public static class LoggedinUserDetailAPI
    {
        public static int GetUserId(HttpContext httpContext)
        {
            var value = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return string.IsNullOrEmpty(value) ? 0 : Convert.ToInt32(value);
        }

        public static int GetUserType(HttpContext httpContext)
        {
            var value = httpContext.User.FindFirst("UserType")?.Value;
            return string.IsNullOrEmpty(value) ? 0 : Convert.ToInt32(value);
        }

        public static string GetUserName(HttpContext httpContext)
        {
            return httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}
