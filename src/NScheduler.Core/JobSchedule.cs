using System;

namespace NScheduler.Core
{
    public class JobSchedule
    {
        public const int InfiniteRepeats = -1;

        private DateTimeOffset? scheduledFireTime;
        private DateTimeOffset? finalFireTime;
        private int interval;
        private int maxRepeats = InfiniteRepeats;
        private int reTryAttempts;
        private TimeInterval span;
        private JobContext context;

        public int ReTryAttempts => reTryAttempts;

        public JobSchedule()
        {
            scheduledFireTime = DateTime.Now;
            span = TimeInterval.Hours;
            interval = 1;
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

        internal DateTimeOffset? CalculateNextFireTime()
        {
            DateTimeOffset now = DateTimeOffset.Now;

            if (!scheduledFireTime.HasValue)
                  return null;

            if (maxRepeats != InfiniteRepeats && context.TimesRun == maxRepeats)
                  return null;

            if (finalFireTime.HasValue && finalFireTime < now)
                  return null;

            DateTimeOffset nextFireTime = scheduledFireTime.Value;

            switch (span)
            {
                case TimeInterval.Seconds:
                    nextFireTime = nextFireTime.AddSeconds(interval);
                    break;
                case TimeInterval.Minutes:
                    nextFireTime = nextFireTime.AddMinutes(interval);
                    break;
                case TimeInterval.Hours:
                    nextFireTime = nextFireTime.AddHours(interval);
                    break;
                case TimeInterval.Days:
                    nextFireTime = nextFireTime.AddDays(interval);
                    break;
                case TimeInterval.Weeks:
                    nextFireTime = nextFireTime.AddDays(7 * interval);
                    break;
                case TimeInterval.Months:
                    nextFireTime = nextFireTime.AddMonths(interval);
                    break;
                case TimeInterval.Years:
                    nextFireTime = nextFireTime.AddYears(interval);
                    break;
            }

            return nextFireTime;
        }

        /// <summary>
        /// Gets exact date & time of scheduled fire 
        /// </summary>
        public DateTimeOffset? GetScheduledFireTime() => scheduledFireTime;

        public JobSchedule SetRepeatInterval(int interval, TimeInterval span)
        {
            this.interval = interval;
            this.span = span;
            return this;
        }

        public JobSchedule SetReTryAttempts(int reTry)
        {
            if (reTry < 0)
            {
                throw new ArgumentException("Count of re-try attempts should be a non-negative value", nameof(reTry));
            }

            this.reTryAttempts = reTry;
            return this;
        }

        /// <summary>
        /// Sets schedule as infinite 
        /// </summary>
        /// <returns></returns>
        public JobSchedule SetInfinite()
        {
            this.maxRepeats = InfiniteRepeats;
            this.finalFireTime = null;
            return this;
        }

        /// <summary>
        /// Sets maximal count of repeats before jobs is un-scheduled
        /// </summary>
        /// <param name="maxRepeats"></param>
        /// <returns></returns>
        public JobSchedule SetMaxRepeats(int maxRepeats)
        {
            if (maxRepeats < 0 && maxRepeats != InfiniteRepeats)
            {
                throw new ArgumentException("Maximal count of repeats should be a non-negative value", nameof(maxRepeats));
            }
            this.maxRepeats = maxRepeats;
            return this;
        }

        public JobSchedule SetFinalFireTime(DateTimeOffset finalTime)
        {
            this.finalFireTime = finalTime;
            return this;
        }

        public JobSchedule Clone()
        {
            JobSchedule clone = new JobSchedule();
            clone.context = this.context;
            clone.finalFireTime = this.finalFireTime;
            clone.interval = this.interval;
            clone.scheduledFireTime = this.scheduledFireTime;
            clone.span = this.span;
            clone.reTryAttempts = this.reTryAttempts;
            return clone;
        }
    }
}
