using System;
using System.Linq;
using System.Security.Claims;
using Enxaquecapp.WebApi.Security;

namespace Enxaquecapp.WebApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid UserId(this ClaimsPrincipal claimsPrincipal)
        {
            var id = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == DefaultClaims.UserId)?.Value;
            return Guid.Parse(id);
        }
    }
}