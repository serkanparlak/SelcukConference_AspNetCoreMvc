using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataAccess.Entities;
using _Core.DataAccess;

namespace DataAccess.Abstract
{
    public interface IUyeDal : IEntityRepository<Uye>
    {
        Uye Get_WInc(Expression<Func<Uye, bool>> filter);
        List<Uye> GetList_WInc(Expression<Func<Uye, bool>> filter = null);
    }
}