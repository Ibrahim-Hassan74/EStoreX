using EStoreX.Core.BackgroundJobs.Interfaces;
using Hangfire;

namespace EStoreX.Core.BackgroundJobs.Schedulers
{
    public static class JobScheduler
    {
        public static void ScheduleJobs()
        {
            RecurringJob.AddOrUpdate<IEmailJob>(
                "weekly-email-job",
                job => job.SendWeeklyEmailsAsync(null),
                "45 15 * * 2"
            );
        }
    }
}
