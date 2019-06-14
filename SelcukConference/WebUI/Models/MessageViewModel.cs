using System.Collections.Generic;
using DataAccess.Entities;

namespace WebUI.Models
{
    public class MessageViewModel
    {
        public List<MessageNotify> SonMesajUyeler { get; set; }
        public List<Uye> MesajsizUyeler { get; set; }
        public Uye Oturum { get; set; }
        public Uye MesajAliciUye { get; set; }
    }
}