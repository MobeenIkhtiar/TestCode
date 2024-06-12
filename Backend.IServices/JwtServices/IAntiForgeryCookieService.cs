using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.JwtServices
{
  public interface IAntiForgeryCookieService
  {
    void RegenerateAntiForgeryCookies(IEnumerable<Claim> claims);
    void DeleteAntiForgeryCookies();
  }
}
