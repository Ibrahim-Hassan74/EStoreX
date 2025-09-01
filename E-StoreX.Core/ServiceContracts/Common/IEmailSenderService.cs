using EStoreX.Core.DTO.Account.Requests;

namespace EStoreX.Core.ServiceContracts.Common
{
    /// <summary>
    /// Service contract for sending emails.
    /// Provides functionality to send email messages asynchronously.
    /// </summary>
    public interface IEmailSenderService
    {
        /// <summary>
        /// Sends an email using the specified email details.
        /// </summary>
        /// <param name="email">The email data transfer object containing recipient, subject, and body.</param>
        /// <returns>
        /// A task that represents the asynchronous operation of sending the email.
        /// </returns>
        Task SendEmailAsync(EmailDTO email);
    }
}
