using AutoMapper;
using Backend.Common.Models;
using Backend.Common.OptionsModel;
using Backend.Dto.UsersSetup;
using Backend.Entity.DatabaseContext;
using Backend.Services.JwtServices;
using Backend.Services.RepositoryConfig;
using Backend.Services.UserSetup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Backend.WebHost.Controllers.UsersSetup
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccountController : ControllerBase
  {
    private readonly IUsersService _usersManager;
    private readonly ITokenStoreService _tokenStoreManager;
    private readonly IUnitOfWork _uow;
    private readonly IAntiForgeryCookieService _antiforgery;
    private readonly ITokenFactoryService _tokenFactoryManager;
    
    private readonly IRepositoryWrapperService _manager;
    private readonly ISecurityService _managerSecurity;
    private readonly IMapper _mapper;
   
    private readonly IOptionsSnapshot<ApiSettings> _configuration;

    private readonly IWebHostEnvironment _hostingEnvironment;


    public AccountController(
        IUsersService usersService,
        ITokenStoreService tokenStoreService,
        ITokenFactoryService tokenFactoryService,
        IUnitOfWork uow,
        IAntiForgeryCookieService antiforgery,
        IRepositoryWrapperService manager,
        ISecurityService securityManager,
        IMapper mapper,
        
        IWebHostEnvironment hostingEnvironment,
        IOptionsSnapshot<ApiSettings> configuration
        )
    {
      _manager = manager;
      _manager.CheckArgumentIsNull(nameof(manager));
      _usersManager = usersService;
      _usersManager.CheckArgumentIsNull(nameof(usersService));

      _tokenStoreManager = tokenStoreService;
      _tokenStoreManager.CheckArgumentIsNull(nameof(tokenStoreService));

      _uow = uow;
      _uow.CheckArgumentIsNull(nameof(_uow));

      _antiforgery = antiforgery;
      _antiforgery.CheckArgumentIsNull(nameof(antiforgery));

      _managerSecurity = securityManager;
      _managerSecurity.CheckArgumentIsNull(nameof(securityManager));

      _tokenFactoryManager = tokenFactoryService;
      _tokenFactoryManager.CheckArgumentIsNull(nameof(tokenFactoryService));

      _mapper = mapper;
      _manager.CheckArgumentIsNull(nameof(mapper));

      

      _configuration = configuration;
      _configuration.CheckArgumentIsNull(nameof(configuration));

      _hostingEnvironment = hostingEnvironment;
      if (string.IsNullOrWhiteSpace(_hostingEnvironment.WebRootPath))
      {
        _hostingEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
      }
    }
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginUser)
    {
      

      ResponseMessage responseMessage = new ResponseMessage();
      if (loginUser.Username == null && loginUser.Password == null)
      {
        responseMessage.StatusCode = 400;
        responseMessage.StatusTitle = "BadRequest";
        responseMessage.StatusMessage = "Username and password is missing";
      
        return BadRequest(responseMessage);
      }
     
      var user = await _usersManager.FindUserAsync(loginUser.Username, loginUser.Password);
      
      if (user?.IsActive != true)
      {
        responseMessage.StatusCode = 401;
        responseMessage.StatusTitle = "Unauthorized";
        responseMessage.StatusMessage = "Username and password is wrong";
       
        return Unauthorized(responseMessage);

      }
      if (user?.EmailConfirmed != true)
      {
        responseMessage.StatusCode = 403;
        responseMessage.StatusTitle = "Unauthorized";
        responseMessage.StatusMessage = "email is not verified";
      
        return Unauthorized(responseMessage);

      }
    
      var result = await _tokenFactoryManager.CreateJwtTokensAsync(user);
     
      //await _tokenStoreManager.AddUserTokenAsync(user, result.RefreshTokenSerial, result.AccessToken, null);
    
     // await _uow.SaveChangesAsync();
      
      _antiforgery.RegenerateAntiForgeryCookies(result.Claims);
     

      LoginResponseDto responseDto = new LoginResponseDto();
      responseDto.access_token = result.AccessToken;
      responseDto.refresh_token = result.RefreshToken;
     
      
      //var _currentLogin_Details = CommonFunction.GetCurrentLogin_ClaimsDetails(User.Claims.ToList());
     
      return Ok(responseDto);


    }

    

   

    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    [HttpPost("[action]")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenDto model)
    {
      var refreshTokenValue = model.RefreshToken;
      if (string.IsNullOrWhiteSpace(refreshTokenValue))
      {
        return BadRequest("refreshToken is not set.");
      }

      var token = await _tokenStoreManager.FindTokenAsync(refreshTokenValue);
      if (token == null)
      {
        return Unauthorized();
      }

      var result = await _tokenFactoryManager.CreateJwtTokensAsync(token.User);
      await _tokenStoreManager.AddUserTokenAsync(token.User, result.RefreshTokenSerial, result.AccessToken, _tokenFactoryManager.GetRefreshTokenSerial(refreshTokenValue));
      await _uow.SaveChangesAsync();

      _antiforgery.RegenerateAntiForgeryCookies(result.Claims);

      return Ok(new { access_token = result.AccessToken, refresh_token = result.RefreshToken });
    }
    [AllowAnonymous]
    [HttpGet("[action]")]
    public async Task<bool> Logout(string refreshToken)
    {
      var claimsIdentity = this.User.Identity as ClaimsIdentity;
      var userIdValue = claimsIdentity.FindFirst(ClaimTypes.UserData)?.Value;

      // The Jwt implementation does not support "revoke OAuth token" (logout) by design.
      // Delete the user's tokens from the database (revoke its bearer token)
      await _tokenStoreManager.RevokeUserBearerTokensAsync(userIdValue, refreshToken);
      await _uow.SaveChangesAsync();

      _antiforgery.DeleteAntiForgeryCookies();
      //var _currentLogin_Details = CommonFunction.GetCurrentLogin_ClaimsDetails(User.Claims.ToList());
     
      return true;
    }

    
    [HttpGet("[action]"), HttpPost("[action]")]
    public bool IsAuthenticated()
    {
      return User.Identity.IsAuthenticated;
    }

    [HttpGet("[action]"), HttpPost("[action]")]
    public IActionResult GetUserInfo()
    {
      var claimsIdentity = User.Identity as ClaimsIdentity;
      return Ok(new { Username = claimsIdentity.Name });
    }

   
    [IgnoreAntiforgeryToken]
    [HttpGet("[action]")]
    public IActionResult ApiStatus()
    {
      return Ok("Api is Starting");
    }
  }
}
