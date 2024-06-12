using Frontend.Web.Models.CommonModels;
using Frontend.Web.Models.Functions;
using Microsoft.Extensions.Options;

namespace Frontend.WebUI.Extensions.HttpConfig
{
  public class TaskClient
  {
    private ApplicationSettings _settings;
    public HttpClient Client { get; private set; }
    public TaskClient(IOptions<ApplicationSettings> iconfig)
    {
      _settings = iconfig.Value;
    }
    public TaskClient(HttpClient client, IHttpContextAccessor httpContextAccessor)
    {
      ClaimsInfoModel _userClaimObj = CommonFunctionUI.LoginUsersClaims(httpContextAccessor.HttpContext.User.Claims.ToList());

      string access_token = string.Empty;
      if (_userClaimObj != null)
      {
        access_token = _userClaimObj.AccessToken;
      }
      client.DefaultRequestHeaders.Add("Accept", "application/json");
      client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sapcars");
      client.DefaultRequestHeaders.Add("Authorization", "Bearer " + access_token);
      Client = client;
    }

  }
}

