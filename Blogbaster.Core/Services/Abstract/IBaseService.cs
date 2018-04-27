using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blogbaster.Core.Data.Entities.Abstract;

namespace Blogbaster.Core.Services.Abstract
{
    public interface IBaseService<TEntity> : IDisposable where TEntity : IEntity
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        TEntity FindById(object id);
        Task<TEntity> Add(TEntity entity);
        void Update(TEntity entity);
        Task<TEntity> Delete(TEntity entity);
        Task<TEntity> DeleteById(object id);
    }
}