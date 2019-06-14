using System.Collections.Generic;
using DataAccess.Entities;

namespace WebUI.Models
{
    public class PaperViewModel
    {
        public List<Bildiri> Bildirilerim { get; set; }
        public List<Bildiri> EditorBildirileri { get; set; }
        public List<Bildiri> HakemBildirileri { get; set; }
        public Uye Oturum { get; set; }
    }
}