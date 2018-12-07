using System;

namespace NScheduler.Core.Schedules
{
    public abstract class Schedule<TSchedule> : Schedule where TSchedule : Schedule
    {
        public const int InfiniteRepeats = -1;

        protected int maxRepeats = InfiniteRepeats;
        protected DateTimeOffset? finalFireTime;

        /// <summary>
        /// Sets maximal count of repeats before jobs is un-scheduled
        /// </summary>
        /// <param name="maxRepeats"></param>
        /// <returns></returns>
        public virtual TSchedule SetMaxRepeats(int maxRepeats)
        {
            if (maxRepeats < 0 && maxRepeats != InfiniteRepeats)
            {
                throw new ArgumentException("Maximal count of repeats should be a non-negative value", nameof(maxRepeats));
            }
            this.maxRepeats = maxRepeats;
            return this as TSchedule;
        }

        public virtual TSchedule SetFinalFireTime(DateTimeOffset finalFireTime)
        {
            this.finalFireTime = finalFireTime;
            return this as TSchedule;
        }

        public virtual TSchedule SetReTryAttempts(int reTry)
        {
            reTryAttempts = reTry;
            return this as TSchedule;
        }

        public virtual TSchedule SetInfinite()
        {
            maxRepeats = InfiniteRepeats;
            finalFireTime = null;
            return this as TSchedule;
        }
    }
}
