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
    public class EfMesajDal : EfEntityRepository<Mesaj, DbtechconfContext>, IMesajDal
    {
        public Mesaj Get_WInc(Expression<Func<Mesaj, bool>> filter)
        {
            return Db.Mesaj.Include(x => x.Gonderen).ThenInclude(x => x.Bildiri)
                .ThenInclude(x => x.HakemBildiriAtama).Include(x => x.Gonderen).ThenInclude(x => x.KonuEtiketi)
                .Include(x => x.Gonderen).ThenInclude(x => x.Sehir).ThenInclude(x => x.Ulke)
                .Include(x => x.Alici).ThenInclude(x => x.Bildiri).ThenInclude(x => x.HakemBildiriAtama)
                .Include(x => x.Alici).ThenInclude(x => x.KonuEtiketi)
                .Include(x => x.Alici).ThenInclude(x => x.Sehir).ThenInclude(x => x.Ulke)
                .FirstOrDefault(filter);
        }

        public List<Mesaj> GetList_WInc(Expression<Func<Mesaj, bool>> filter)
        {
            return filter == null
                ? Db.Mesaj.Include(x => x.Gonderen).ThenInclude(x => x.Bildiri).ThenInclude(x => x.HakemBildiriAtama)
                    .Include(x => x.Gonderen).ThenInclude(x => x.KonuEtiketi)
                    .Include(x => x.Gonderen).ThenInclude(x => x.Sehir).ThenInclude(x => x.Ulke)
                    .Include(x => x.Alici).ThenInclude(x => x.Bildiri).ThenInclude(x => x.HakemBildiriAtama)
                    .Include(x => x.Alici).ThenInclude(x => x.KonuEtiketi)
                    .Include(x => x.Alici).ThenInclude(x => x.Sehir).ThenInclude(x => x.Ulke)
                    .ToList()
                : Db.Mesaj.Include(x => x.Gonderen).ThenInclude(x => x.Bildiri).ThenInclude(x => x.HakemBildiriAtama)
                    .Include(x => x.Gonderen).ThenInclude(x => x.KonuEtiketi)
                    .Include(x => x.Gonderen).ThenInclude(x => x.Sehir).ThenInclude(x => x.Ulke)
                    .Include(x => x.Alici).ThenInclude(x => x.Bildiri).ThenInclude(x => x.HakemBildiriAtama)
                    .Include(x => x.Alici).ThenInclude(x => x.KonuEtiketi)
                    .Include(x => x.Alici).ThenInclude(x => x.Sehir).ThenInclude(x => x.Ulke)
                    .Where(filter).ToList();
        }

        public List<MessageNotify> GetForMesajNotify(int id)
        {
            return (from m in Db.Mesaj.Include(x => x.Gonderen).Where(x => x.AliciId == id && !x.Okundu)
                    group m by m.GonderenId
                into grup
                    select new MessageNotify
                    {
                        TotalUnread = grup.Count(),
                        LastMessage = grup.LastOrDefault()
                    }).ToList();
        }

        public List<MessageNotify> GetMyLastMessageList(int gonderenId)
        {
            var myMessages = Db.Mesaj.Include(x => x.Gonderen).Include(x=>x.Alici).Where(x => x.AliciId == gonderenId || x.GonderenId == gonderenId);
            var konusulanIds = myMessages.Select(x => x.AliciId).ToList();
            konusulanIds.AddRange(myMessages.Select(x=>x.GonderenId));
            konusulanIds.RemoveAll(i => i.Equals(gonderenId));
            return konusulanIds.Distinct()
                .Select(uyeId => myMessages.Where(x => x.AliciId == uyeId || x.GonderenId == uyeId))
                .Select(thisUye => new MessageNotify
                {
                    LastMessage = thisUye.LastOrDefault(),
                    TotalUnread = thisUye.Count(x => x.AliciId == gonderenId && !x.Okundu)
                })
                .ToList();
            //var mNotify = new List<MessageNotify>();
            //foreach (var uyeId in konusulanIds.Distinct())
            //{
            //    var thisUye = myMessages.Where(x => x.AliciId == uyeId || x.GonderenId == uyeId);
            //    mNotify.Add(new MessageNotify
            //    {
            //        LastMessage = thisUye.OrderByDescending(x => x.IletimTarihi).LastOrDefault(),
            //        TotalUnread = thisUye.Count(x =>x.AliciId == gonderenId && !x.Okundu)
            //    });
            //}
            //return mNotify;

        }

        public int GetMyLastReadedMesageId(int aId, int gId)
        {
            var lastMessage = Db.Mesaj.LastOrDefault(x => x.AliciId == aId && x.GonderenId == gId && x.Okundu);
            return lastMessage?.Id ?? 0;
        }
    }
}