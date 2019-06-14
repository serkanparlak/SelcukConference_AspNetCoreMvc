namespace DataAccess.Entities
{
    public class PaperNotify
    {
        public int Id { get; set; }
        public int PaperId { get; set; }
        public int UyeId { get; set; }
        public string BildiriMesaj { get; set; }
        public bool OkunduMu { get; set; }
    }
}
