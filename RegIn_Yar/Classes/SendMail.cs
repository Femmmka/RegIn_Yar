﻿using System.Net;
using System.Net.Mail;

namespace RegIn_Yar.Classes
{
    public class SendMail
    {
        public static void SendMessage(string Message, string To)
        {
            var smtpClient = new SmtpClient("smtp.yandex.ru")
            {
                Port = 587,
                Credentials = new NetworkCredential("yandex@yandex.ru", "jvdfmebuouuiesyg"),
                EnableSsl = true,
            };
            smtpClient.Send("qwerty@yandex.ru", To, "Проект RegIn", Message);
        }
    }
}