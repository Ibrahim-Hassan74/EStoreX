using System.Linq.Expressions;

namespace EStoreX.Core.BackgroundJobs.Wrapper
{
    public interface IBackgroundJobClientWrapper
    {
        void Enqueue<T>(Expression<Action<T>> methodCall);
    }
}
