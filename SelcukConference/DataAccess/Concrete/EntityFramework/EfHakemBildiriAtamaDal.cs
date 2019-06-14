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
    public class EfHakemBildiriAtamaDal : EfEntityRepository<HakemBildiriAtama, DbtechconfContext>, IHakemBildiriAtamaDal
    {
        public HakemBildiriAtama Get_WInc(Expression<Func<HakemBildiriAtama, bool>> filter)
        {
            return Db.HakemBildiriAtama.Include(x => x.Bildiri).ThenInclude(x => x.Ek)
                .Include(x => x.Bildiri).ThenInclude(x => x.Yazar)
                .Include(x=>x.Bildiri).ThenInclude(x=>x.HakemBildiriAtama)
                .SingleOrDefault(filter);
        }

        public List<HakemBildiriAtama> GetList_WInc(Expression<Func<HakemBildiriAtama, bool>> filter = null)
        {
            return filter == null
                ? Db.HakemBildiriAtama.Include(x => x.Bildiri).ThenInclude(x => x.Ek)
                    .Include(x => x.Bildiri).ThenInclude(x => x.Yazar)
                    .ToList()
                : Db.HakemBildiriAtama.Include(x => x.Bildiri).ThenInclude(x => x.Ek)
                    .Include(x => x.Bildiri).ThenInclude(x => x.Yazar)
                    .Where(filter).ToList();

        }

        public void TopluSil(List<HakemBildiriAtama> silHakemBildiriList)
        {
            Db.HakemBildiriAtama.RemoveRange(silHakemBildiriList);
        }
    }
}