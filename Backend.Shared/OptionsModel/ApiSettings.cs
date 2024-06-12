using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Common.OptionsModel
{
  public class ApiSettings
  {
    public string FilesUploadPath { get; set; }
    public string UIAppLink { get; set; }
    public string APIAppLink { get; set; }
    public int OTPExpiryTime { get; set; }

  }
}
