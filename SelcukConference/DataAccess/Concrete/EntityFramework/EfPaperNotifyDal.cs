using System.Collections.Generic;
using DataAccess.Abstract;
using DataAccess.Entities;
using _Core.DataAccess.EntityFramework;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfPaperNotifyDal : EfEntityRepository<PaperNotify, DbtechconfContext>, IPaperNotifyDal
    {
        public void DeleteMultiple(List<PaperNotify> pNotifyList)
        {
            Db.PaperNotify.RemoveRange(pNotifyList);
        }
    }
}