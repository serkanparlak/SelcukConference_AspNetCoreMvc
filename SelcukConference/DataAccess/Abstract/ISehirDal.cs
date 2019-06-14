using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataAccess.Entities;
using _Core.DataAccess;

namespace DataAccess.Abstract
{
    public interface ISehirDal : IEntityRepository<Sehir>
    {
        Sehir Get_WInc(Expression<Func<Sehir, bool>> filter);

        List<Sehir> GetList_WInc(Expression<Func<Sehir, bool>> filter = null);
    }
}