using System.Net;
using System.Net.Mail;

namespace RegIn_Yar.Classes
{
    public class SendMail
    {
        public static void SendMessage(string Message, string To)
        {
            SmtpClient smtpClient = new SmtpClient
            {
                Host = "smtp.yandex.ru",
                Port = 587,
                Credentials = new NetworkCredential("Femkaaaaa@yandex.ru", "yrzggtzozowvqmcs"),
                EnableSsl = true,
            };

            smtpClient.Send("Femkaaaaa@yandex.ru", To, "Project RegIn", Message);
        }
    }
}
