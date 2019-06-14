using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Menu
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string MenuAdi { get; set; }
        [StringLength(50)]
        public string MenuAdiEng { get; set; }
        public string SayfaIcerik { get; set; }
        public string SayfaIcerikEng { get; set; }
        public int ListeSira { get; set; }
        public int AltMenuId { get; set; }
    }
}
