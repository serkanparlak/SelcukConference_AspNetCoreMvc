using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Sehir
    {
        public Sehir()
        {
            Uye = new HashSet<Uye>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string SehirAdi { get; set; }
        public int UlkeId { get; set; }

        [ForeignKey("UlkeId")]
        [InverseProperty("Sehir")]
        public Ulke Ulke { get; set; }
        [InverseProperty("Sehir")]
        public ICollection<Uye> Uye { get; set; }
    }
}
