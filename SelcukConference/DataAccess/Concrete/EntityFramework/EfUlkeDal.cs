using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.Abstract;
using _Core.DataAccess.EntityFramework;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUlkeDal : EfEntityRepository<Ulke, DbtechconfContext>, IUlkeDal
    {
        public Ulke Get_WInc(Expression<Func<Ulke, bool>> filter)
        {
            return Db.Ulke.Include(x => x.Sehir).ThenInclude(x=>x.Uye).FirstOrDefault(filter);
        }

        public List<Ulke> GetList_WInc(Expression<Func<Ulke, bool>> filter)
        {
            return filter == null
                ? Db.Ulke.Include(x=>x.Sehir).ThenInclude(x=>x.Uye).ToList()
                : Db.Ulke.Include(x => x.Sehir).ThenInclude(x => x.Uye).Where(filter).ToList();
        }
    }
}