using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Web.Models.CommonModels
{
  public static class CommonConstantsModel
  {
    public const int Width = 64;
    public const int Height = 64;
    public const int ProfileWidth = 128;
    public const int ProfileHeight = 128;
  }
  public static class AccountPath
  {
    public const string Login = "/api/Account/Login";
    public const string Logout = "/api/Account/Logout";

  }
    public static class BackendPath
    {
        public const string GetAllTask = "/api/Task/GetAllTask";
        public const string PostTask = "/api/Task/Post";
        public const string PutTask = "/api/Task/Put";
        public const string Delete = "/api/Task/Delete?Id=";
        public const string GetById = "/api/Task/GetById?Id=";


    }
}
