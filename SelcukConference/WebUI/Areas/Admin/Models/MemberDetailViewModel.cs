using System.Collections.Generic;
using DataAccess.Entities;

namespace WebUI.Areas.Admin.Models
{
    public class MemberDetailViewModel
    {
        public Uye Uye { get; internal set; }
        public List<Ulke> Ulkeler { get; set; }
        public List<Sehir> Sehirler { get; internal set; }
    }
}
