using System.Collections.Generic;
using DataAccess.Entities;

namespace WebUI.Models
{
    public class HeaderViewModel
    {
        public Uye Oturum { get; set; }
        public List<PaperNotify> PaperNotifyList { get; set; }
        public List<MessageNotify> MesajNotifyList { get; internal set; }
    }
}
