using System;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using WebUI.Areas.Admin.Models;
using WebUI.Services;

namespace WebUI.Components
{
    public class LeftMenuUserViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;
        private readonly ISessionService _sessionService;

        public LeftMenuUserViewComponent(IMenuService menuService, ISessionService sessionService)
        {
            _menuService = menuService;
            _sessionService = sessionService;
        }

        public ViewViewComponentResult Invoke()
        {
            var pageId = Int32.TryParse(Request.Query["page"], out _) ? Convert.ToInt32(Request.Query["page"]) : 0;
            var activeMenu = pageId == 0 ? null : _menuService.GetMenu(pageId);
            var model = new LeftMenuViewModel
            {
                Menus = _menuService.SiraliListe(),
                ActiveMenu = activeMenu,
                Oturum = _sessionService.GetUyeSession()
            };
            return View(model);
        }
    }
}
