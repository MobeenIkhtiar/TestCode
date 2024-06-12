using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Common.JwtModel
{
  public class JwtTokensData
  {
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string RefreshTokenSerial { get; set; }
    public IEnumerable<Claim> Claims { get; set; }
  }
}
