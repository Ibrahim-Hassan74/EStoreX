using EStoreX.Core.DTO;
using System;

namespace EStoreX.Core.ServiceContracts
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(EmailDTO email);
    }
}
