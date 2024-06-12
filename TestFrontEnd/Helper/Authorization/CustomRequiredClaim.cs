using Microsoft.AspNetCore.Authorization;

namespace Frontend.WebUI.Helper.Authorization
{
  public class CustomRequiredClaim : IAuthorizationRequirement
  {
    public string ClaimType { get; }
    public CustomRequiredClaim(string claimType)
    {
      ClaimType = claimType;
    }
  }
}
