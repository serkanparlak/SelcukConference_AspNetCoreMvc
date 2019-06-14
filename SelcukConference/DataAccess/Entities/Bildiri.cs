using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Bildiri
    {
        public Bildiri()
        {
            Ek = new HashSet<Ek>();
            HakemBildiriAtama = new HashSet<HakemBildiriAtama>();
        }

        public int Id { get; set; }
        public int YazarId { get; set; }
        public int EditorId { get; set; }
        [StringLength(250)]
        public string SayfaKimlik { get; set; }
        public string Konu { get; set; }
        public string Aciklama { get; set; }
        public string EditorNotu { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime BildiriTarih { get; set; }
        /*
            2 : Bekliyor 
            3 : Onaylandı
            4 : Revizyon
            5 : Ret
        */
        public int Sonuc { get; set; }

        [ForeignKey("YazarId")]
        [InverseProperty("Bildiri")]
        public Uye Yazar { get; set; }
        [InverseProperty("Bildiri")]
        public ICollection<Ek> Ek { get; set; }
        [InverseProperty("Bildiri")]
        public ICollection<HakemBildiriAtama> HakemBildiriAtama { get; set; }
    }
}
