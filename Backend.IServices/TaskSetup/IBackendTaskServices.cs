using Backend.Common.Helpers;
using Backend.Common.Models;
using Backend.Domain.BackendSetup;
using Backend.Domain.UserSetup;
using Backend.Services.RepositoryConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.TaskSetup
{
  public interface IBackendTaskServices:IRepositoryBaseService<BackendTask>
  {
    PagedList<BackendTask> GetPaginationList(PaginationModel paginationModel);

    BackendTask GetDetailsById(long id);
    List<BackendTask> GetAlDetails();

  }
}
