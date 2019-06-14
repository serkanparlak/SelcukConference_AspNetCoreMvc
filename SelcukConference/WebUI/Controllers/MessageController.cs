using System;
using System.Collections.Generic;
using System.Linq;
using Business.Abstract;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMesajService _mesajService;
        private readonly IUyeService _uyeService;
        private readonly ISessionService _sessionService;


        public MessageController(IMesajService mesajService, IUyeService uyeService, ISessionService sessionService)
        {
            _mesajService = mesajService;
            _uyeService = uyeService;
            _sessionService = sessionService;
        }

        public IActionResult Messages(int mId) // mId : mesajlaşılan kişinin Id'si
        {
            var oturum = _sessionService.GetUyeSession();
            var sonKonusmalar = _mesajService.GetMyLastMessageList(oturum.Id);
            var ids = sonKonusmalar.Select(x => x.LastMessage).Select(x => x.GonderenId).ToList();
            ids.AddRange(sonKonusmalar.Select(x => x.LastMessage).Select(x => x.AliciId));
            var konusulamayan = _uyeService.GetMesajsizUyeler(ids.Distinct().ToArray());
            var model = new MessageViewModel
            {
                MesajAliciUye = _uyeService.Get(mId),
                SonMesajUyeler = sonKonusmalar,
                MesajsizUyeler = konusulamayan,
                Oturum = oturum
            };
            return View(model);
        }

        public PartialViewResult _ChatMessages(int alici)
        {
            var gonderen = _sessionService.GetUyeSession();
            var model = new ChatMessagesViewModel
            {
                Messages = _mesajService.GetListBoth(gonderen.Id, alici),
                Gonderen = gonderen,
            };
            if (model.Messages.Count > 0)
            {
                var okunanlar = new List<Mesaj>();
                foreach (var item in model.Messages)
                {
                    if (item.GonderenId == alici && item.AliciId == gonderen.Id && !item.Okundu)
                    {
                        item.Okundu = true;
                        okunanlar.Add(item);
                    }
                }
                if (okunanlar.Count > 0)
                {
                    _mesajService.TopluGuncelleKaydet(okunanlar);
                }
            }
            return PartialView(model);
        }

        public JsonResult MessageSend(Mesaj msj)
        {
            msj.IletimTarihi = DateTime.Now;
            msj.Okundu = false;
            try
            {
                var addedMesaj = _mesajService.EkleKaydet(msj);
                return Json(new { sonuc = true, icerik = msj.Icerik, tarih = msj.IletimTarihi.ToString("f"), mesajId = addedMesaj.Id });
            }
            catch (Exception e)
            {
                return Json(new { sonuc = false, icerik = e.Message });
            }
        }

        public JsonResult MesajOkunduMuRealTime(int aliciId) // son okunan mesajımın Id'si gelir
        {
            return Json(_mesajService.GetSonOkunanMesajId(aliciId, _sessionService.GetUyeSession().Id));
        }

        public JsonResult NewMessageCheck(int aId)
        {
            var gId = _sessionService.GetUyeSession().Id;
            var message = _mesajService.GetNewMessageCheck(aId, gId);

            if (message != null)
            {
                message.Okundu = true;
                _mesajService.GuncelleKaydet(message);
                return Json(new { isSuccess = true, icerik = message.Icerik, tarih = message.IletimTarihi.ToString("f") });
            }
            return Json(new { isSuccess = false });
        }
    }
}