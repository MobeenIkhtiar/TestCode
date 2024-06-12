using System.ComponentModel.DataAnnotations.Schema;

namespace Frontend.Web.Models.UsersSetup
{
  public class LoginModel
  {
    public string Username { get; set; }

    public string Password { get; set; }

    public bool RememberMe { get; set; }

  }
}
