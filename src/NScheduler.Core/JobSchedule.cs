using System;

namespace NScheduler.Core
{
    public class JobSchedule
    {
        private DateTime startsOn;
        private DateTime? finalFireTime;
        private DateTime? lastFireTime;
        private int interval;
        private TimeInterval span;
        private ITimeService timeService;

        public JobSchedule()
        {
            startsOn = DateTime.Now;
            span = TimeInterval.Hours;
            interval = 1;
        }

        public JobSchedule(ITimeService timeService)
        {
            this.timeService = timeService;
            startsOn = timeService.Now();
            span = TimeInterval.Hours;
            interval = 1;
        }

        internal void Refresh(DateTime lastFireTime)
        {
            this.lastFireTime = lastFireTime;
        }

        private DateTime Now()
        {
            if (timeService != null)
                  return timeService.Now();
            return DateTime.Now;
        }

        public DateTime? GetNextFireTime() => GetNextFireTime(Now());

        /// <summary>
        /// Gets exact date & time of next fire 
        /// </summary>
        public DateTime? GetNextFireTime(DateTime now)
        {
            if (now <= startsOn)
                  return startsOn;

            if (finalFireTime.HasValue && finalFireTime <= now)
                  return null;

            if (lastFireTime.HasValue)
            {
                 var next = lastFireTime.Value;
                 switch (span)
                 {
                    case TimeInterval.Seconds:
                        next = next.AddSeconds(interval);
                        break;
                    case TimeInterval.Minutes:
                        next = next.AddMinutes(interval);
                        break;
                    case TimeInterval.Hours:
                        next = next.AddHours(interval);
                        break;
                    case TimeInterval.Days:
                        next = next.AddDays(interval);
                        break;
                    case TimeInterval.Weeks:
                        next = next.AddDays(7 * interval);
                        break;
                    case TimeInterval.Months:
                        next = next.AddMonths(interval);
                        break;
                    case TimeInterval.Years:
                        next = next.AddYears(interval);
                        break;
                 }

                 return next;
            }

            return now;
        }

        public JobSchedule SetStartTime(DateTime dt)
        {
            startsOn = dt;
            return this;
        }

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
