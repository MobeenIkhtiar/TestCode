using Backend.Entity.DatabaseContext;
using Backend.Manager.TaskSetup;
using Backend.Manager.UserSetup;
using Backend.Services.RepositoryConfig;
using Backend.Services.TaskSetup;
using Backend.Services.UserSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Manager.RepositoryConfig
{
  public class RepositoryWrapperManager : IRepositoryWrapperService
  {
    private readonly ApplicationDbContext _apiDbContext;
    private IUsersRepoService _iUsersRepoService;
    private IBackendTaskServices _iTaskServices;


    public IUsersRepoService UsersRepoService
    {
      get
      {
        if (_iUsersRepoService == null)
        {
          _iUsersRepoService = new UsersRepoManager(_apiDbContext);
        }

        return _iUsersRepoService;
      }
    }

    public IBackendTaskServices TaskServices
    {
      get
      {
        if (_iTaskServices == null)
        {
          _iTaskServices = new BackenTaskManager(_apiDbContext);
        }

        return _iTaskServices;
      }
    }
    public RepositoryWrapperManager(ApplicationDbContext apiDbContext)
    {
      _apiDbContext = apiDbContext;

    }

    public void Save()
    {
      _apiDbContext.SaveChanges();
    }
  }
}
