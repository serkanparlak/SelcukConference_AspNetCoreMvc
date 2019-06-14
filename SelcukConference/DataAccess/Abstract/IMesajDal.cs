using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataAccess.Entities;
using _Core.DataAccess;

namespace DataAccess.Abstract
{
    public interface IMesajDal : IEntityRepository<Mesaj>
    {
        Mesaj Get_WInc(Expression<Func<Mesaj, bool>> filter);

        List<Mesaj> GetList_WInc(Expression<Func<Mesaj, bool>> filter = null);
        List<MessageNotify> GetForMesajNotify(int id);
        List<MessageNotify> GetMyLastMessageList(int gonderenId);
        int GetMyLastReadedMesageId(int aId, int gId);
    }
}