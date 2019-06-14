using System.Collections.Generic;
using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Entities;

namespace Business.Concrete
{
    public class UlkeManager : IUlkeService
    {
        private readonly IUlkeDal _ulkeDal;

        public UlkeManager(IUlkeDal ulkeDal)
        {
            _ulkeDal = ulkeDal;
        }
        
        public Ulke Get(int ulkeId)
        {
            return _ulkeDal.Get(x => x.Id == ulkeId);
        }

        public Ulke GetWInc(int id)
        {
            return _ulkeDal.Get_WInc(x => x.Id == id);
        }

        public List<Ulke> GetList()
        {
            return _ulkeDal.GetList();
        }

        public List<Ulke> GetListWInc()
        {
            return _ulkeDal.GetList_WInc();
        }
    }
}
