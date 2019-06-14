using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Genel
    {
        public string BannerYol { get; set; }
        public string SimgeYol { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? KonferanTarihi { get; set; }
        public int Id { get; set; }
    }
}
