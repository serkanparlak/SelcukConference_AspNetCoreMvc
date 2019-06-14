using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace _Core.MyClasses
{
    public static class StaticOperations
    {
        public static bool MailGonder(string alici, string baslik, string icerik, bool htmlDestegi)
        {
            try
            {
                var aliciMail = new MailAddress(alici);
                var gonderen = new MailAddress("tfselcuk@mail.com", "Selcuk Conference");
                using (var email = new MailMessage())
                {
                    email.From = gonderen;
                    email.To.Add(aliciMail);
                    email.BodyEncoding = Encoding.GetEncoding("ISO-8859-9");
                    email.Subject = baslik;
                    email.Body = icerik;
                    email.IsBodyHtml = htmlDestegi;
                    SmtpClient gonder = new SmtpClient("smtp.yandex.com.tr", 587)
                    {
                        EnableSsl = true,
                        Credentials = new NetworkCredential("tfselcuk@mail.com", "pwd")
                    };
                    gonder.Send(email);
                }
                return true;
            }
            catch
            {
                return false;
            }
            //return true;
        }

        public static string Md5Creator(string input)
        {
            var md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            var sBuilder = new StringBuilder();
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
