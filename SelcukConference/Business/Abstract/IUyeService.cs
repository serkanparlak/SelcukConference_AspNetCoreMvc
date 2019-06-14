using System.Collections.Generic;
using DataAccess.Entities;

namespace Business.Abstract
{
    public interface IUyeService
    {
        void EkleVeKaydet(Uye uye);
        bool EmailKontrol(string uyeMail);
        List<Uye> GetAll();
        List<Uye> GetAllWInc();
        Uye Get(int id);
        void GuncelleKaydet(Uye uye);
        void SilKaydet(Uye uye);
        Uye Get_WIncById(int id);
        bool EmailKontrolWithId(string mail, int id);
        ICollection<Uye> GetAllEditors_WInc();
        List<Uye> GetOrgKomite();
        List<Uye> GetBilKomite();
        Uye OturumKontrol(string m, string p);
        bool SifreHatirlat(string m);
        List<Uye> GetMesajsizUyeler(int[] ids);
        List<Uye> GetAll_ByHakemId(int[] v);
        List<Uye> GetAll_WithoutEditorYazar(int yazarId, int editorId);
        Uye GetByCookie(string cookUye);
    }
}