using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Web.Models.CommonModels
{
  public class ClaimsInfoModel
  {
    #region Region_Common
    public string UserName { get; set; }
    public string Password { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public long UserId { get; set; }
   
    public string DasboardName { get; set; }
    public string DisplayName { get; set; }
    
    public string UserProfile { get; set; }
   
    
    #endregion

    
   
  }
}
