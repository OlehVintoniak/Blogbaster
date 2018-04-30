﻿using Blogbaster.Core.Data.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blogbaster.Core.Services.Abstract
{
    public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, IEntity
    {
        private readonly DbSet<TEntity> _dbSet;
        protected DbSet<TEntity> DbSet
        {
            get { return _dbSet; }
        }
        protected readonly ApplicationDbContext Context;
        protected BaseService(ApplicationDbContext сontext)
        {
            Context = сontext;
            if (Context != null)
            {
                _dbSet = Context.Set<TEntity>();
            }
        }

        protected BaseService(ApplicationDbContext context, DbSet<TEntity> dbSet)
        {
            Context = context;
            _dbSet = dbSet;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return DbSet.AsEnumerable();
        }

        public IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).AsEnumerable();
        }

        public virtual TEntity FindById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual async Task<TEntity> Add(TEntity entity)
        {
            var addedEntity = DbSet.Add(entity);
            await Context.SaveChangesAsync();
            return addedEntity;
        }

        public virtual void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual async Task<TEntity> Delete(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            var deletedEntity = DbSet.Remove(entity);
            await Context.SaveChangesAsync();
            return deletedEntity;
        }

        public virtual async Task<TEntity> DeleteById(object id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            return await Delete(entityToDelete);
        }

        #region IDisposable

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        #endregion
    }
}