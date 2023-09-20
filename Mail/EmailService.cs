using System.Net;
using System.Net.Mail;
namespace employeeDailyTaskRecorder.Mail
{
    public static class EmailService
    {
        public static async Task SendEmailAsync(string to, string subject, string emailBody)
        {
            var smtpServer = "mail.antsware.com";
            var smtpPort = 26;
            var smtpUsername = "webmaster@antsware.com";
            var smtpPassword = "webAdmin@101";

            var smtpClient = new SmtpClient
            {
                Host = smtpServer,
                Port = smtpPort,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpUsername),
                Subject = subject,
                Body = emailBody,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}

