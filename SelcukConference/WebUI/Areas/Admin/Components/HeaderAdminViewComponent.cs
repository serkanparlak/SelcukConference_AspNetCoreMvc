using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace WebUI.Areas.Admin.Components
{
    public class HeaderAdminViewComponent : ViewComponent
    {

        public ViewViewComponentResult Invoke()
        {
            return View();
        }

    }
}
