using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Entities;

namespace Business.Concrete
{
    public class PaperNotifyManager : IPaperNotifyService
    {
        private readonly IPaperNotifyDal _paperNotifyDal;

        public PaperNotifyManager(IPaperNotifyDal paperNotifyDal)
        {
            _paperNotifyDal = paperNotifyDal;
        }

        public List<PaperNotify> GetList(Expression<Func<PaperNotify, bool>> p)
        {
            return _paperNotifyDal.GetList(p);
        }

        public List<PaperNotify> GetMyNotifyList(int id)
        {
            return _paperNotifyDal.GetList(x => x.UyeId == id);
        }

        public void AddPaperNotify(PaperNotify pNotify)
        {
            _paperNotifyDal.Add(pNotify);
        }

        public void RemovePaperNotify(PaperNotify pNotify)
        {
            _paperNotifyDal.Delete(pNotify);
        }

        public void RemovePaperNotifyMultiple(List<PaperNotify> pNotifyList)
        {
            _paperNotifyDal.DeleteMultiple(pNotifyList);
        }

        public void Kaydet()
        {
            _paperNotifyDal.Save();
        }

        public void TopluOkundu(List<PaperNotify> list)
        {
            _paperNotifyDal.UpdateMultiple(list);
        }

        public PaperNotify GetById(int nId)
        {
            return _paperNotifyDal.Get(x => x.Id == nId);
        }
    }
}
