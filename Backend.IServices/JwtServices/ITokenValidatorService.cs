using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.JwtServices
{
  public interface ITokenValidatorService
  {
    Task ValidateAsync(TokenValidatedContext context);
  }
}
