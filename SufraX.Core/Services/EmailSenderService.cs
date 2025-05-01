using Microsoft.Extensions.Options;
using SufraX.Core.Domain.Options;
using SufraX.Core.ServiceContracts;
using System.Net.Mail;
using System.Net;


namespace SufraX.Core.Services
{
    public class EmailSenderService : IEmailSender
    {
        private readonly AccountSenderDetails _senderDetails;
        public EmailSenderService(IOptions<AccountSenderDetails> senderDetails)
        {
            _senderDetails = senderDetails.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpClient = new SmtpClient(_senderDetails.Host, _senderDetails.Port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_senderDetails.Email, _senderDetails.Password)
            };

            var message = new MailMessage(_senderDetails.Email!, email)
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
                BodyEncoding = System.Text.Encoding.UTF8,
                SubjectEncoding = System.Text.Encoding.Default,
            };

            message.ReplyToList.Add(_senderDetails.Email!);

            await smtpClient.SendMailAsync(message);

        }
    }
}
