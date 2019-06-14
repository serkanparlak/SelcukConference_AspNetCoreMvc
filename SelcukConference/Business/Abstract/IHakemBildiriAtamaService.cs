using System.Collections.Generic;
using DataAccess.Entities;

namespace Business.Abstract
{
    public interface IHakemBildiriAtamaService
    {
        void EkleSilKaydet(int bildiriId, int[] seciliHakemIds, int editorId);
        void DeleteByBildiriId(int bId);
        HakemBildiriAtama Ekle(HakemBildiriAtama hakemBildiriAtama);
        void Kaydet();
        HakemBildiriAtama GetById(int hbId);
        HakemBildiriAtama GetWIncById(int hbId);
        void GuncelleKaydet(HakemBildiriAtama hakemBildiri);
        void TopluGuncelleKaydet(List<HakemBildiriAtama> hakemler);
        List<HakemBildiriAtama> GetByBildiriId(int bId);
        bool Kontrol(int bId);
    }
}