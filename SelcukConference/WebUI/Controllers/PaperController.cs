using System;
using System.IO;
using System.Linq;
using Business.Abstract;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using WebUI.Services;

/*
Bildiri.Sonuc ->
    2 : Bekliyor 
    3 : Onaylandı
    4 : Revizyon
    5 : Ret
*/
namespace WebUI.Controllers
{
    public class PaperController : Controller
    {
        private readonly IBildiriService _bildiriService;
        private readonly IHakemBildiriAtamaService _hakemBildiriAtamaService;
        private readonly IEkService _ekService;
        private readonly IKonuEtiketiService _konuEtiketiService;
        private readonly IUyeService _uyeService;
        private readonly ISessionService _sessionService;
        private readonly IPaperNotifyService _paperNotifyService;
        private readonly CustomLocalizer Localizer;

        public PaperController(IBildiriService bildiriService, ISessionService sessionService, IEkService ekService, IKonuEtiketiService konuEtiketiService, IUyeService uyeService, IHakemBildiriAtamaService hakemBildiriAtamaService, IPaperNotifyService paperNotifyService, CustomLocalizer localizer)
        {
            _sessionService = sessionService;
            _ekService = ekService;
            _konuEtiketiService = konuEtiketiService;
            _uyeService = uyeService;
            _hakemBildiriAtamaService = hakemBildiriAtamaService;
            _paperNotifyService = paperNotifyService;
            Localizer = localizer;
            _bildiriService = bildiriService;
        }

        public ActionResult Papers(int listId = 1)
        {
            var oturum = _sessionService.GetUyeSession();
            var model = new PaperViewModel
            {
                Bildirilerim = _bildiriService.GetListByYazarId(oturum.Id, listId),
                EditorBildirileri = _bildiriService.GetListByEditorId(oturum.Id, listId),
                HakemBildirileri = _bildiriService.GetListByHakemId(oturum.Id, listId),
                Oturum = oturum
            };
            TempData["oturum"] = oturum; //_PaperList'te kullanabilmek için oluşturuldu
            return View(model);
        }

        public PartialViewResult _PaperList(string listBy, int listId)
        {
            TempData["oturum"] = _sessionService.GetUyeSession();
            switch (listBy)
            {
                case "yazar":
                    return PartialView(_bildiriService.GetListByYazarId(_sessionService.GetUyeSession().Id, listId));
                case "editor":
                    return PartialView(_bildiriService.GetListByEditorId(_sessionService.GetUyeSession().Id, listId));
                default:
                    return PartialView(_bildiriService.GetListByHakemId(_sessionService.GetUyeSession().Id, listId));
            }
        }

        public ActionResult NewPaper()
        {
            if (_sessionService.GetUyeSession().Yazar)
                return View();
            return RedirectToAction("Papers", "Paper");
        }

        [HttpPost]
        public JsonResult NewPaper(Bildiri bildiri)
        {
            var jModel = new JsonModel();
            Uye editor = null;
            try
            {
                //var konuKelimeleri = bildiri.Konu.ToLower().Split(' ');
                var kEtikets = _konuEtiketiService.GetAll_WInc_NotMe(_sessionService.GetUyeSession().Id); //hem yazar kem editör ise kendi konuları gelmez
                foreach (var item in kEtikets)
                {
                    if (bildiri.Konu.ToLower().Contains(item.EtiketAdiTr.ToLower()) || bildiri.Konu.ToLower().Contains(item.EtiketAdiEng.ToLower()))
                    {
                        editor = item.Editor;
                        break;
                    }
                }
                if (editor == null)
                {
                    editor = kEtikets[new Random().Next(kEtikets.Count)].Editor;
                }
                var yeniBildiri = new Bildiri
                {
                    Aciklama = bildiri.Aciklama,
                    Konu = bildiri.Konu,
                    BildiriTarih = DateTime.Now,
                    YazarId = _sessionService.GetUyeSession().Id,
                    EditorId = editor.Id,
                    SayfaKimlik = Guid.NewGuid().ToString(),
                    Sonuc = 2, // bekleyen bildiri
                };
                var olusanBildiri = _bildiriService.EkleKaydet(yeniBildiri);
                _paperNotifyService.AddPaperNotify(new PaperNotify // bildiri oluşturuldu
                {
                    PaperId = olusanBildiri.Id,
                    UyeId = olusanBildiri.EditorId,
                    BildiriMesaj = Localizer["editorOlarakAtandiniz", olusanBildiri.Id],
                    OkunduMu = false
                });
                _paperNotifyService.Kaydet();
                jModel.sonuc = true;
                jModel.nesne = new
                {
                    olusanBildiri.Id,
                    olusanBildiri.YazarId
                };
            }
            catch (Exception e)
            {
                jModel.sonuc = false;
                jModel.mesaj = e.Message;
            }
            return Json(jModel);
        }

