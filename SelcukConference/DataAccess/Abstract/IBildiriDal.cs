using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataAccess.Entities;
using _Core.DataAccess;

namespace DataAccess.Abstract
{
    public interface IBildiriDal : IEntityRepository<Bildiri>
    {
        Bildiri Get_WInc(Expression<Func<Bildiri, bool>> filter);

        List<Bildiri> GetList_WInc(Expression<Func<Bildiri, bool>> filter = null);
    }
}