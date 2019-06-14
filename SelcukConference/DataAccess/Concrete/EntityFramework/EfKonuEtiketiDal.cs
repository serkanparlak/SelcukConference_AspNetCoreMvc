using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.Abstract;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using _Core.DataAccess.EntityFramework;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfKonuEtiketiDal : EfEntityRepository<KonuEtiketi, DbtechconfContext>, IKonuEtiketiDal
    {
        public KonuEtiketi GetWIncById(int id)
        {
            return Db.KonuEtiketi.Include(x => x.Editor)
                .ThenInclude(x => x.KonuEtiketi).SingleOrDefault(x => x.Id == id);
        }

        public List<KonuEtiketi> GetListWInc(Expression<Func<KonuEtiketi, bool>> filter)
        {
            return filter == null
                ? Db.KonuEtiketi.Include(x => x.Editor)
                    .ThenInclude(x => x.KonuEtiketi).OrderByDescending(x=>x.Id).ToList()
                : Db.KonuEtiketi.Include(x => x.Editor)
                    .ThenInclude(x => x.KonuEtiketi)
                    .Where(filter).OrderByDescending(x => x.Id).ToList();
        }
    }
}