using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Blogbaster.Helpers
{
    public static class EmailHelper
    {
        public static async Task SendEmail(string toAddress, string subject, string body)
        {
            var senderEmail = ConfigurationManager.AppSettings["CredenteisForEmailAnnounce_Email"];
            var senderPass = ConfigurationManager.AppSettings["CredenteisForEmailAnnounce_Password"];


            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(senderEmail, senderPass),
                EnableSsl = true
            };
            var mail = new MailMessage();

            mail.From = new MailAddress(senderEmail, "Article For Every One.");
            mail.To.Add(new MailAddress(toAddress));
            mail.CC.Add(new MailAddress(senderEmail));
            mail.IsBodyHtml = true;
            mail.Body = body;
            mail.Subject = subject;

            await smtpClient.SendMailAsync(mail);
        }
    }
}
