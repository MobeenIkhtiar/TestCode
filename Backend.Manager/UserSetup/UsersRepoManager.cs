using Backend.Common.CommonFunctions;
using Backend.Common.Helpers;
using Backend.Common.Models;
using Backend.Domain.UserSetup;
using Backend.Entity.DatabaseContext;
using Backend.Manager.ExtensionSetup;
using Backend.Manager.RepositoryConfig;
using Backend.Services.UserSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static Backend.Common.CommonFunctions.CommonEnums;

namespace Backend.Manager.UserSetup
{
  public class UsersRepoManager : RepositoryBaseManager<User>, IUsersRepoService
  {
    private ApplicationDbContext _apiDbContext;

    public UsersRepoManager(ApplicationDbContext apiDbContext) : base(apiDbContext)
    {
      _apiDbContext = apiDbContext;

    }

    //public NewSerialNumber NewSerialNumber()
    //{
    //    string SrNo = "";
    //    var list = (from max in _ApiDbContext.Customer
    //                where max.SrNo != null
    //                select max.SrNo).ToList();
    //    long maxvalue = 0;
    //    if (list.Count() != 0)
    //        maxvalue = list.Select(long.Parse).ToList().Max();
    //    if (maxvalue == 0)
    //    {
    //        SrNo = "1";
    //    }
    //    else
    //    {
    //        long newSrNo = maxvalue + 1;
    //        SrNo = newSrNo.ToString();
    //    }
    //    NewSerialNumber Model = new NewSerialNumber
    //    {
    //        SrNo = SrNo,
    //    };
    //    return Model;
    //}
    public User GetDetailsById(long id)
    {
      return _apiDbContext.Users.Where(a => a.Id == id).FirstOrDefault();
    }
   
    public User GetDetailsById(long id, string email)
    {
      return _apiDbContext.Users.Where(a => a.Id == id && a.Email == email).FirstOrDefault();
    }
    public User GetDetailsByEmail(string email)
    {
      return _apiDbContext.Users.Where(a => a.Email == email).FirstOrDefault();
    }
    public PagedList<User> GetAdminPaginationList(PaginationModel paginationModel)
    {
      IQueryable<User> obj;

      if (paginationModel.StartDate != null && paginationModel.EndDate != null)
      {
        obj = _apiDbContext.Users.Where(a => a.IsDeleted == false && a.CreatedAt.Value.Date >= paginationModel.StartDate.Value.Date && a.CreatedAt.Value.Date <= paginationModel.EndDate.Value).Where(a => a.IsActive == true).Search_User(paginationModel.SearchTerm).Sort_User(paginationModel.OrderBy, paginationModel.OrderDir).Where(a => a.CreatedAt >= paginationModel.StartDate && a.CreatedAt <= paginationModel.EndDate);
      }
      else
      {
        obj = _apiDbContext.Users.Where(a => a.IsDeleted == false && a.IsActive==true).Search_User(paginationModel.SearchTerm).Sort_User(paginationModel.OrderBy, paginationModel.OrderDir);
      }


      return PagedList<User>.ToPagedList(obj, paginationModel.PageNumber, paginationModel.PageSize);

    }
   
    public void ChangePassword(User model)
    {
      var findObj = _apiDbContext.Users.Where(a => a.Id == model.Id).FirstOrDefault();
      findObj.PasswordHash = model.PasswordHash;
      _apiDbContext.Entry(findObj).State = EntityState.Modified;
      _apiDbContext.SaveChanges();
    }
    public bool IsEmail(string email)
    {
      bool isExsit = false;
      var findObj = _apiDbContext.Users.Where(a => a.Email.Contains(email.ToLower().ToString()) && a.IsDeleted == false).ToList().Count();
      if (findObj > 0)
      {
        isExsit = true;
      }
      return isExsit;
    }
   
   

    
  }
}
