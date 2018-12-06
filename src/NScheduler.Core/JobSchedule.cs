using System;

namespace NScheduler.Core
{
    public class JobSchedule
    {
        public const int InfiniteRepeats = -1;

        private DateTime? scheduledFireTime;
        private DateTime? previousFireTime;
        private DateTime? finalFireTime;
        private int interval;
        private int maxRepeats = InfiniteRepeats;
        private TimeInterval span;
        private ITimeService timeService;

        public JobSchedule()
        {
            scheduledFireTime = DateTime.Now;
            span = TimeInterval.Hours;
            interval = 1;
        }

        public JobSchedule(ITimeService timeService)
        {
            this.timeService = timeService;
            scheduledFireTime = timeService.Now();
            span = TimeInterval.Hours;
            interval = 1;
        }

        internal void SetNextFireTime()
        {
            this.scheduledFireTime = CalculateNextFireTime();
        }

        internal DateTime? CalculateNextFireTime()
        {
            if (!scheduledFireTime.HasValue)
                 return null;

            DateTime nextFireTime = scheduledFireTime.Value;

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
        public DateTime? GetScheduledFireTime() => scheduledFireTime;

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
