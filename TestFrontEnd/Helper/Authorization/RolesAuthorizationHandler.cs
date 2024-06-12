using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Frontend.WebUI.Helper.Authorization
{
  public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
  {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   RolesAuthorizationRequirement requirement)
    {
      if (context.User == null || !context.User.Identity.IsAuthenticated)
      {
        context.Fail();
        return Task.CompletedTask;
      }

      var validRole = false;
      if (requirement.AllowedRoles == null ||
          requirement.AllowedRoles.Any() == false)
      {
        validRole = true;
      }
      else
      {
        var claims = context.User.Claims;
        var userName = claims.FirstOrDefault(c => c.Type == "UserName").Value;
        var userRole = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
        var roles = requirement.AllowedRoles;

        validRole = roles.Contains(userRole);

      }

      if (validRole)
      {
        context.Succeed(requirement);
      }
      else
      {
        context.Fail();
      }
      return Task.CompletedTask;
    }
  }
}
