using Hangfire;
using System.Linq.Expressions;

namespace EStoreX.Core.BackgroundJobs.Wrapper
{
    public class BackgroundJobClientWrapper : IBackgroundJobClientWrapper
    {
        public void Enqueue<T>(Expression<Action<T>> methodCall)
        {
            BackgroundJob.Enqueue(methodCall);
        }
    }
}
