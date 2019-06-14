using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace _Core.DataAccess
{
    public interface IEntityRepository<TEntity> where TEntity : class, new()
    {
        TEntity Get(Expression<Func<TEntity,bool>> filter);

        bool Control(Expression<Func<TEntity, bool>> kontrol);

        List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter);

        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null);

        TEntity Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        int Save();

        TEntity AddWithSave(TEntity entity);

        void UpdateWithSave(TEntity entity);

        void UpdateMultiple(List<TEntity> entities);

        void DeleteWithSave(TEntity entity);
        
    }
}
