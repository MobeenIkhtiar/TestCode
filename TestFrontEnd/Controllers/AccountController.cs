using Frontend.Web.Models.CommonModels;
using Frontend.Web.Models.Functions;
using Frontend.Web.Models.UsersSetup;
using Frontend.WebUI.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static Frontend.WebUI.Helper.ExportModelStateAttribute;
using System.Security.Claims;
using System.Text;
using Frontend.WebUI.Extensions.HttpConfig;
using System.IdentityModel.Tokens.Jwt;

namespace Frontend.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly TaskClient _httpClient;
        private readonly ApplicationSettings _settings;

        public AccountController(TaskClient httpClient, IOptions<ApplicationSettings> settings)

        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _httpClient.Client.BaseAddress = new Uri(settings.Value.APIBaseUrl);

        }
        public async Task<IActionResult> Login()
        {
            ViewBag.BasePath = _settings.APIBaseUrl;
            LoginModel user = CheckCookies();
            if (user == null)
            {
                LoginModel viewModel = new LoginModel();
                return View(viewModel);
            }
            else
            {

                LoginModel viewModel = new LoginModel();
                return View(viewModel);
                viewModel.Username = user.Username;
                viewModel.Password = user.Password;
                Result _result = await LoginFuntion(viewModel);

                if (_result.Succeeded)
                {
                    string RedirectUrl = "";

                    RedirectUrl = TempData["DasboardUrl"].ToString();
                    return LocalRedirect(RedirectUrl);
                }
                else
                {

                    return View(viewModel);
                }
            }
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {

            if (ModelState.IsValid)
            {

                Result _result = await LoginFuntion(loginModel);
                if (_result.Succeeded)
                {

                    string RedirectUrl = "";

                    ClaimsInfoModel _userClaimObj = CommonFunctionUI.LoginUsersClaims(User.Claims.ToList());

                    RedirectUrl = TempData["DasboardUrl"].ToString();


                    if (loginModel.RememberMe)
                    {

                        SetCookie("UserName", loginModel.Username, DateTime.Now.AddDays(_settings.CookieExpires));
                        SetCookie("Password", loginModel.Password, DateTime.Now.AddDays(_settings.CookieExpires));

                    }

                    return LocalRedirect(RedirectUrl);
                }
                else
                {
                    var errorMessage = _result.Errors[0].ToString();
                    if (errorMessage == "email is not verified")
                    {
                        //loginModel.IsResend = true;
                        return View(loginModel);
                    }

                    else
                    {
                        ModelState.AddModelError(string.Empty, _result.Errors[0]);
                        return View(loginModel);
                    }

                }

            }

            return View(loginModel);
        }

        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            ClaimsInfoModel _userClaimObj = CommonFunctionUI.LoginUsersClaims(User.Claims.ToList());
            if (!String.IsNullOrEmpty(_userClaimObj.RefreshToken))
            {
                var response = await _httpClient.Client.GetAsync(AccountPath.Logout + "?refreshToken=" + _userClaimObj.RefreshToken);
                if (response.IsSuccessStatusCode)
                {


                    if (Request.Cookies["UserName"] != null)
                    {
                        RemoveCookie("UserName");
                    }
                    if (Request.Cookies["Password"] != null)
                    {
                        RemoveCookie("Password");
                    }
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Logout successfully";
            return RedirectToAction("Login");

        }







        //=========================>>>>>>>>>>>>>>>--------------------------//
        //=========================>>>>>>>>>>>>>>> Functions----------------//
        //=========================>>>>>>>>>>>>>>>--------------------------//
        public async Task<Result> LoginFuntion(LoginModel loginModel)
        {
            string _serializeObj = JsonConvert.SerializeObject(loginModel);
            HttpResponseMessage _response = await _httpClient.Client.PostAsync(AccountPath.Login, new StringContent(_serializeObj, Encoding.UTF8, "application/json"));

            if (_response.IsSuccessStatusCode)
            {
                var _dataResponse = await _response.Content.ReadAsStringAsync();
                LoginResponseModel _desObject = JsonConvert.DeserializeObject<LoginResponseModel>(_dataResponse);
                var handler = new JwtSecurityTokenHandler();
                var readToken = handler.ReadJwtToken(_desObject.access_token);
                var payloadClaimsList = readToken.Payload.Claims.ToList();
                var emailConfirmed = payloadClaimsList[8].Value;
                var userId = payloadClaimsList[7].Value;
                var displayName = payloadClaimsList[5].Value;
                var organizationId = payloadClaimsList[9].Value;
                var rolename = payloadClaimsList[10].Value;
                var profilelink = payloadClaimsList[11].Value;
                if (emailConfirmed != "True")
                {
                    string sessionKey_EmailNotverified = "_Email";
                    HttpContext.Session.SetString(sessionKey_EmailNotverified, loginModel.Username);
                    ResponseMessage _ResponseObject = new ResponseMessage();
                    _ResponseObject.StatusMessage = "Your email is not verified. Please resend email to verify your account.";
                    _ResponseObject.StatusCode = 400;
                    _ResponseObject.StatusTitle = "BadRequest";
                    return Result.Failure(CommonFunctionUI.ErrorResponseToErrorResult(_ResponseObject));
                }


                var userClaims = new List<Claim>()
                    {
                        new Claim(LoginUserClaim.UserName, loginModel.Username),
                        new Claim(LoginUserClaim.Password, loginModel.Password),
                        new Claim(LoginUserClaim.AccessToken, _desObject.access_token),
                        new Claim(LoginUserClaim.RefreshToken, _desObject.refresh_token),
                        new Claim(ClaimTypes.Name, displayName),
                        new Claim(ClaimTypes.Role, rolename),
                        new Claim(LoginUserClaim.DasboardName, CommonFunctionUI.DashboardName(rolename)),
                         new Claim(LoginUserClaim.UserProfile, profilelink),
                        new Claim(LoginUserClaim.UserId, userId)


                    };
                TempData["DasboardUrl"] = CommonFunctionUI.DashboardUrl(rolename);
                var identity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var userPrincipal = new ClaimsPrincipal(new[] { identity });
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
                return Result.Success;
            }
            else
            {
                var _dataResponse = await _response.Content.ReadAsStringAsync();
                ResponseMessage _desObject = JsonConvert.DeserializeObject<ResponseMessage>(_dataResponse);
                if (_desObject.StatusMessage == "email is not verified")
                {
                    string sessionKey_EmailNotverified = "_Email";
                    HttpContext.Session.SetString(sessionKey_EmailNotverified, loginModel.Username);
                }
                return Result.Failure(CommonFunctionUI.ErrorResponseToErrorResult(_desObject));
            }
        }

        public LoginModel CheckCookies()
        {
            LoginModel _user = null;
            string UserName = string.Empty;
            string Password = string.Empty;
            if (Request.Cookies["UserName"] != null)
            {
                UserName = GetCookie("UserName");
            }
            if (Request.Cookies["Password"] != null)
            {
                Password = GetCookie("Password");
            }
            if (!String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(Password))
            {
                _user = new LoginModel();
                _user.Username = UserName;
                _user.Password = Password;
            }
            return _user;
        }
        public void SetCookie(string key, string value, DateTime expireTime)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = expireTime;

            Response.Cookies.Append(key, value, option);
        }
        public void RemoveCookie(string key)
        {
            Response.Cookies.Delete(key);
        }
        public string GetCookie(string key)
        {
            return Request.Cookies[key];
        }

    }
}
