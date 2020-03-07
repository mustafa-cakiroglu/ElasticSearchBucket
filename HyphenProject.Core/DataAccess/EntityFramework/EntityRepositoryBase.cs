using Microsoft.EntityFrameworkCore;
using HyphenProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HyphenProject.Core.DataAccess.EntityFramework
{
    public class EntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
         where TEntity : class, IEntity, new()
         where TContext : DbContext, new()
    {
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return filter == null
    ? context.Set<TEntity>().ToList()
    : context.Set<TEntity>().Where(filter).ToList();
            }
        }
        public TEntity Add(TEntity entity)
        {
            using (var context = new TContext())
            {
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
                return entity;
            }
        }

        public TEntity Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}