using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Entities;

namespace Business.Concrete
{
    public class EkManager : IEkService
    {
        private readonly IEkDal _ekDal;

        public EkManager(IEkDal ekDal)
        {
            _ekDal = ekDal;
        }

        public void Ekle(Ek ek)
        {
            _ekDal.Add(ek);
        }

        public void Kaydet()
        {
            _ekDal.Save();
        }

        public Ek GetById(int ekId)
        {
            return _ekDal.Get(x => x.Id == ekId);
        }

        public void SilKaydet(Ek ek)
        {
            _ekDal.DeleteWithSave(ek);
        }
    }
}