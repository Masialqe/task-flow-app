using System.Security.Claims;

namespace TFA.App.Extensions.Common;

public static class ClaimsPrincipalExtension
{
    public static string GetUserId(this ClaimsPrincipal principal)
        => principal.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
}