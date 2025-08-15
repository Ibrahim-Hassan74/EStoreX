using MimeKit;
using Microsoft.Extensions.Options;
using EStoreX.Core.Domain.Options;
using EStoreX.Core.DTO.Account.Requests;
using EStoreX.Core.ServiceContracts.Common;


namespace EStoreX.Core.Services.Common
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailSetting _senderDetails;
        public EmailSenderService(IOptions<EmailSetting> senderDetails)
        {
            _senderDetails = senderDetails.Value;
        }

        public async Task SendEmailAsync(EmailDTO email)
        {
            MimeMessage message = new();

            message.From.Add(new MailboxAddress("E-StoreX", _senderDetails.Email));
            message.Subject = email.Subject;
            message.To.Add(new MailboxAddress(email.Email, email.Email));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = email.HtmlMessage
            };

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                await smtp.ConnectAsync(_senderDetails.Host, _senderDetails.Port, true);
                await smtp.AuthenticateAsync(_senderDetails.Email, _senderDetails.Password);

                await smtp.SendAsync(message);
                smtp.Disconnect(true);
            }
        }

    }
}

