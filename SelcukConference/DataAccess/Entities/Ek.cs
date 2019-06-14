using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Ek
    {
        public int Id { get; set; }
        public string EkYol { get; set; }
        public int BildiriId { get; set; }

        [ForeignKey("BildiriId")]
        [InverseProperty("Ek")]
        public Bildiri Bildiri { get; set; }
    }
}
