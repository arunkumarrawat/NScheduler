using System;

namespace NScheduler.Core.Schedules
{
    /// <summary>
    /// Base class defining <see cref="IJob"/>'s runtime schedule
    /// </summary>
    public abstract class Schedule
    {
        public const int InfiniteRepeats = -1;

        protected DateTimeOffset? scheduledFireTime;
        protected JobContext context;
        protected DateTimeOffset? finalFireTime;
        protected int maxRepeats = InfiniteRepeats;
        protected int reTryAttempts;

        /// <summary>
        /// Sets job's context
        /// </summary>
        /// <param name="context"></param>
        internal void SetContext(JobContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Sets exact date & time of the next fire
        /// </summary>
        internal void SetNextFireTime()
        {
            scheduledFireTime = CalculateNextFireTime();
        }


        /// <summary>
        /// Allows a schedule to prepare/initialize itself before adding to the 
        /// queue of running jobs
        /// </summary>
        public virtual void Prepare()
        {
            scheduledFireTime = DateTimeOffset.Now;
        }

        /// <summary>
        /// Calculates exact date & time of the next fire
        /// </summary>
        /// <returns></returns>
        public abstract DateTimeOffset? CalculateNextFireTime();
        
        /// <summary>
        /// Gets exact date & time of scheduled fire 
        /// </summary>
        public DateTimeOffset? GetScheduledFireTime() => scheduledFireTime;

        /// <summary>
        /// Gets max count of re-try attempts before job is considered faulted
        /// </summary>
        public int ReTryAttempts => reTryAttempts;

        public Schedule SetFinalFireTime(DateTimeOffset finalTime)
        {
            this.finalFireTime = finalTime;
            return this;
        }
    }
}
