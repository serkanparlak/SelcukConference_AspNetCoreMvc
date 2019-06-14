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
    public class EfSehirDal : EfEntityRepository<Sehir, DbtechconfContext>, ISehirDal
    {
        public Sehir Get_WInc(Expression<Func<Sehir, bool>> filter)
        {
            return Db.Sehir.Include(x => x.Ulke).Include(x => x.Uye).FirstOrDefault(filter);
        }

        public List<Sehir> GetList_WInc(Expression<Func<Sehir, bool>> filter)
        {
            return filter == null
                ? Db.Sehir.Include(x => x.Ulke).Include(x => x.Uye).ToList()
                : Db.Sehir.Include(x => x.Ulke).Include(x => x.Uye).Where(filter).ToList();
        }
    }
}