using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Mesaj
    {
        public int Id { get; set; }
        public int GonderenId { get; set; }
        [StringLength(250)]
        public string Konu { get; set; }
        public string Icerik { get; set; }
        public int AliciId { get; set; }
        public bool Okundu { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime IletimTarihi { get; set; }

        [ForeignKey("AliciId")]
        [InverseProperty("MesajAlici")]
        public Uye Alici { get; set; }
        [ForeignKey("GonderenId")]
        [InverseProperty("MesajGonderen")]
        public Uye Gonderen { get; set; }
    }
}
