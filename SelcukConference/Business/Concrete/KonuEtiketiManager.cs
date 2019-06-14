using System.Collections.Generic;
using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Entities;

namespace Business.Concrete
{
    public class KonuEtiketiManager : IKonuEtiketiService
    {
        private readonly IKonuEtiketiDal _konuEtiketiDal;

        public KonuEtiketiManager(IKonuEtiketiDal konuEtiketiDal)
        {
            _konuEtiketiDal = konuEtiketiDal;
        }


        public KonuEtiketi Get_WIncById(int id)
        {
            return _konuEtiketiDal.GetWIncById(id);
        }

        public List<KonuEtiketi> GetAll_WInc()
        {
            return _konuEtiketiDal.GetListWInc();
        }

        public void SilKaydet(KonuEtiketi konu)
        {
            _konuEtiketiDal.DeleteWithSave(konu);
        }

        public void GuncelleKaydet(KonuEtiketi editKonu)
        {
            _konuEtiketiDal.UpdateWithSave(editKonu);
        }

        public void EkleKaydet(KonuEtiketi newKonu)
        {
            _konuEtiketiDal.AddWithSave(newKonu);
        }

        public List<KonuEtiketi> GetAll_WInc_NotMe(int yazarId)
        {
            return _konuEtiketiDal.GetListWInc(x=>x.EditorId != yazarId);
        }
    }
}