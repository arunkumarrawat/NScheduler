using System;
using System.Threading.Tasks;

namespace NScheduler.Core
{
    /// <summary>
    /// Special type of <see cref="IJob"/> that adapts <see cref="Action"/> delegates instance to execute
    /// </summary>
    public class ActionJob : IJob
    {
        public static ActionJob Action(Action action) => new ActionJob(action);

        public static ActionJob Action(Action<JobContext> action) => new ActionJob(action);

        protected readonly Action<JobContext> action;
        protected readonly Action simpleAction;

        public ActionJob(Action<JobContext> action)
        {
            this.action = action;
        }

        public ActionJob(Action action)
        {
            this.simpleAction = action;
        }

        public virtual Task Execute(JobContext context)
        {
            if (action != null)
            {
                action(context);
                return Task.CompletedTask;
            }

            simpleAction?.Invoke();
            return Task.CompletedTask;
        }
    }
}