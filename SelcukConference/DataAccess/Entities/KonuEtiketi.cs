using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class KonuEtiketi
    {
        public int Id { get; set; }
        [StringLength(200)]
        public string EtiketAdiTr { get; set; }
        [StringLength(200)]
        public string EtiketAdiEng { get; set; }
        public int EditorId { get; set; }

        [ForeignKey("EditorId")]
        [InverseProperty("KonuEtiketi")]
        public Uye Editor { get; set; }
    }
}
