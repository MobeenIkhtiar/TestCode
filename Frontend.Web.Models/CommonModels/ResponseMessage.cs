using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Web.Models.CommonModels
{
  public class ResponseMessage
  {
    public int StatusCode { get; set; }
    public string StatusTitle { get; set; }
    public string StatusMessage { get; set; }
    public string Message { get; set; }
    public long Id { get; set; }

  }
}
