using Backend.Common.Helpers;
using Backend.Common.Models;
using Backend.Domain.BackendSetup;
using Backend.Domain.UserSetup;
using Backend.Entity.DatabaseContext;
using Backend.Manager.ExtensionSetup;
using Backend.Manager.RepositoryConfig;
using Backend.Services.TaskSetup;
using Backend.Services.UserSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Manager.TaskSetup
{
  public  class BackenTaskManager: RepositoryBaseManager<BackendTask>,IBackendTaskServices
  {
    private ApplicationDbContext _apiDbContext;

    public BackenTaskManager(ApplicationDbContext apiDbContext) : base(apiDbContext)
    {
      _apiDbContext = apiDbContext;

    }
    public BackendTask GetDetailsById(long id)
    {
      return _apiDbContext.BackendTasks.Where(a => a.Id == id).FirstOrDefault();
    }
   public  PagedList<BackendTask> GetPaginationList(PaginationModel paginationModel)
    {
      IQueryable<BackendTask> obj;

      if (paginationModel.StartDate != null && paginationModel.EndDate != null)
      {
        obj = _apiDbContext.BackendTasks.Where(a => a.IsDeleted == false && a.CreatedAt.Value.Date >= paginationModel.StartDate.Value.Date && a.CreatedAt.Value.Date <= paginationModel.EndDate.Value).Where(a => a.IsActive == true).Search_Backend(paginationModel.SearchTerm).Sort_Backend(paginationModel.OrderBy, paginationModel.OrderDir).Where(a => a.CreatedAt >= paginationModel.StartDate && a.CreatedAt <= paginationModel.EndDate);
      }
      else
      {
        obj = _apiDbContext.BackendTasks.Where(a => a.IsDeleted == false && a.IsActive == true).Search_Backend(paginationModel.SearchTerm).Sort_Backend(paginationModel.OrderBy, paginationModel.OrderDir);
      }


      return PagedList<BackendTask>.ToPagedList(obj, paginationModel.PageNumber, paginationModel.PageSize);
    }
   public  List<BackendTask> GetAlDetails()
    {
      return _apiDbContext.BackendTasks.Where(a => a.Id >=1).ToList();

    }


  }
}
