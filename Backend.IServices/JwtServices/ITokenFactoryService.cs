using Backend.Common.JwtModel;
using Backend.Domain.UserSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.JwtServices
{
  public interface ITokenFactoryService
  {
    Task<JwtTokensData> CreateJwtTokensAsync(User user);
    string GetRefreshTokenSerial(string refreshTokenValue);
  }
}
