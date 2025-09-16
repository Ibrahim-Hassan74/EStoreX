using Hangfire.Server;

namespace EStoreX.Core.BackgroundJobs.Interfaces
{
    public interface IEmailJob
    {
        Task SendWeeklyEmailsAsync(PerformContext context);
        Task SendOrderConfirmationEmailAsync(Guid orderId, PerformContext context);
        Task SendPaymentFailedEmailAsync(Guid orderId, PerformContext context);
    }

}
