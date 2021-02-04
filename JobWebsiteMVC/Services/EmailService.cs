using Microsoft.AspNetCore.Identity.UI.Services;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace JobWebsiteMVC.Services
{
    public class EmailService : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.Run(() =>
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(email);
                mailMessage.From = new MailAddress("test@blah.com");
                mailMessage.Body = htmlMessage;
                mailMessage.Subject = subject;

                var client = new SmtpClient("localhost", 25);
                client.UseDefaultCredentials = true;
                //client.Credentials = new NetworkCredential("username", "password");
                client.Send(mailMessage);
            });
        }

        private void WriteEmailToText(MailMessage message)
        {
            string path = "../RegistrationEmails.txt";
            using (StreamWriter w = File.AppendText(path))
            {
                w.WriteLine("To: ", message.To.Select(x => x.Address));
                w.WriteLine("From: ", message.From.Address);
                w.WriteLine("Subject: ", message.Subject.ToString());
                w.WriteLine(message.Body.ToString());
                w.WriteLine("");
            }
        }
    }
}