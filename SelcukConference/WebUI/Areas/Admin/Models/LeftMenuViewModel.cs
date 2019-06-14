using System.Collections.Generic;
using DataAccess.Entities;

namespace WebUI.Areas.Admin.Models
{
    public class LeftMenuViewModel
    {
        public List<Menu> Menus { get; internal set; }
        public Menu ActiveMenu { get; set; }
        public Uye Oturum { get; set; }
    }
}
