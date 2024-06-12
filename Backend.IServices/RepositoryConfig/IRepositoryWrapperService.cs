using Backend.Services.TaskSetup;
using Backend.Services.UserSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.RepositoryConfig
{
  public interface IRepositoryWrapperService
  {
    void Save();
    IUsersRepoService UsersRepoService { get; }
    IBackendTaskServices TaskServices { get; }
    
}
}
