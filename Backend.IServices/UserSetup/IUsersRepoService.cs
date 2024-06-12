using Backend.Common.Helpers;
using Backend.Common.Models;
using Backend.Domain.UserSetup;
using Backend.Services.RepositoryConfig;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.UserSetup
{
  public interface IUsersRepoService : IRepositoryBaseService<User>
  {
    PagedList<User> GetAdminPaginationList(PaginationModel paginationModel);

    User GetDetailsById(long id);

    
   
    User GetDetailsById(long id, string email);
    User GetDetailsByEmail(string email);

    void ChangePassword(User model);

    bool IsEmail(string email);




  }
}
