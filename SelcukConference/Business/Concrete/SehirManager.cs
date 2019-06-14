using System.Collections.Generic;
using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Entities;

namespace Business.Concrete
{
    public class SehirManager : ISehirService
    {
        private readonly ISehirDal _sehirDal;

        public SehirManager(ISehirDal sehirDal)
        {
            _sehirDal = sehirDal;
        }

        public List<Sehir> GetListByUlkeId(int ulkeId)
        {
            return _sehirDal.GetList(x => x.UlkeId == ulkeId);
        }

        public Sehir GetById(int id)
        {
            return _sehirDal.Get(x=>x.Id == id);
        }

        public List<Sehir> All()
        {
            return _sehirDal.GetList();
        }

        public Sehir GetWIncById(int id)
        {
            return _sehirDal.Get_WInc(sehir => sehir.Id == id);
        }
    }
}