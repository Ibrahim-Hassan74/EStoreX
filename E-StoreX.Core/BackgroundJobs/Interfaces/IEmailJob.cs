using Hangfire.Server;

namespace EStoreX.Core.BackgroundJobs.Interfaces
{
    public interface IEmailJob
    {
        Task SendWeeklyEmailsAsync(PerformContext context);
    }

}
