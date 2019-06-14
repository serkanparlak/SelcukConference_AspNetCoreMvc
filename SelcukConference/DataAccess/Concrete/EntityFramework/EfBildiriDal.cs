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
    public class EfBildiriDal : EfEntityRepository<Bildiri, DbtechconfContext>, IBildiriDal
    {
        public Bildiri Get_WInc(Expression<Func<Bildiri, bool>> filter)
        {
            return Db.Bildiri.Include(x => x.HakemBildiriAtama)
                .Include(x => x.Ek).Include(x => x.Yazar).SingleOrDefault(filter);
        }

        public List<Bildiri> GetList_WInc(Expression<Func<Bildiri, bool>> filter = null)
        {
            return filter == null
                ? Db.Bildiri.Include(x => x.HakemBildiriAtama)
                    .Include(x => x.Ek).Include(x => x.Yazar)
                    .ToList()
                : Db.Bildiri.Include(x => x.HakemBildiriAtama)
                    .Include(x => x.Ek).Include(x => x.Yazar)
                    .Where(filter).OrderByDescending(x=>x.BildiriTarih).ToList();
        }
    }
}