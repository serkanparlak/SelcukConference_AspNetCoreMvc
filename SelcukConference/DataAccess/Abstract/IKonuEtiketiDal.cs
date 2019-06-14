using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataAccess.Entities;
using _Core.DataAccess;

namespace DataAccess.Abstract
{
    public interface IKonuEtiketiDal : IEntityRepository<KonuEtiketi>
    {
        KonuEtiketi GetWIncById(int id);

        List<KonuEtiketi> GetListWInc(Expression<Func<KonuEtiketi, bool>> filter = null);
    }
}