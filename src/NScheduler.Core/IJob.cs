using System.Threading.Tasks;

namespace NScheduler.Core
{
    /// <summary>
    /// Interface of an abstract job to execute
    /// </summary>
    public interface IJob
    {
        Task Execute(JobContext context);
    }
}
