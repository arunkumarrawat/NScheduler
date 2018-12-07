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

        public int ReTryAttempts => reTryAttempts;


        protected Schedule()
        {
            scheduledFireTime = DateTimeOffset.Now;
        }

        /// <summary>
        /// Constructor internally used for cloning
        /// </summary>
        /// <param name="schedule"></param>
        protected internal Schedule(Schedule schedule)
        {
        }

        internal void SetContext(JobContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Sets exact date & time of the next fire
        /// </summary>
        internal void SetNextFireTime()
        {
            this.scheduledFireTime = CalculateNextFireTime();
        }

        /// <summary>
        /// Calculates exact date & time of the next fire
        /// </summary>
        /// <returns></returns>
        internal abstract DateTimeOffset? CalculateNextFireTime();
        
        /// <summary>
        /// Gets exact date & time of scheduled fire 
        /// </summary>
        public DateTimeOffset? GetScheduledFireTime() => scheduledFireTime;

        /// <summary>
        /// Sets maximal count of repeats before jobs is un-scheduled
        /// </summary>
        /// <param name="maxRepeats"></param>
        /// <returns></returns>
        public Schedule SetMaxRepeats(int maxRepeats)
        {
            if (maxRepeats < 0 && maxRepeats != InfiniteRepeats)
            {
                throw new ArgumentException("Maximal count of repeats should be a non-negative value", nameof(maxRepeats));
            }
            this.maxRepeats = maxRepeats;
            return this;
        }

        public Schedule SetFinalFireTime(DateTimeOffset finalTime)
        {
            this.finalFireTime = finalTime;
            return this;
        }
    }
}
