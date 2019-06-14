using DataAccess.Entities;

namespace Business.Abstract
{
    public interface IEkService
    {
        void Ekle(Ek ek);
        void Kaydet();
        Ek GetById(int ekId);
        void SilKaydet(Ek ek);
    }
}