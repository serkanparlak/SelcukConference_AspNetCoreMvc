using DataAccess.Entities;
using _Core.DataAccess;
using System.Collections.Generic;

namespace DataAccess.Abstract
{
    public interface IPaperNotifyDal : IEntityRepository<PaperNotify>
    {
        void DeleteMultiple(List<PaperNotify> pNotifyList);
    }
}