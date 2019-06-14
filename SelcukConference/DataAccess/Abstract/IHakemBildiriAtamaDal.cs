using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataAccess.Entities;
using _Core.DataAccess;

namespace DataAccess.Abstract
{
    public interface IHakemBildiriAtamaDal : IEntityRepository<HakemBildiriAtama>
    {
        HakemBildiriAtama Get_WInc(Expression<Func<HakemBildiriAtama, bool>> filter);

        List<HakemBildiriAtama> GetList_WInc(Expression<Func<HakemBildiriAtama, bool>> filter = null);

        void TopluSil(List<HakemBildiriAtama> silHakemBildiriList);
    }
}