using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Common.Models
{
  public static class GuardExtensions
  {
    /// <summary>
    /// Checks if the argument is null.
    /// </summary>
    public static void CheckArgumentIsNull(this object o, string name)
    {
      if (o == null)
        throw new ArgumentNullException(name);
    }
  }
}
