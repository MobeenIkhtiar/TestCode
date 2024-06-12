using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Web.Models.CommonModels
{
  public class ApplicationSettings
  {
    public const string SectionKey = "Application";
    public string Name { get; set; }
    public string APIBaseUrl { get; set; }
    public string UIBaseUrl { get; set; }
    public int ClaimExpires { get; set; }
    public int CookieExpires { get; set; }
    public int NotifyExpireMinutes { get; set; }
    public int SessionExpireNotificationMinutes { get; set; }
    public int IntervalNotifiyMinutes { get; set; }
  }
}
