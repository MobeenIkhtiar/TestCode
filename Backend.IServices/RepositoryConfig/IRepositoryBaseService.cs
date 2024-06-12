using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services.RepositoryConfig
{
  public interface IRepositoryBaseService<T>
  {
    IQueryable<T> GetAll(Expression<Func<T, object>> expression);
    IQueryable<T> GetAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    void DeleteRange(List<T> entity);
    void UpdateRange(List<T> entity);
    void CreateRange(List<T> entity);

    T CreateEntity(T entity);
    T UpdateEntity(T entity);

  }
}
