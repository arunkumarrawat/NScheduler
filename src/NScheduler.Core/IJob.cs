namespace NScheduler.Core
{
    /// <summary>
    /// Interface of an abstract job to execute
    /// </summary>
    public interface IJob
    {
        void Execute(JobContext context);
    }
}
