using Backend.Domain.UserSetup;

namespace Backend.Services.UserSetup
{
  public interface IUsersService
  {
    Task<string> GetSerialNumberAsync(long userId);
    Task<User> FindUserAsync(string username, string password);
    ValueTask<User> FindUserAsync(long userId);
    Task UpdateUserLastActivityDateAsync(long userId);
    ValueTask<User> GetCurrentUserAsync();
    long GetCurrentUserId();
    Task<(bool Succeeded, string Error)> ChangePasswordAsync(User user, string currentPassword, string newPassword);
  }
}
