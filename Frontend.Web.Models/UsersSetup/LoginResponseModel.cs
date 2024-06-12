using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Web.Models.UsersSetup
{
  public class LoginResponseModel
  {
    public string access_token { get; set; }
    public string refresh_token { get; set; }
  }
}
