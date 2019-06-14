using System;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using WebUI.Areas.Admin.Models;

namespace WebUI.Areas.Admin.Components
{
    public class LeftMenuAdminViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;
        
        public LeftMenuAdminViewComponent(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public ViewViewComponentResult Invoke()
        {
            var pageId = Int32.TryParse(Request.Query["page"], out _) ? Convert.ToInt32(Request.Query["page"]) : 0;
            var activeMenu = pageId == 0 ? null : _menuService.GetMenu(pageId);
            var model = new LeftMenuViewModel
            {
                Menus = _menuService.SiraliListe(),
                ActiveMenu = activeMenu
            };
            return View(model);
        }
    }
}
