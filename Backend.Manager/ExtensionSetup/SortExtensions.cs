using Backend.Domain.BackendSetup;
using Backend.Domain.UserSetup;

using System.Reflection;
using System.Text;


namespace Backend.Manager.ExtensionSetup
{
  public static class SortExtensions
  {
    
    public static IQueryable<User> Sort_User(this IQueryable<User> obj, string OrderBy, string OrderDir)
    {
      if (string.IsNullOrWhiteSpace(OrderBy) || string.IsNullOrWhiteSpace(OrderDir))
      {
        return obj;
      }
      OrderDir = OrderDir.ToLower();
      switch (OrderBy)
      {
        case "Id":
          switch (OrderDir)
          {
            case "asc":
              obj = obj.OrderBy(x => x.Id);
              break;
            case "desc":
              obj = obj.OrderByDescending(x => x.Id);
              break;
            default:
              break;
          }
          break;

        case "FirstName":
          switch (OrderDir)
          {
            case "asc":
              obj = obj.OrderBy(x => x.FirstName);
              break;
            case "desc":
              obj = obj.OrderByDescending(x => x.FirstName);
              break;
            default:
              break;
          }
          break;
        default:
          obj = obj.OrderBy(x => x.Id);
          break;
      }

      return obj;
    }




    public static IQueryable<BackendTask> Sort_Backend(this IQueryable<BackendTask> obj, string OrderBy, string OrderDir)
    {
      if (string.IsNullOrWhiteSpace(OrderBy) || string.IsNullOrWhiteSpace(OrderDir))
      {
        return obj;
      }
      OrderDir = OrderDir.ToLower();
      switch (OrderBy)
      {
        case "Id":
          switch (OrderDir)
          {
            case "asc":
              obj = obj.OrderBy(x => x.Id);
              break;
            case "desc":
              obj = obj.OrderByDescending(x => x.Id);
              break;
            default:
              break;
          }
          break;

        case "Name":
          switch (OrderDir)
          {
            case "asc":
              obj = obj.OrderBy(x => x.Name);
              break;
            case "desc":
              obj = obj.OrderByDescending(x => x.Name);
              break;
            default:
              break;
          }
          break;
        default:
          obj = obj.OrderBy(x => x.Id);
          break;
      }

      return obj;
    }






  }
}
