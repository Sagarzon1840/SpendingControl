using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace SpendingControl.Api.Helpers
{
    public static class UserContextHelper
    {
        public static Guid GetUserId(HttpContext httpContext)
        {
            var user = httpContext.User;
            var sub = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(sub) || !Guid.TryParse(sub, out var userId))
                throw new UnauthorizedAccessException("Invalid user id in token");
            return userId;
        }
    }
}
