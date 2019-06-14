using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataAccess.Entities;

namespace Business.Abstract
{
    public interface IPaperNotifyService
    {
        List<PaperNotify> GetList(Expression<Func<PaperNotify, bool>> p);
        List<PaperNotify> GetMyNotifyList(int id);
        void AddPaperNotify(PaperNotify pNotify);
        void RemovePaperNotify(PaperNotify pNotify);
        void RemovePaperNotifyMultiple(List<PaperNotify> pNotifyList);
        void Kaydet();
        void TopluOkundu(List<PaperNotify> list);
        PaperNotify GetById(int nId);
    }


}