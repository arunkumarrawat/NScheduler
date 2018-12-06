using System;

namespace NScheduler.Core
{
    /// <summary>
    /// Execution context of an <see cref="IJob"/> instance
    /// </summary>
    public sealed class JobContext
    {
        private DateTimeOffset? previousFireTime;
        private Exception lastException;
        private int timesRun;
        private int timesFaulted;
    
        internal void OnJobExecuted(JobHolder jh)
        {
            lastException = null;
            timesRun++;
            previousFireTime = jh.Schedule.GetScheduledFireTime();
            jh.Schedule.SetNextFireTime();
        }

        internal void OnJobFaulted(Exception ex, JobHolder jh)
        {
            timesFaulted++;
            jh.Schedule.SetNextFireTime();
        }

        /// <summary>
        /// Gets count of times the task Job was executed
        /// </summary>
        public int TimesRun => timesRun;

        public int TimesFaulted => timesFaulted;
    }
}
