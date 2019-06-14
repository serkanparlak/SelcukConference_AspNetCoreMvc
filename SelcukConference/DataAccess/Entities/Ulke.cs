using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Ulke
    {
        public Ulke()
        {
            Sehir = new HashSet<Sehir>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string UlkeAdi { get; set; }

        [InverseProperty("Ulke")]
        public ICollection<Sehir> Sehir { get; set; }
    }
}
