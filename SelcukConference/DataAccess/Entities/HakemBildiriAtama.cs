using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class HakemBildiriAtama
    {
        public int Id { get; set; }
        public int BildiriId { get; set; }
        public int HakemId { get; set; }
        public string HakemNotu { get; set; }
        /*
            2 : Bekliyor 
            3 : Onaylandı
            4 : Revizyon
            5 : Ret
        */
        public int Sonuc { get; set; }

        [ForeignKey("BildiriId")]
        [InverseProperty("HakemBildiriAtama")]
        public Bildiri Bildiri { get; set; }
    }
}
