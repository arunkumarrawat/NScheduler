using System;

namespace NScheduler.Core.Schedules
{
    public abstract class Schedule<TSchedule> : Schedule where TSchedule : Schedule
    {
        protected int reTryAttempts;

        /// <summary>
        /// Sets maximal count of repeats before jobs is un-scheduled
        /// </summary>
        /// <param name="maxRepeats"></param>
        /// <returns></returns>
        public TSchedule SetMaxRepeats(int maxRepeats)
        {
            if (maxRepeats < 0 && maxRepeats != InfiniteRepeats)
            {
                throw new ArgumentException("Maximal count of repeats should be a non-negative value", nameof(maxRepeats));
            }
            this.maxRepeats = maxRepeats;
            return this as TSchedule;
        }

        public virtual TSchedule SetReTryAttempts(int reTry)
        {
            this.reTryAttempts = reTry;
            return this as TSchedule;
        }

        public virtual TSchedule SetInfinite()
        {
            return this as TSchedule;
        }
    }
}
