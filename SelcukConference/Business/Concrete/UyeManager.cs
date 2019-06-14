using System.Collections.Generic;
using System.Linq;
using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Entities;
using _Core.MyClasses;

namespace Business.Concrete
{
    public class UyeManager : IUyeService
    {
        private readonly IUyeDal _uyeDal;

        public UyeManager(IUyeDal uyeDal)
        {
            _uyeDal = uyeDal;
        }

        public void EkleVeKaydet(Uye uye)
        {
            _uyeDal.AddWithSave(uye);
        }

        public bool EmailKontrol(string uyeMail)
        {
            return _uyeDal.Control(x => x.Mail == uyeMail);
        }

        public List<Uye> GetAll()
        {
            return _uyeDal.GetList(x=>!x.Admin);
        }

        public List<Uye> GetAllWInc()
        {
            return _uyeDal.GetList_WInc(x => !x.Admin);
        }

        public Uye Get(int id)
        {
            return _uyeDal.Get(x => x.Id == id);
        }

        public void GuncelleKaydet(Uye uye)
        {
            _uyeDal.UpdateWithSave(uye);
        }

        public void SilKaydet(Uye uye)
        {
            _uyeDal.DeleteWithSave(uye);
        }

        public Uye Get_WIncById(int id)
        {
            return _uyeDal.Get_WInc(x => x.Id == id);
        }

        public bool EmailKontrolWithId(string mail, int id)
        {
            return _uyeDal.Control(x => x.Mail == mail && x.Id != id);
        }

        public ICollection<Uye> GetAllEditors_WInc()
        {
            return _uyeDal.GetList_WInc(x => x.Editor && !x.Admin && x.Aktif);
        }

        public List<Uye> GetOrgKomite()
        {
            return _uyeDal.GetList_WInc(x => x.OrganizasyonKomite && !x.Admin);
        }

        public List<Uye> GetBilKomite()
        {
            return _uyeDal.GetList_WInc(x => x.BilimselKomite && !x.Admin);
        }

        public Uye OturumKontrol(string m, string p)
        {
            return _uyeDal.Get(x => x.Mail.ToLower() == m.ToLower() && x.Sifre == p && x.Aktif);
        }

        public bool SifreHatirlat(string m)
        {
            var uye = _uyeDal.Get(x => x.Mail == m);
            if (uye != null)
            {
                return StaticOperations.MailGonder(m, "Şifre Hatırlatma", "Şifreniz : " + uye.Sifre, false);
            }
            return false;
        }

        public List<Uye> GetMesajsizUyeler(int[] ids)
        {
            return _uyeDal.GetList(x => !ids.Contains(x.Id) && x.Aktif);
        }

        public List<Uye> GetAll_ByHakemId(int[] v)
        {
            return _uyeDal.GetList(x => v.Contains(x.Id));
        }

        public List<Uye> GetAll_WithoutEditorYazar(int yazarId, int editorId)
        {
            return _uyeDal.GetList(x => x.Id != yazarId && x.Id != editorId && !x.Admin && x.Aktif);
        }

        public Uye GetByCookie(string cookUye)
        {
            return _uyeDal.Get(x => StaticOperations.Md5Creator(x.Mail + "-" + x.Sifre) == cookUye);
        }
        
    }
}