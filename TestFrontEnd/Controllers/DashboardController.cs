using Frontend.Web.Models.CommonModels;
using Frontend.Web.Models.Functions;
using Frontend.Web.Models.TaskSetup;
using Frontend.WebUI.Extensions.HttpConfig;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using static Frontend.Web.Models.Functions.CommonEnumsUI;
using System.Reflection;

namespace Frontend.WebUI.Controllers
{
    public class DashboardController : Controller
    {

        private readonly TaskClient _httpClient;
        private readonly ApplicationSettings _settings;
        public DashboardController(TaskClient httpClient, IOptions<ApplicationSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _httpClient.Client.BaseAddress = new Uri(settings.Value.APIBaseUrl);


        }
        public async Task<IActionResult> Index()
        {
            ClaimsInfoModel _userClaimObj = CommonFunctionUI.LoginUsersClaims(User.Claims.ToList());
            HttpResponseMessage _response = new HttpResponseMessage();
            BackEndTaskModel backEndTaskModel = new BackEndTaskModel();
            _response = await _httpClient.Client.GetAsync(BackendPath.GetAllTask);
            if (_response.IsSuccessStatusCode)
            {
                var _dataResponse = await _response.Content.ReadAsStringAsync();
                List<BackendTask> _desObject = JsonConvert.DeserializeObject<List<BackendTask>>(_dataResponse);
                backEndTaskModel.BackendTask = _desObject;
                backEndTaskModel.PostModel = new ItemPostDto();
                backEndTaskModel.PutModel = new ItemPutDto();

                return View(backEndTaskModel);
            }
            else
            {
                var _dataResponse = await _response.Content.ReadAsStringAsync();
                ResponseMessage _desObject = JsonConvert.DeserializeObject<ResponseMessage>(_dataResponse);
                return View();
                // return Json(CommonFunctionUI.JsonResponse(ResponseType.Failure, _desObject.StatusMessage, ""));
            }




            //return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ItemPostDto model)
        {
            HttpResponseMessage _response = new HttpResponseMessage();
            if (ModelState.IsValid)
            {
                ClaimsInfoModel _userClaimObj = CommonFunctionUI.LoginUsersClaims(User.Claims.ToList());
                string _jsonObjectModel = JsonConvert.SerializeObject(model);

                _response = await _httpClient.Client.PostAsync(BackendPath.PostTask, new StringContent(_jsonObjectModel, Encoding.UTF8, "application/json"));

              

                if (_response.IsSuccessStatusCode)
                {
                    var _dataResponse = await _response.Content.ReadAsStringAsync();
                    ResponseMessage _desObject = JsonConvert.DeserializeObject<ResponseMessage>(_dataResponse);
                    return RedirectToAction("Index");
                    //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, CommonFunctionUI.ClaimsUpdate_Type((ClaimsIdentity)User.Identity, LoginUserClaim.MasterOpportunityId, _desObject.Id.ToString()));
                    //return Json(CommonFunctionUI.JsonResponse(ResponseType.Success, "Record updated successfully.", ""));
                }
                else
                {
                    var _dataResponse = await _response.Content.ReadAsStringAsync();
                    ResponseMessage _desObject = JsonConvert.DeserializeObject<ResponseMessage>(_dataResponse);
                    return RedirectToAction("Index");
                    //return Json(CommonFunctionUI.JsonResponse(ResponseType.Failure, _desObject.StatusMessage, ""));
                }
            }
            else
            {
                string ErrorMessage = "";
                var errors = ModelState.Select(x => x.Value.Errors)
                        .Where(y => y.Count > 0)
                        .FirstOrDefault();
                ErrorMessage = errors[0].ErrorMessage;

                return Json(CommonFunctionUI.JsonResponse(ResponseType.Failure, ErrorMessage, ""));
            }
        }
        public IActionResult UpdateItemView(int userId)
        {
            HttpContext.Session.SetString("UpdateItemId", userId.ToString());
            return ViewComponent("UpdateItem");
        }



        [HttpPost]
        public async Task<IActionResult> Update(ItemPutDto model)
        {
            try
            {
                HttpResponseMessage _response = new HttpResponseMessage();
                string _jsonObjectModel = JsonConvert.SerializeObject(model);
                _response = await _httpClient.Client.PutAsync(BackendPath.PutTask, new StringContent(_jsonObjectModel, Encoding.UTF8, "application/json"));

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error updating product: " + ex.Message });
            }
        }


        public async Task<IActionResult> Delete(long id)
        {
            HttpResponseMessage _response = await _httpClient.Client.DeleteAsync(BackendPath.Delete + id);
            if (_response.IsSuccessStatusCode)
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Record Deleted successfully";
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }


    }
}
