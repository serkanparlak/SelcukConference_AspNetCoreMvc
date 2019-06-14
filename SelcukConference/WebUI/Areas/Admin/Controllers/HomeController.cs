using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Business.Abstract;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebUI.Areas.Admin.Models;
using WebUI.Services;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IUyeService _uyeService;
        private readonly IUlkeService _ulkeService;
        private readonly ISehirService _sehirService;
        private readonly IKonuEtiketiService _konuEtiketiService;
        private readonly IGenelService _genelService;
        private readonly ISessionService _sessionService;
        
        public HomeController(IMenuService menuService, IUyeService uyeService, ISehirService sehirService, IUlkeService ulkeService, IKonuEtiketiService konuEtiketiService, IGenelService genelService, ISessionService sessionService)
        {
            _menuService = menuService;
            _uyeService = uyeService;
            _sehirService = sehirService;
            _ulkeService = ulkeService;
            _konuEtiketiService = konuEtiketiService;
            _genelService = genelService;
            _sessionService = sessionService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (_sessionService.GetUyeSession() == null || !_sessionService.GetUyeSession().Admin)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
            base.OnActionExecuting(context);
        }

        public ViewResult Index()
        {
            var pageId = Int32.TryParse(Request.Query["page"], out _) ? Convert.ToInt32(Request.Query["page"]) : 0;
            var activeMenu = _menuService.GetMenu(pageId) ?? _menuService.Anasayfa();
            var model = new ContentViewModel
            {
                ActiveMenu = activeMenu,
                UstMenu = activeMenu.AltMenuId == 0 ? null : _menuService.GetMenu(activeMenu.AltMenuId),
                GenelData = _genelService.GetSingle()
            };
            if (activeMenu.MenuAdi.ToLower().Contains("komite") || activeMenu.MenuAdiEng.ToLower().Contains("commit"))
            {
                model.OrgKomites = _uyeService.GetOrgKomite();
                model.BilKomites = _uyeService.GetBilKomite();
            }
            return View(model);
        }

        public JsonResult GetMenu(int id)
        {
            var menu = _menuService.GetMenu(id);
            var menuList = _menuService.GetMenuListWithSubId(menu.AltMenuId, id).Select(x => new { x.Id, x.MenuAdi, x.MenuAdiEng });
            return Json(new
            {
                menuid = menu.Id,
                altmenuid = menu.AltMenuId,
                menuad = menu.MenuAdi,
                menuadieng = menu.MenuAdiEng,
                menuler = menuList
            });
        }

        public JsonResult GetSubMenus(int altMenuId)
        {
            return Json(_menuService.GetMenuListWithSubId(altMenuId, 0).Select(x => new { x.Id, x.MenuAdi, x.MenuAdiEng }));
        }

        public JsonResult MenuKaydet(Menu m)
        {
            if (_menuService.MenuKontrol(m.Id, m.MenuAdi, m.MenuAdiEng)) //isimler daha önce kullanılmış mı?
            {
                return Json(new { IsSuccess = false });
            }

            var donenMenuId = 0;
            if (m.Id == 0) //Ekleme işlemi
            {
                donenMenuId = _menuService.Ekle(m);
            }
            else //Güncellme işlemi
            {
                _menuService.Guncelle(m);
            }
            return Json(new { isSuccess = true, id = donenMenuId });
        }

        public JsonResult MenuSil(int id)
        {
            var menu = _menuService.GetMenu(id);
            if (menu == null)
                return Json(false);
            foreach (var submenu in _menuService.SiraliListe().Where(x => x.AltMenuId == id))
            {
                try { _menuService.Sil(submenu); }
                catch { return Json(false); }
            }
            try { _menuService.Sil(menu); }
            catch { return Json(false); }
            return Json(true);
        }

        [HttpPost]
        //[DataType(DataType.MultilineText)]
        public JsonResult IcerikKayit(Menu icerik)
        {
            _menuService.Guncelle(icerik);
            return Json(true);
        }

        public ViewResult Members(MemberSearchModel msm)
        {
            IEnumerable<Uye> sorgulama = _uyeService.GetAllWInc();
            if (msm.AktifPasif != 0)
            {
                sorgulama = sorgulama.Where(x => x.Aktif == (msm.AktifPasif == 1));
            }

            if (msm.EmailTelefon != null)
            {
                sorgulama = sorgulama.Where(x =>
                    msm.EmailTelefon.ToLower().Contains(x.Mail.ToLower()) ||
                    x.Mail.ToLower().Contains(msm.EmailTelefon.ToLower()) ||
                    msm.EmailTelefon.ToLower().Contains(x.Telefon.ToLower()) ||
                    x.Telefon.ToLower().Contains(msm.EmailTelefon.ToLower()));
            }

            if (msm.IsimSoyisimKurum != null)
            {
                sorgulama = sorgulama.Where(x =>
                    msm.IsimSoyisimKurum.ToLower().Contains(x.Unvan.ToLower()) ||
                    x.Unvan.ToLower().Contains(msm.IsimSoyisimKurum.ToLower()) ||
                    msm.IsimSoyisimKurum.ToLower().Contains(x.Adi.ToLower()) ||
                    x.Adi.ToLower().Contains(msm.IsimSoyisimKurum.ToLower()) ||
                    msm.IsimSoyisimKurum.ToLower().Contains(x.Soyadi.ToLower()) ||
                    x.Soyadi.ToLower().Contains(msm.IsimSoyisimKurum.ToLower()) ||
                    msm.IsimSoyisimKurum.ToLower().Contains(x.Kurum.ToLower()) ||
                    x.Kurum.ToLower().Contains(msm.IsimSoyisimKurum.ToLower()));
            }

            if (msm.YazarEditorHakem != 0)
            {
                if (msm.YazarEditorHakem == 1)
                    sorgulama = sorgulama.Where(x => x.Yazar);
                if (msm.YazarEditorHakem == 2)
                    sorgulama = sorgulama.Where(x => x.Editor);
                if (msm.YazarEditorHakem == 3)
                    sorgulama = sorgulama.Where(x => x.Hakem);
            }
            var model = new MemberListViewModel
            {
                UyeList = sorgulama.ToList(),
                MemberSearch = msm
            };
            return View(model);
        }

        public JsonResult UyeSil(int id)
        {
            if (!_sessionService.GetUyeSession().Admin)
            {
                return Json(false);
            }
            var uye = _uyeService.Get(id);
            if (uye == null)
                return Json("Null Referans");
            try { _uyeService.SilKaydet(uye); }
            catch (Exception e) { return Json(e.Message); }
            return Json(true);
        }

        public JsonResult ChangePassword(int id, string sifre)
        {
            var uye = _uyeService.Get(id);
            uye.Sifre = sifre;
            try
            {
                _uyeService.GuncelleKaydet(uye);
                return Json(true);
            }
            catch
            {
                return Json(false);
            }

        }

        public PartialViewResult _MemberDetail(int id)
        {
            var uye = _uyeService.Get_WIncById(id);
            var model = new MemberDetailViewModel
            {
                Uye = uye,
                Ulkeler = _ulkeService.GetList(),
                Sehirler = _sehirService.GetListByUlkeId(uye.Sehir.UlkeId)
            };
            return PartialView(model);
        }

        public JsonResult MailKontrol(string mail, int id)
        {
            return Json(!_uyeService.EmailKontrolWithId(mail, id));
        }

        public ActionResult MemberEdit(Uye uGet, string intEditor, string intHakem, string intYazar, string intAktif, string intBil, string intOrg)
        {
            try
            {
                var uEdit = _uyeService.Get(uGet.Id);
                uEdit.Adi = uGet.Adi;
                uEdit.Adres = uGet.Adres;
                uEdit.Kurum = uGet.Kurum;
                uEdit.Unvan = uGet.Unvan;
                uEdit.Mail = uGet.Mail;
                uEdit.SehirId = uGet.SehirId;
                uEdit.Soyadi = uGet.Soyadi;
                uEdit.Telefon = uGet.Telefon;
                uEdit.Aktif = uGet.Aktif;
                uEdit.Editor = intEditor != null;
                uEdit.Hakem = intHakem != null;
                uEdit.Yazar = intYazar != null;
                uEdit.Aktif = intAktif != null;
                uEdit.OrganizasyonKomite = intOrg != null;
                uEdit.BilimselKomite = intBil != null;
                _uyeService.GuncelleKaydet(uEdit);
                TempData["sonuc"] = "True";
            }
            catch (Exception e)
            {
                TempData["sonuc"] = e.Message;
            }
            return RedirectToRoute("areaRoute", new { controller = "Home", action = "Members" });
        }

        public ActionResult SubjectOperations()
        {
            var model = new EditorOperationsViewModel
            {
                Konular = _konuEtiketiService.GetAll_WInc()
            };
            ViewBag.Editors = _uyeService.GetAllEditors_WInc().Select(x => new { x.Id, Adi = x.Unvan + " " + x.Adi + " " + x.Soyadi });
            return View(model);
        }

        public ActionResult KonuEkle(KonuEtiketi newKonu)
        {
            try
            {
                _konuEtiketiService.EkleKaydet(newKonu);
                TempData["sonuc"] = "True";
            }
            catch (Exception e)
            {
                TempData["sonuc"] = e.Message;
            }
            return RedirectToRoute("areaRoute", new { controller = "Home", action = "SubjectOperations" });
        }

        public PartialViewResult _SubjectDetail(int id)
        {
            var konu = _konuEtiketiService.Get_WIncById(id);
            ViewBag.Editors = _uyeService.GetAllEditors_WInc().Select(x => new { x.Id, Adi = x.Nick });
            return PartialView(konu);
        }

        public JsonResult KonuSil(int id)
        {
            var konu = _konuEtiketiService.Get_WIncById(id);
            try
            {
                _konuEtiketiService.SilKaydet(konu);
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        public ActionResult SubjectEdit(KonuEtiketi editKonu)
        {
            try
            {
                _konuEtiketiService.GuncelleKaydet(editKonu);
                TempData["sonuc"] = "True";
            }
            catch (Exception e)
            {
                TempData["sonuc"] = e.Message;
            }
            return RedirectToRoute("areaRoute", new { controller = "Home", action = "SubjectOperations" });
        }

        public ActionResult MemberFastOperations(MemberSearchModel msm)
        {
            IEnumerable<Uye> sorgulama = _uyeService.GetAllWInc();
            if (msm.AktifPasif != 0)
            {
                sorgulama = sorgulama.Where(x => x.Aktif == (msm.AktifPasif == 1));
            }
            if (msm.IsimSoyisimKurum != null)
            {
                sorgulama = sorgulama.Where(x =>
                    msm.IsimSoyisimKurum.ToLower().Contains(x.Unvan.ToLower()) ||
                    x.Unvan.ToLower().Contains(msm.IsimSoyisimKurum.ToLower()) ||
                    msm.IsimSoyisimKurum.ToLower().Contains(x.Adi.ToLower()) ||
                    x.Adi.ToLower().Contains(msm.IsimSoyisimKurum.ToLower()) ||
                    msm.IsimSoyisimKurum.ToLower().Contains(x.Soyadi.ToLower()) ||
                    x.Soyadi.ToLower().Contains(msm.IsimSoyisimKurum.ToLower()) ||
                    msm.IsimSoyisimKurum.ToLower().Contains(x.Kurum.ToLower()) ||
                    x.Kurum.ToLower().Contains(msm.IsimSoyisimKurum.ToLower()));
            }
            if (msm.YazarEditorHakem != 0)
            {
                if (msm.YazarEditorHakem == 1)
                    sorgulama = sorgulama.Where(x => x.Yazar);
                else if (msm.YazarEditorHakem == 2)
                    sorgulama = sorgulama.Where(x => x.Editor);
                else if (msm.YazarEditorHakem == 3)
                    sorgulama = sorgulama.Where(x => x.Hakem);
            }
            if (msm.BilimselOrganizasyon != 0)
            {
                if (msm.BilimselOrganizasyon == 1)
                    sorgulama = sorgulama.Where(x => x.BilimselKomite);
                else if (msm.BilimselOrganizasyon == 2)
                    sorgulama = sorgulama.Where(x => x.OrganizasyonKomite);
            }
            var model = new MemberListViewModel
            {
                UyeList = sorgulama.ToList(),
                MemberSearch = msm
            };
            return View(model);
        }

        public JsonResult FastEdit(int uyeid, string kolon, bool deger)
        {
            var uye = _uyeService.Get(uyeid);
            if (kolon == "Aktif")
                uye.Aktif = deger;
            else if (kolon == "Yazar")
                uye.Yazar = deger;
            else if (kolon == "Editor")
                uye.Editor = deger;
            else if (kolon == "Hakem")
                uye.Hakem = deger;
            else if (kolon == "BilimselKomite")
                uye.BilimselKomite = deger;
            else if (kolon == "Organizasyonkomite")
                uye.OrganizasyonKomite = deger;
            try
            {
                _uyeService.GuncelleKaydet(uye);
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        public ActionResult General()
        {
            return View(_genelService.GetSingle());
        }

        public ActionResult GenelData(string date, IFormFile banner)
        {
            var genelData = _genelService.GetSingle();
            //resim düzenle ??
            if (banner != null && banner.Length > 0)
            {
                try
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + genelData.BannerYol);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    var bannerYol = "/images/banner/" + banner.FileName;
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + bannerYol);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        banner.CopyTo(stream);
                    }
                    genelData.BannerYol = bannerYol;
                    _genelService.GuncelleKaydet(genelData);
                    TempData.Add("resimYukleme", "True");
                }
                catch (Exception e)
                {
                    TempData.Add("resimYukleme", e.Message);
                }
                return RedirectToRoute("areaRoute", new { controller = "Home", action = "General" });
            }
            //date change
            if (date != null)
            {
                try
                {
                    if (GlobalViews.Culture.Name == "en")
                    {
                        var bolunmus = date.Split('.');
                        date = bolunmus[1] + "." + bolunmus[0] + "." + bolunmus[2];
                    }
                    genelData.KonferanTarihi = Convert.ToDateTime(date);
                    _genelService.GuncelleKaydet(genelData);
                    return Json(true);
                }
                catch
                {
                    return Json(false);
                }
            }
            return Json(false);
        }
    }
}
