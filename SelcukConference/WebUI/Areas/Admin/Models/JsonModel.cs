namespace WebUI.Areas.Admin.Models
{
    public class JsonModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
}