        public JsonResult DeletePaper(int bId)
        {
            var oturum = _sessionService.GetUyeSession();
            var jMod = new JsonModel { sonuc = true };
            try
            {
                var bildiri = _bildiriService.GetById(bId);
                if (bildiri.YazarId != oturum.Id && !oturum.Admin)
                {
                    jMod.sonuc = false;
                    jMod.mesaj = Localizer["silmeYetkinizYok"];
                    return Json(jMod);
                }
                var dosyaPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\ekler", bId + "\\");
                if (Directory.Exists(dosyaPath))
                {
                    Directory.Delete(dosyaPath, true);
                }
                _bildiriService.SilKaydet(bildiri);
                _paperNotifyService.RemovePaperNotifyMultiple(_paperNotifyService.GetList(x => x.PaperId == bId));
                _paperNotifyService.Kaydet();
            }
            catch (Exception e)
            {
                jMod.sonuc = false;
                jMod.mesaj = e.Message;
            }
            return Json(jMod);
        }

        public ActionResult EditPaper(int bId)
        {
            var oturum = _sessionService.GetUyeSession();
            var model = new PaperEditViewModel
            {
                Bildiri = _bildiriService.GetWIncById(bId)
            };
            if (model.Bildiri.YazarId != oturum.Id && !oturum.Admin)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult EditPaper(Bildiri formBildiri)
        {
            var jModel = new JsonModel { sonuc = true };
            try
            {
                var bildiri = _bildiriService.GetWIncById(formBildiri.Id);
                bildiri.Aciklama = formBildiri.Aciklama;
                bildiri.Konu = formBildiri.Konu;
                bildiri.BildiriTarih = DateTime.Now;
                bildiri.Sonuc = 2; // bekleyen bildiri
                _bildiriService.GuncelleKaydet(bildiri);
                _paperNotifyService.AddPaperNotify(new PaperNotify // bildiri oluşturuldu
                {
                    PaperId = bildiri.Id,
                    UyeId = bildiri.EditorId,
                    BildiriMesaj = Localizer["editorOldugunuzBidiriDuzenlendi", bildiri.Id],
                    OkunduMu = false
                });
                if (bildiri.HakemBildiriAtama.Count > 0)
                {
                    var hakemler = bildiri.HakemBildiriAtama;
                    foreach (var hakemBildiri in hakemler)
                    {
                        hakemBildiri.Sonuc = 2;
                        _paperNotifyService.AddPaperNotify(new PaperNotify // bildiri oluşturuldu
                        {
                            PaperId = bildiri.Id,
                            UyeId = hakemBildiri.HakemId,
                            BildiriMesaj = Localizer["hakemOldugunuzBidiriDuzenlendi", bildiri.Id],
                            OkunduMu = false
                        });
                    }
                    _hakemBildiriAtamaService.TopluGuncelleKaydet(hakemler.ToList());
                }
                _paperNotifyService.Kaydet();
            }
            catch (Exception e)
            {
                jModel.sonuc = false;
                jModel.mesaj = e.Message;
            }
            return Json(jModel);
        }

        [HttpPost]
        public JsonResult ImageUpload(int bId = 0)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\ekler\\" + bId);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            foreach (var file in Request.Form.Files)
            {
                var yeniAd = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var ekYol = "/images/ekler/" + bId + "/" + yeniAd;
                using (var stream = new FileStream(path + "\\" + yeniAd, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                var ek = new Ek
                {
                    BildiriId = bId,
                    EkYol = ekYol
                };
                _ekService.Ekle(ek);
            }
            _ekService.Kaydet();
            return Json(true);
        }

        [HttpPost]
        public JsonResult ImageRemove(int ekId)
        {
            var jMod = new JsonModel { sonuc = true };
            try
            {
                var ek = _ekService.GetById(ekId);
                var ekYol = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ek.EkYol.Substring(1).Replace("/", "\\"));
                if (System.IO.File.Exists(ekYol))
                    System.IO.File.Delete(ekYol);
                _ekService.SilKaydet(ek);
            }
            catch (Exception e)
            {
                jMod.sonuc = false;
                jMod.mesaj = e.Message;
            }
            return Json(jMod);
        }

        public ActionResult PaperDetail(int bId)
        {
            var bildiri = _bildiriService.GetWIncById(bId);
            var oturum = _sessionService.GetUyeSession();
            var model = new PaperDetailViewModel
            {
                Bildiri = bildiri,
                Oturum = oturum,
                AtananHakemler = _uyeService.GetAll_ByHakemId(bildiri.HakemBildiriAtama.Select(x => x.HakemId).ToArray()),
                TumHakemler = _uyeService.GetAll_WithoutEditorYazar(bildiri.YazarId, bildiri.EditorId),
                HakemBildiri = bildiri.HakemBildiriAtama.SingleOrDefault(x => x.HakemId == oturum.Id)
            };
            return View(model);
        }

        [HttpPost]
        public JsonResult HakemAta(int bId, int selectAssign, int adet, int[] seciliHakemIds)
        {
            var rnd = new Random();
            try
            {
                var bildiri = _bildiriService.GetById(bId);
                if (selectAssign == 0) // rastgele hakem ataması yapılır
                {
                    _hakemBildiriAtamaService.DeleteByBildiriId(bId); // eski hakemler siliniyor
                    var silinecekHakemIds = _hakemBildiriAtamaService.GetByBildiriId(bId).Select(x => x.HakemId).ToArray(); // eski hakemId'ler getiriliyor
                    var hakemler = _uyeService.GetAll_WithoutEditorYazar(bildiri.YazarId, bildiri.EditorId);
                    if (silinecekHakemIds.Length > 0)
                    {
                        _paperNotifyService.RemovePaperNotifyMultiple(_paperNotifyService.GetList(x => x.PaperId == bId && silinecekHakemIds.Contains(x.UyeId)));
                    }
                    for (var i = 0; i < adet; i++)
                    {
                        var secilenUye = hakemler[rnd.Next(0, hakemler.Count)];
                        _hakemBildiriAtamaService.Ekle(new HakemBildiriAtama
                        {
                            BildiriId = bId,
                            HakemId = secilenUye.Id,
                            Sonuc = 2
                        });
                        _paperNotifyService.AddPaperNotify(new PaperNotify // bildiri oluşturuldu
                        {
                            PaperId = bildiri.Id,
                            UyeId = secilenUye.Id,
                            BildiriMesaj = Localizer["hakemOlarakAtandiniz", bildiri.Id],
                            OkunduMu = false
                        });
                        hakemler.Remove(secilenUye);
                    }
                    _hakemBildiriAtamaService.Kaydet();
                    _paperNotifyService.Kaydet();
                }
                else // seçilen hakemler atanır
                {
                    _hakemBildiriAtamaService.EkleSilKaydet(bId, seciliHakemIds, bildiri.EditorId);
                }
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult EditorKarar(int bId, int editorSonuc, string editorNotu)
        {
            try
            {
                var bildiri = _bildiriService.GetWIncById(bId);
                bildiri.Sonuc = editorSonuc;
                bildiri.EditorNotu = editorNotu;
                _bildiriService.GuncelleKaydet(bildiri);
                _paperNotifyService.AddPaperNotify(new PaperNotify // bildiri oluşturuldu
                {
                    PaperId = bId,
                    UyeId = bildiri.YazarId,
                    BildiriMesaj = Localizer["yazarOldugunEditorKarari", bId],
                    OkunduMu = false
                });
                foreach (var hakem in bildiri.HakemBildiriAtama)
                {
                    _paperNotifyService.AddPaperNotify(new PaperNotify // bildiri oluşturuldu
                    {
                        PaperId = bId,
                        UyeId = hakem.HakemId,
                        BildiriMesaj = Localizer["hakemOldugunEditorKarari", bId],
                        OkunduMu = false
                    });
                }
                _paperNotifyService.Kaydet();
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult HakemKarar(int hbId, int hakemSonuc, string hakemNotu)
        {
            try
            {
                var hakemBildiri = _hakemBildiriAtamaService.GetWIncById(hbId);
                hakemBildiri.Sonuc = hakemSonuc;
                hakemBildiri.HakemNotu = hakemNotu;
                _hakemBildiriAtamaService.GuncelleKaydet(hakemBildiri);
                if (hakemBildiri.Bildiri.HakemBildiriAtama.All(x => x.Sonuc != 2))
                {
                    _paperNotifyService.AddPaperNotify(new PaperNotify // bildiri oluşturuldu
                    {
                        PaperId = hakemBildiri.BildiriId,
                        UyeId = hakemBildiri.Bildiri.EditorId,
                        BildiriMesaj = Localizer["editorOldugunTumHakemKarari", hakemBildiri.BildiriId],
                        OkunduMu = false
                    });
                }
                else
                {
                    _paperNotifyService.AddPaperNotify(new PaperNotify // bildiri oluşturuldu
                    {
                        PaperId = hakemBildiri.BildiriId,
                        UyeId = hakemBildiri.Bildiri.EditorId,
                        BildiriMesaj = Localizer["editorOldugunHakemKarari", hakemBildiri.BildiriId],
                        OkunduMu = false
                    });
                }
                _paperNotifyService.Kaydet();
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        public JsonResult BildirilerOkundu(int uId)
        {
            var notifys = _paperNotifyService.GetList(x => !x.OkunduMu);
            foreach (var notify in notifys)
            {
                notify.OkunduMu = true;
            }
            _paperNotifyService.TopluOkundu(notifys);
            _paperNotifyService.Kaydet();
            return Json(true);
        }

        public JsonResult RemoveNotify(int nId)
        {
            var jMod = new JsonModel { sonuc = true };
            try
            {
                var notify = _paperNotifyService.GetById(nId);
                _paperNotifyService.RemovePaperNotify(notify);
                _paperNotifyService.Kaydet();
            }
            catch (Exception e)
            {
                jMod.sonuc = false;
                jMod.mesaj = e.Message;
            }
            return Json(jMod);
        }
    }
}