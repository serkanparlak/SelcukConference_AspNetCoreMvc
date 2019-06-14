using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataAccess.Entities;
using _Core.DataAccess;

namespace DataAccess.Abstract
{
    public interface IUlkeDal : IEntityRepository<Ulke>
    {
        Ulke Get_WInc(Expression<Func<Ulke, bool>> filter);

        List<Ulke> GetList_WInc(Expression<Func<Ulke, bool>> filter = null);
        // Custom operations for Ulke table
    }
}