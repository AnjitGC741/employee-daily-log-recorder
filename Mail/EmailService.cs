using System.Net;
using System.Net.Mail;
namespace employeeDailyTaskRecorder.Mail
{
    public static class EmailService

    {
      
        public static async  Task SendEmailAsync(IConfiguration configuration,string emailBody,string? receiverEmail, string subject)
        {

            var emailSettings = configuration.GetSection("EmailSettings");

            var smtpServer = emailSettings["SmtpServer"];
            var smtpPort = Convert.ToInt32(emailSettings["SmtpPort"]);
            var smtpUsername = emailSettings["SmtpUsername"];
            var smtpPassword = emailSettings["SmtpPassword"];
            var smtpReceiver = emailSettings["SmtpReceiver"];
            var smtpFrom = emailSettings["SmtpFrom"];
            bool smtpPoverSsl = bool.Parse(emailSettings["SmtpPoverSsl"]);
            var smtpClient = new SmtpClient
            {
                Host = smtpServer,
                Port = smtpPort,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = smtpPoverSsl,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpFrom),
                Subject = subject,
                Body = emailBody,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(receiverEmail == null ?  smtpReceiver : receiverEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}

