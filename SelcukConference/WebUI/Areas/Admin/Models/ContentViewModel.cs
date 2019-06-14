using System.Collections.Generic;
using DataAccess.Entities;

namespace WebUI.Areas.Admin.Models
{
    public class ContentViewModel
    {
        public Menu ActiveMenu { get; internal set; }
        public Menu UstMenu { get; internal set; }
        public Genel GenelData { get; set; }
        public List<Uye> OrgKomites { get; set; }
        public List<Uye> BilKomites { get; set; }
    }
}
