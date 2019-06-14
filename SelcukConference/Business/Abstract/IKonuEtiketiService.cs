using System.Collections.Generic;
using DataAccess.Entities;

namespace Business.Abstract
{
    public interface IKonuEtiketiService
    {
        KonuEtiketi Get_WIncById(int id);
        List<KonuEtiketi> GetAll_WInc();
        void SilKaydet(KonuEtiketi konu);
        void GuncelleKaydet(KonuEtiketi editKonu);
        void EkleKaydet(KonuEtiketi newKonu);
        List<KonuEtiketi> GetAll_WInc_NotMe(int yazarId);
    }
}