using System;
using System.Linq;
using Business.Abstract;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Admin.Models;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IUlkeService _ulkeService;
        private readonly ISehirService _sehirService;
        private readonly IUyeService _uyeService;
        private readonly IGenelService _genelService;
        private readonly ISessionService _sessionService;

        public HomeController(IMenuService menuService, ISessionService sessionService, IUlkeService ulkeService, ISehirService sehirService, IUyeService uyeService, IGenelService genelService)
        {
            _menuService = menuService;
            _sessionService = sessionService;
            _ulkeService = ulkeService;
            _sehirService = sehirService;
            _uyeService = uyeService;
            _genelService = genelService;
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
            return View(model);
        }

        public ActionResult Register()
        {
            var model = new RegisterViewModel
            {
                Ulkeler = _ulkeService.GetList(),
                Turkiye = _sehirService.GetListByUlkeId(213)
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(Uye uye)
        {
            uye.Yazar = true;
            uye.Aktif = true;
            _uyeService.EkleVeKaydet(uye);
            _sessionService.SetUyeSession(uye);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Login(string mail, string pwd, string rmbr)
        {
            var user = _uyeService.OturumKontrol(mail, pwd);
            if (user == null) return Json(false);
            _sessionService.SetUyeSession(user);
            //if (rmbr != null)
            //{
            //    _sessionService.SetUyeCookie(mail, pwd);
            //}
            return Json(true);
        }


        public JsonResult MailKontrol(string mail)
        {
            return Json(!_uyeService.EmailKontrol(mail));
        }

        public JsonResult GetCityList(int ulkeId)
        {
            return Json(_sehirService.GetListByUlkeId(ulkeId).Select(x => new { x.Id, x.SehirAdi, x.UlkeId }));
        }

        public JsonResult RememberPwd(string mail)
        {
            return Json(_uyeService.SifreHatirlat(mail));
        }

        public ActionResult Logout()
        {
            _sessionService.ClearUyeSession();
            //_sessionService.ClerUyeCookie();
            return RedirectToAction("Index");
        }

        public JsonResult SifreDegistir(string yeniSifre)
        {
            try
            {
                var hesabim = _uyeService.Get(_sessionService.GetUyeSession().Id);
                hesabim.Sifre = yeniSifre;
                _uyeService.GuncelleKaydet(hesabim);
                _sessionService.SetUyeSession(hesabim);
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        public PartialViewResult _MemberDetail()
        {
            var uye = _uyeService.Get_WIncById(_sessionService.GetUyeSession().Id);
            var model = new MemberDetailViewModel
            {
                Uye = uye,
                Ulkeler = _ulkeService.GetList(),
                Sehirler = _sehirService.GetListByUlkeId(uye.Sehir.UlkeId)
            };
            return PartialView(model);
        }

        public JsonResult MailKontrolWithId(string mail)
        {
            return Json(!_uyeService.EmailKontrolWithId(mail, _sessionService.GetUyeSession().Id));
        }

        [HttpPost]
        public JsonResult MemberEdit(Uye uGet)
        {
            try
            {
                var uEdit = _uyeService.Get(_sessionService.GetUyeSession().Id);
                uEdit.Adi = uGet.Adi;
                uEdit.Adres = uGet.Adres;
                uEdit.Kurum = uGet.Kurum;
                uEdit.Unvan = uGet.Unvan;
                uEdit.Mail = uGet.Mail;
                uEdit.SehirId = uGet.SehirId;
                uEdit.Soyadi = uGet.Soyadi;
                uEdit.Telefon = uGet.Telefon;
                _uyeService.GuncelleKaydet(uEdit);
            }
            catch
            {
                return Json(false);
            }
            return Json(true);
        }

        public JsonResult LangChange(string lang)
        {
            _sessionService.SetLanguageCookie(lang);
            return Json(1);
        }
    }
}