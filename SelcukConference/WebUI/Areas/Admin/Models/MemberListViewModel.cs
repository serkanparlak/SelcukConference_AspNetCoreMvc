using System.Collections.Generic;
using DataAccess.Entities;

namespace WebUI.Areas.Admin.Models
{
    public class MemberListViewModel
    {
        public List<Uye> UyeList { get; set; }
        public MemberSearchModel MemberSearch { get; set; }
    }
}
