using System.Collections.Generic;
using DataAccess.Entities;

namespace WebUI.Models
{
    public class PaperDetailViewModel
    {
        public Bildiri Bildiri { get; set; }
        public Uye Oturum { get; set; }
        public List<Uye> AtananHakemler { get; set; }
        public List<Uye> TumHakemler { get; set; }
        public HakemBildiriAtama HakemBildiri { get; internal set; }
    }
}
