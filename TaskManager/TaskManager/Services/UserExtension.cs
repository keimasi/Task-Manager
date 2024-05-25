using System.Security.Claims;

namespace TaskManager.Services;

public static class UserExtension
{
    public static int GetPkUser(this ClaimsPrincipal user)
    {
        var claim= user.Claims.FirstOrDefault(x => x.Type == "Id");
        
        if (claim == null)
        {
            return 0;
        }

        return int.Parse(claim.Value);
    }
}