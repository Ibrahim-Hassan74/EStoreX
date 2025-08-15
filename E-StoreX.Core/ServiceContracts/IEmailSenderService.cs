using EStoreX.Core.DTO.Account.Requests;

namespace EStoreX.Core.ServiceContracts
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(EmailDTO email);
    }
}
