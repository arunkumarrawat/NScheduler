using System;

namespace NScheduler.Core
{
    /// <summary>
    /// Execution context of an <see cref="IJob"/> instance
    /// </summary>
    public sealed class JobContext
    {
        private DateTimeOffset? previousFireTime;
        private Exception lastError;
        private int timesRun;
        private int timesFaulted;
        private int reTry;
    
        internal void OnJobExecuted(JobHolder jh)
        {
            reTry = 0;
            lastError = null;
            timesRun++;
            previousFireTime = jh.Schedule.GetScheduledFireTime();
            jh.Schedule.SetNextFireTime();
        }

        internal void OnJobFaulted(Exception ex, JobHolder jh)
        {
            reTry = 0;
            lastError = ex;
            timesFaulted++;
            jh.Schedule.SetNextFireTime();
        }

        /// <summary>
        /// Gets count of times the task Job was executed
        /// </summary>
        public int TimesRun => timesRun;

        public int TimesFaulted => timesFaulted;

        /// <summary>
        /// Gets last exception from previous re-try
        /// </summary>
        public Exception LastError => lastError;

        /// <summary>
        /// Gets current re-try attempt
        /// </summary>
        public int ReTryAttempt => reTry;

        internal int IncrementReTry()
        {
            return ++this.reTry;
        }

        internal void SetLastError(Exception lastError)
        {
            this.lastError = lastError;
        }
    }
}
