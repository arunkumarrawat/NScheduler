using System;

namespace NScheduler.Core.Schedules
{
    /// <summary>
    /// Daily schedule with fires at specified time of day
    /// </summary>
    public class DailySchedule : Schedule<DailySchedule>
    {
        private DayTime dayTime = DayTime.ZeroTime;

        public override void Initialze()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            nextFireTime = dayTime.GetDateTimeOffset(now);
        }

        /// <summary>
        /// Gets time of day when the job is triggered
        /// </summary>
        public DayTime TimeOfDay => dayTime;

        public DailySchedule SetTimeOfDay(DayTime dayTime)   
        {
            if (dayTime == null)
            {
                throw new ArgumentNullException(nameof(dayTime), "DayTime is NULL");
            }
            this.dayTime = dayTime;
            return this;
        }

        public DailySchedule SetTimeOfDay(int hour, int minute, int second)
        {
            this.dayTime = new DayTime(hour, minute, second);
            return this;
        }

        public DailySchedule SetTimeOfDay(int hour, int minute) => SetTimeOfDay(hour, minute, second: 0);

        public DailySchedule SetTimeOfDay(int hour) => SetTimeOfDay(hour, minute: 0, second: 0);

        public override DateTimeOffset? CalculateNextFireTime()
        {
            if (!nextFireTime.HasValue)
                  return null;

            nextFireTime = nextFireTime.Value.AddDays(1);
            nextFireTime = dayTime.GetDateTimeOffset(nextFireTime.Value);
            return nextFireTime;
        }
    }
}
