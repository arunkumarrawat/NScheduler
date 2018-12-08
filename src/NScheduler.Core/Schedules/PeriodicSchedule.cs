using System;

namespace NScheduler.Core.Schedules
{
    /// <summary>
    /// Base class for periodic schedules that 
    /// repeat job once per specified period of time
    /// </summary>
    public abstract class PeriodicSchedule<TSchedule> : Schedule<TSchedule>
        where TSchedule : Schedule 
    {
        public const int PresetInterval = 1;

        protected int interval = PresetInterval;
        protected DateTimeOffset? firstFireTime;

        /// <summary>
        /// Gets type of time interval
        /// </summary>
        public abstract TimeInterval Period { get; }

        public sealed override void SetInitialFireTime()
        {
            nextFireTime = createdOn;
            if (firstFireTime != null)
                  nextFireTime = firstFireTime;
        }
    
        public virtual TSchedule SetInterval(int interval)
        {
            if (interval <= 0)
            {
                 throw new ArgumentException("Time interval should be a positive value", nameof(interval));
            }

            this.interval = interval;
            return this as TSchedule;
        }

        /// <summary>
        /// Sets exact date & time of the first fire
        /// </summary>
        /// <param name="fireTime"></param>
        /// <returns></returns>
        public virtual TSchedule SetFirstFireTime(DateTimeOffset fireTime)
        {
            firstFireTime = fireTime;
            return this as TSchedule;
        }

        public virtual TSchedule SetFirstFireTimeTodayAt(int hour, int minute, int second)
        {
            DayTime dt = new DayTime(hour, minute, second);
            firstFireTime = dt.GetAdjustedTime(Time.Now());
            return this as TSchedule;
        }

        public virtual TSchedule SetFirstFireTimeTodayAt(int hour, int minute)
        {
            DayTime dt = new DayTime(hour, minute, second: 0);
            firstFireTime = dt.GetAdjustedTime(Time.Now());
            return this as TSchedule;
        }

        public override void OnMisfired(DateTimeOffset misfireTime, TimeSpan diff)
        {
            if (finalFireTime.HasValue && finalFireTime < misfireTime)
                  nextFireTime = null;

            this.nextFireTime = misfireTime;
        }

        public override DateTimeOffset? CalculateNextFireTime()
        {
            if (!prevFireTime.HasValue)
                return null;
            if (maxRepeats != InfiniteRepeats && maxRepeats == context.TimesRun)
                return null;
            if (finalFireTime.HasValue && finalFireTime < Time.Now())
                return null;

            DateTimeOffset result = prevFireTime.Value;

            switch (Period)
            {
                case TimeInterval.Seconds:
                    result = result.AddSeconds(interval);
                    break;

                case TimeInterval.Minutes:
                    result = result.AddMinutes(interval);
                    break;

                case TimeInterval.Hours:
                    result = result.AddHours(interval);
                    break;

                case TimeInterval.Days:
                    result = result.AddDays(interval);
                    break;

                case TimeInterval.Weeks:
                    result = result.AddDays(7 * interval);
                    break;

                case TimeInterval.Months:
                    result = result.AddMonths(interval);
                    break;

                case TimeInterval.Years:
                    result = result.AddYears(interval);
                    break;
            }

            return result;
        }
    }
}
