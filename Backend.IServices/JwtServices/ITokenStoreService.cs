using Backend.Domain.UserSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.JwtServices
{
  public interface ITokenStoreService
  {
    Task AddUserTokenAsync(UserToken userToken);
    Task AddUserTokenAsync(User user, string refreshTokenSerial, string accessToken, string refreshTokenSourceSerial);
    Task<bool> IsValidTokenAsync(string accessToken, long userId);
    Task DeleteExpiredTokensAsync();
    Task<UserToken> FindTokenAsync(string refreshTokenValue);
    Task DeleteTokenAsync(string refreshTokenValue);
    Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdHashSource);
    Task InvalidateUserTokensAsync(long userId);
    Task RevokeUserBearerTokensAsync(string userIdValue, string refreshTokenValue);
  }
}
