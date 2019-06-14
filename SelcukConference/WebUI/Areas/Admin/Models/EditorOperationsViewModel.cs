using System.Collections.Generic;
using DataAccess.Entities;

namespace WebUI.Areas.Admin.Models
{
    public class EditorOperationsViewModel
    {
        public ICollection<KonuEtiketi> Konular { get; set; }
    }
}