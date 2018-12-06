using System;

namespace NScheduler.Core
{
    public class JobSchedule
    {
        public const int InfiniteRepeats = -1;

        private DateTime? nextFireTime;
        private DateTime? previousFireTime;
        private DateTime? finalFireTime;
        private int interval;
        private int maxRepeats = InfiniteRepeats;
        private TimeInterval span;
        private ITimeService timeService;

        public JobSchedule()
        {
            nextFireTime = DateTime.Now;
            span = TimeInterval.Hours;
            interval = 1;
        }

        public JobSchedule(ITimeService timeService)
        {
            this.timeService = timeService;
            nextFireTime = timeService.Now();
            span = TimeInterval.Hours;
            interval = 1;
        }

        internal void CalculateNextFireTime(DateTime now)
        {
            previousFireTime = nextFireTime;
            if (finalFireTime.HasValue && finalFireTime <= now)
            {
                nextFireTime = null;
                return;
            }

            var next = nextFireTime.Value;

            switch (span)
            {
                case TimeInterval.Seconds:
                    nextFireTime = next.AddSeconds(interval);
                    break;
                case TimeInterval.Minutes:
                    nextFireTime = next.AddMinutes(interval);
                    break;
                case TimeInterval.Hours:
                    nextFireTime = next.AddHours(interval);
                    break;
                case TimeInterval.Days:
                    nextFireTime = next.AddDays(interval);
                    break;
                case TimeInterval.Weeks:
                    nextFireTime = next.AddDays(7 * interval);
                    break;
                case TimeInterval.Months:
                    nextFireTime = next.AddMonths(interval);
                    break;
                case TimeInterval.Years:
                    nextFireTime = next.AddYears(interval);
                    break;
            }
        }

        private DateTime Now()
        {
            if (timeService != null)
                  return timeService.Now();
            return DateTime.Now;
        }

        /// <summary>
        /// Gets exact date & time of the next fire 
        /// </summary>
        public DateTime? GetNextFireTime() => nextFireTime;

        /// <summary>
        /// Gets exact date & time of the previous fire 
        /// </summary>
        public DateTime? GetPreviousFireTime() => previousFireTime;

        public JobSchedule SetRepeatInterval(int interval, TimeInterval span)
        {
            this.interval = interval;
            this.span = span;
            return this;
        }

        public JobSchedule Clone()
        {
            JobSchedule clone = new JobSchedule();
            return clone;
        }
    }
}
