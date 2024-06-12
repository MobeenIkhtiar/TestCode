using Backend.Domain.BackendSetup;
using Backend.Domain.UserSetup;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Manager.ExtensionSetup
{
  public static class SearchExtensions
  {
    public static IQueryable<User> Search_User(this IQueryable<User> obj, string searchTearm)
    {
      if (string.IsNullOrWhiteSpace(searchTearm))

        return obj;
      var lowerCaseSearchTerm = searchTearm.Trim().ToLower();

      obj = obj.Where(p => p.Email.ToLower().Contains(lowerCaseSearchTerm)
      || p.FirstName.ToLower().Contains(lowerCaseSearchTerm)
      || p.LastName.ToLower().Contains(lowerCaseSearchTerm)
      || p.CreatedAt.Value.ToString().ToLower().Contains(lowerCaseSearchTerm)
      );

      return obj;
    }


    public static IQueryable<BackendTask> Search_Backend(this IQueryable<BackendTask> obj, string searchTearm)
    {
      if (string.IsNullOrWhiteSpace(searchTearm))

        return obj;
      var lowerCaseSearchTerm = searchTearm.Trim().ToLower();

      obj = obj.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm)
      || p.CreatedAt.Value.ToString().ToLower().Contains(lowerCaseSearchTerm)
      );

      return obj;
    }


  }
}
