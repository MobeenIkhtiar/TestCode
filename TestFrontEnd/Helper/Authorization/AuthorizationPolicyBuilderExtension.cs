using Microsoft.AspNetCore.Authorization;

namespace Frontend.WebUI.Helper.Authorization
{
  public static class AuthorizationPolicyBuilderExtension
  {
    public static AuthorizationPolicyBuilder UserRequireCustomClaim(this AuthorizationPolicyBuilder builder, string claimType)
    {
      builder.AddRequirements(new CustomRequiredClaim(claimType));
      return builder;
    }
  }
}
