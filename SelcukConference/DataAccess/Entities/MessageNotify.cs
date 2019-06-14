namespace DataAccess.Entities
{
    public class MessageNotify
    {
        public int TotalUnread { get; internal set; }
        public Mesaj LastMessage { get; internal set; }
    }
}
