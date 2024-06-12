using Microsoft.AspNetCore.Authorization;

namespace Frontend.WebUI.Helper.Authorization
{
  public class PoliciesAuthorizationHandler : AuthorizationHandler<CustomRequiredClaim>
  {
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CustomRequiredClaim requirement)
    {
      if (context.User == null || !context.User.Identity.IsAuthenticated)
      {
        context.Fail();
        return Task.CompletedTask;
      }

      var hasClaim = context.User.Claims.Any(x => x.Type == requirement.ClaimType);

      if (hasClaim)
      {
        context.Succeed(requirement);
        return Task.CompletedTask;
      }

      context.Fail();
      return Task.CompletedTask;
    }
  }
}
