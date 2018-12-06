﻿using System;

namespace NScheduler.Core.Schedules
{
    public abstract class PeriodicSchedule : Schedule
    {
        public const int DefaultInterval = 1;

        protected int interval;

        protected PeriodicSchedule()
        {
            this.interval = DefaultInterval;
        }

        public PeriodicSchedule SetInterval(int interval)
        {
            if (interval <= 0)
            {
                throw new ArgumentException("Time interval should be a positive value", nameof(interval));
            }

            this.interval = interval;
            return this;
        }

        internal override DateTimeOffset? CalculateNextFireTime()
        {
            DateTimeOffset now = DateTimeOffset.Now;

            if (!scheduledFireTime.HasValue)
                return null;
            if (finalFireTime.HasValue && finalFireTime < now)
                return null;
            if (maxRepeats != InfiniteRepeats && maxRepeats == context.TimesRun)
                return null;

            DateTimeOffset nextFireTime = scheduledFireTime.Value;

            switch (Period)
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

        protected abstract TimeInterval Period { get; }
    }
}