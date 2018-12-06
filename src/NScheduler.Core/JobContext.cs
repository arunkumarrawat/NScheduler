using System;

namespace NScheduler.Core
{
    /// <summary>
    /// Execution context of an <see cref="IJob"/> instance
    /// </summary>
    public sealed class JobContext
    {
        private DateTime? previousFireTime;
        private Exception lastException;
        private int timesRun;
        private int timesFaulted;
        private Type jobType; 
    
        internal void OnJobExecuted(JobHolder jh)
        {
            timesRun++;
            jobType = jh.Job.GetType();
            previousFireTime = jh.Schedule.GetPreviousFireTime();
        }

        internal void OnJobFaulted(Exception ex)
        {

        }

        /// <summary>
        /// Gets count of times the task Job was executed
        /// </summary>
        public int TimesRun => timesRun;

        public Type JobType => jobType;
    }
}
