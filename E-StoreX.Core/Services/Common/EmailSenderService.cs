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
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("E-StoreX", _senderDetails.Email));
            message.To.Add(new MailboxAddress(email.Email, email.Email));
            message.Subject = email.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = email.HtmlMessage
            };

            if (email.Attachments != null && email.Attachments.Any())
            {
                foreach (var attachment in email.Attachments)
                {
                    builder.Attachments.Add(
                        attachment.FileName,
                        attachment.FileBytes,
                        ContentType.Parse(attachment.ContentType)
                    );
                }
            }

            message.Body = builder.ToMessageBody();

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                await smtp.ConnectAsync(_senderDetails.Host, _senderDetails.Port, true);
                await smtp.AuthenticateAsync(_senderDetails.Email, _senderDetails.Password);

                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
            }
        }


    }
}

