using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace _Core.DataAccess.EntityFramework
{
    public class EfEntityRepository<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, new()
        where TContext : DbContext, new()
    {
        protected readonly TContext Db = new TContext();

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return Db.Set<TEntity>().FirstOrDefault(filter);
        }

        public bool Control(Expression<Func<TEntity, bool>> kontrol)
        {
            return Db.Set<TEntity>().Any(kontrol);
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
        {
            if (filter == null)
            {
                return Db.Set<TEntity>().ToList();
            }
            return Db.Set<TEntity>().Where(filter).ToList();
        }

        public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return Db.Set<TEntity>().SingleOrDefaultAsync(filter);
        }

        public Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter != null ? Db.Set<TEntity>().Where(filter).ToListAsync() : Db.Set<TEntity>().ToListAsync();
        }

        public TEntity Add(TEntity entity)
        {
            var added = Db.Entry(entity);
            added.State = EntityState.Added;
            return entity;
        }

        public void Delete(TEntity entity)
        {
            var deleted = Db.Entry(entity);
            deleted.State = EntityState.Deleted;
        }

        public void Update(TEntity entity)
        {
            var updated = Db.Entry(entity);
            updated.State = EntityState.Modified;
        }

        public int Save() => Db.SaveChanges();

        public TEntity AddWithSave(TEntity entity)
        {
            var added = Add(entity);
            Save();
            return added;
        }

        public void UpdateWithSave(TEntity entity)
        {
            Update(entity);
            Save();
        }

        public void UpdateMultiple(List<TEntity> entities)
        {
            Db.UpdateRange(entities);
        }

        public void DeleteWithSave(TEntity entity)
        {
            Delete(entity);
            Save();
        }
    }
}
