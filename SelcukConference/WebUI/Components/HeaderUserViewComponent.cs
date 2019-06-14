using System.Collections.Generic;
using Business.Abstract;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Components
{
    public class HeaderUserViewComponent : ViewComponent
    {
        private readonly ISessionService _sessionService;
        private readonly IPaperNotifyService _paperNotifyService;
        private readonly IMesajService _mesajService;

        public HeaderUserViewComponent(ISessionService sessionService, IPaperNotifyService paperNotifyService, IMesajService mesajService)
        {
            _sessionService = sessionService;
            _paperNotifyService = paperNotifyService;
            _mesajService = mesajService;
        }

        public ViewViewComponentResult Invoke()
        {
            var oturum = _sessionService.GetUyeSession();
            List<PaperNotify> paperNotifies = null;
            List<MessageNotify> mesajNotifies = null;
            if (oturum != null)
            {
                paperNotifies = _paperNotifyService.GetMyNotifyList(oturum.Id);
                mesajNotifies = _mesajService.GetForMesajNotify(oturum.Id);
            }

            var model = new HeaderViewModel
            {
                Oturum = oturum,
                PaperNotifyList = paperNotifies,
                MesajNotifyList = mesajNotifies
            };
            return View(model);
        }
    }
}
