using System.Collections.Generic;
using DataAccess.Entities;

namespace WebUI.Models
{
    public class ChatMessagesViewModel
    {
        public List<Mesaj> Messages { get;  set; }
        public Uye Gonderen { get;  set; }
    }
}
