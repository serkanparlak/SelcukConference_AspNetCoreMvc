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
    public class EfUyeDal : EfEntityRepository<Uye, DbtechconfContext>, IUyeDal
    {
        public Uye Get_WInc(Expression<Func<Uye, bool>> filter)
        {
            return Db.Uye.Include(uye => uye.Bildiri).ThenInclude(x=>x.Ek)
                .Include(uye => uye.Sehir).ThenInclude(x=>x.Ulke)
                .Include(uye => uye.KonuEtiketi)
                .Include(uye => uye.MesajAlici).ThenInclude(x => x.Gonderen)
                .Include(uye => uye.MesajGonderen).ThenInclude(x => x.Alici)
                .FirstOrDefault(filter);
        }

        public List<Uye> GetList_WInc(Expression<Func<Uye, bool>> filter = null)
        {
            return filter == null
                ? Db.Uye.Include(uye => uye.Bildiri).ThenInclude(x => x.Ek).Include(uye => uye.Sehir).ThenInclude(x => x.Ulke)
                    .Include(uye => uye.KonuEtiketi).Include(uye => uye.MesajAlici).ThenInclude(x => x.Alici)
                    .Include(uye => uye.MesajGonderen).ThenInclude(x => x.Gonderen).ToList()
                : Db.Uye.Include(uye => uye.Bildiri).ThenInclude(x => x.Ek).Include(uye => uye.Sehir).ThenInclude(x => x.Ulke)
                    .Include(uye => uye.KonuEtiketi).Include(uye => uye.MesajAlici).ThenInclude(x => x.Alici)
                    .Include(uye => uye.MesajGonderen).ThenInclude(x => x.Gonderen).Where(filter).ToList();
        }
    }
}