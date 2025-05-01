using System;

namespace SufraX.Core.ServiceContracts
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
