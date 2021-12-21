using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace HangFireSample.Models
{
    public class MailHelper
    {
        public void SendMail(string username, string subject, string mailBody)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("hangfiresample@gmail.com");
                mail.To.Add(username);

                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = mailBody;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new NetworkCredential("hangfiresample@gmail.com", "hangfire123456");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void NotifySystemAdmin()
        {
            Console.WriteLine("Continuations will start soon...");
        }
    }
}
