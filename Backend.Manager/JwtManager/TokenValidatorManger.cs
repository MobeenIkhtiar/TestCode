using Backend.Common.Models;
using Backend.Services.JwtServices;
using Backend.Services.UserSetup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Manager.JwtManager
{
  public class TokenValidatorManger : ITokenValidatorService
  {
    private readonly IUsersService _usersManager;
    private readonly ITokenStoreService _tokenStoreManager;

    public TokenValidatorManger(IUsersService usersService, ITokenStoreService tokenStoreService)
    {
      _usersManager = usersService;
      _usersManager.CheckArgumentIsNull(nameof(usersService));

      _tokenStoreManager = tokenStoreService;
      _tokenStoreManager.CheckArgumentIsNull(nameof(_tokenStoreManager));
    }

    public async Task ValidateAsync(TokenValidatedContext context)
    {
      var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
      if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
      {
        context.Fail("This is not our issued token. It has no claims.");
        return;
      }

      var serialNumberClaim = claimsIdentity.FindFirst(ClaimTypes.SerialNumber);
      if (serialNumberClaim == null)
      {
        context.Fail("This is not our issued token. It has no serial.");
        return;
      }

      var userIdString = claimsIdentity.FindFirst(ClaimTypes.UserData).Value;
      long userId = 0;
      if (userIdString != null)
      {
        userId = Convert.ToInt64(userIdString);
      }
      if (string.IsNullOrEmpty(userIdString))
      {
        context.Fail("This is not our issued token. It has no user-id.");
        return;
      }

      var user = await _usersManager.FindUserAsync(userId);
      if (user == null || user.SerialNumber != serialNumberClaim.Value || !user.IsActive)
      {
        // user has changed his/her password/roles/stat/IsActive
        context.Fail("This token is expired. Please login again.");
      }

      if (!(context.SecurityToken is JwtSecurityToken accessToken) || string.IsNullOrWhiteSpace(accessToken.RawData) ||
          !await _tokenStoreManager.IsValidTokenAsync(accessToken.RawData, userId))
      {
        context.Fail("This token is not in our database.");
        return;
      }

      await _usersManager.UpdateUserLastActivityDateAsync(userId);
    }

  }
}

