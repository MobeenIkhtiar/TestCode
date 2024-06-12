using Backend.Common.Models;
using Backend.Domain.UserSetup;
using Backend.Entity.DatabaseContext;
using Backend.Services.JwtServices;
using Backend.Services.UserSetup;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Manager.UserSetup
{
  public class UsersManager : IUsersService
  {
    private readonly IUnitOfWork _uow;
    private readonly DbSet<User> _users;
    private readonly ISecurityService _securityService;
    private readonly IHttpContextAccessor _contextAccessor;

    public UsersManager(
        IUnitOfWork uow,
        ISecurityService securityService,
        IHttpContextAccessor contextAccessor)
    {
      _uow = uow;
      _uow.CheckArgumentIsNull(nameof(_uow));

      _users = _uow.Set<User>();

      _securityService = securityService;
      _securityService.CheckArgumentIsNull(nameof(_securityService));

      _contextAccessor = contextAccessor;
      _contextAccessor.CheckArgumentIsNull(nameof(_contextAccessor));
    }

    public ValueTask<User> FindUserAsync(long userId)
    {
      return _users.FindAsync(userId);
    }

    public async Task<User> FindUserAsync(string username, string password)
    {
      var passwordHash = _securityService.GetSha256Hash(password);
      var user =  await _users.FirstOrDefaultAsync(x => x.Username == username && x.PasswordHash == passwordHash);
      return user;    }

    public async Task<string> GetSerialNumberAsync(long userId)
    {
      var user = await FindUserAsync(userId);
      return user.SerialNumber;
    }

    public async Task UpdateUserLastActivityDateAsync(long userId)
    {
      var user = await FindUserAsync(userId);
      if (user.LastLoggedIn != null)
      {
        var updateLastActivityDate = TimeSpan.FromMinutes(2);
        var currentUtc = DateTime.UtcNow;
        var timeElapsed = currentUtc.Subtract(user.LastLoggedIn.Value);
        if (timeElapsed < updateLastActivityDate)
        {
          return;
        }
      }
      user.LastLoggedIn = DateTime.UtcNow;
      await _uow.SaveChangesAsync();
    }

    public long GetCurrentUserId()
    {
      var claimsIdentity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
      var userDataClaim = claimsIdentity?.FindFirst(ClaimTypes.UserData);
      var userId = userDataClaim?.Value;
      if (userId != null)
      {
        var Id= Convert.ToInt32(userId);
        return Id;
      }
      return 0;
    }

    public ValueTask<User> GetCurrentUserAsync()
    {
      var userId = GetCurrentUserId();
      return FindUserAsync(userId);
    }

    public async Task<(bool Succeeded, string Error)> ChangePasswordAsync(User user, string currentPassword, string newPassword)
    {
      var currentPasswordHash = _securityService.GetSha256Hash(currentPassword);
      if (user.PasswordHash != currentPasswordHash)
      {
        return (false, "Current password is wrong.");
      }

      user.PasswordHash = _securityService.GetSha256Hash(newPassword);
      // user.SerialNumber = Guid.NewGuid().ToString("N"); // To force other logins to expire.
      await _uow.SaveChangesAsync();
      return (true, string.Empty);
    }
  }
}
