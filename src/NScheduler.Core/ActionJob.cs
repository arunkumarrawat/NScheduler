using System;

namespace NScheduler.Core
{
    /// <summary>
    /// Special type of <see cref="IJob"/> that wraps <see cref="Action"/> instance to execute
    /// </summary>
    public class ActionJob : IJob
    {
        protected readonly Action<JobContext> Action;
        protected readonly Action SimpleAction;

        public ActionJob(Action action)
        {
            SimpleAction = action;
        }

        public ActionJob(Action<JobContext> action)
        {
            Action = action;
        }

        public virtual void Execute(JobContext context)
        {
            if (Action != null)
            {
                Action(context);
                return;
            }

            SimpleAction?.Invoke();
        }
    }
}