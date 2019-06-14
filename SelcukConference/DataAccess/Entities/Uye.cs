using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Uye
    {
        public Uye()
        {
            Bildiri = new HashSet<Bildiri>();
            KonuEtiketi = new HashSet<KonuEtiketi>();
            MesajAlici = new HashSet<Mesaj>();
            MesajGonderen = new HashSet<Mesaj>();
        }

        public string Nick => Unvan + " " + Adi + " " + Soyadi;

        public int Id { get; set; }
        [StringLength(50)]
        public string Adi { get; set; }
        [StringLength(50)]
        public string Soyadi { get; set; }
        [StringLength(50)]
        public string Sifre { get; set; }
        [StringLength(200)]
        public string Mail { get; set; }
        [StringLength(100)]
        public string Telefon { get; set; }
        public int SehirId { get; set; }
        [StringLength(200)]
        public string Kurum { get; set; }
        public string ProfilYolu { get; set; }
        public int Cinsiyet { get; set; }
        [StringLength(100)]
        public string Unvan { get; set; }
        public string Adres { get; set; }
        public bool Admin { get; set; }
        public bool Editor { get; set; }
        public bool Hakem { get; set; }
        public bool Yazar { get; set; }
        public bool BilimselKomite { get; set; }
        public bool OrganizasyonKomite { get; set; }
        public bool Aktif { get; set; }

        [ForeignKey("SehirId")]
        [InverseProperty("Uye")]
        public Sehir Sehir { get; set; }
        [InverseProperty("Yazar")]
        public ICollection<Bildiri> Bildiri { get; set; }
        [InverseProperty("Editor")]
        public ICollection<KonuEtiketi> KonuEtiketi { get; set; }
        [InverseProperty("Alici")]
        public ICollection<Mesaj> MesajAlici { get; set; }
        [InverseProperty("Gonderen")]
        public ICollection<Mesaj> MesajGonderen { get; set; }
    }
}
