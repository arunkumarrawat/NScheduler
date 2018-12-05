using System;

namespace NScheduler.Core
{
    public class JobSchedule
    {
        private DateTime? finalFireTime;
        private DateTime? nextFireTime;
        private int interval;
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

        internal void OnJobTriggered(DateTime now)
        {
            if (finalFireTime.HasValue && finalFireTime <= now)
            {
                nextFireTime = null;
                return;
            }

            var next = nextFireTime.Value;

            switch(span)
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
            }
        }

        private DateTime Now()

        {
            if (timeService != null)
                  return timeService.Now();
            return DateTime.Now;
        }

        /// <summary>
        /// Gets exact date & time of next fire 
        /// </summary>
        public DateTime? GetNextFireTime() => nextFireTime;

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
