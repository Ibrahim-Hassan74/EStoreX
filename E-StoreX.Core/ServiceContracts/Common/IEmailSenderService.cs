using EStoreX.Core.DTO.Account.Requests;

namespace EStoreX.Core.ServiceContracts.Common
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(EmailDTO email);
    }
}
