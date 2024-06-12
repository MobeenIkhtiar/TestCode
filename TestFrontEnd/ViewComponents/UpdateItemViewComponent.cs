using Frontend.Web.Models.CommonModels;
using Frontend.Web.Models.Functions;
using Frontend.Web.Models.TaskSetup;
using Frontend.WebUI.Extensions.HttpConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Frontend.WebUI.ViewComponents
{
    [ViewComponent(Name = "UpdateItem")]
    public class UpdateItemViewComponent : ViewComponent
    {


        private readonly TaskClient _httpClient;
        private readonly ApplicationSettings _settings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UpdateItemViewComponent(TaskClient httpClient, IOptions<ApplicationSettings> settings, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _httpClient.Client.BaseAddress = new Uri(settings.Value.APIBaseUrl);
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            BackEndTaskModel model = new BackEndTaskModel();
            string updateItemId = HttpContext.Session.GetString("UpdateItemId");
            long Id = 0;
            if (!string.IsNullOrEmpty(updateItemId))
            {
                Id = Convert.ToInt64(updateItemId);
            }
            // ClaimsInfoModel _userClaimObj = CommonFunctionUI.LoginUsersClaims(_httpContextAccessor.HttpContext.User.Claims.ToList());
            string result = await _httpClient.Client.GetStringAsync(BackendPath.GetById + Id);
            
            
            BackendTask _object = JsonConvert.DeserializeObject<BackendTask>(result);
            if (_object != null)
            {
                model.PutModel = new ItemPutDto
                {
                    Id = _object.Id,
                    Name = _object.Name
                };
            }
            else
            {
                model.BackendTask = new List<BackendTask>();
                model.PutModel = new ItemPutDto();
                model.PostModel = new ItemPostDto();
            }
            HttpContext.Session.Remove("UpdateItemId");
            return View(model);
        }
    }
}
