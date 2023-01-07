using Demo.DAL.Entities;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("samir.dagher10@gmail.com", "jhgvjlgyfrenubbf");
            client.Send("samir.dagher10@gmail.com", email.To, email.Subject, email.Body);
        }
    }
}
