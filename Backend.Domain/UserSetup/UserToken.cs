using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.UserSetup
{
  public class UserToken
  {
    [Key]
    public long Id { get; set; }

    public string AccessTokenHash { get; set; }

    public DateTimeOffset AccessTokenExpiresDateTime { get; set; }

    public string RefreshTokenIdHash { get; set; }

    public string RefreshTokenIdHashSource { get; set; }

    public DateTimeOffset RefreshTokenExpiresDateTime { get; set; }

    public long UserId { get; set; }
    public virtual User User { get; set; }
  }
}
