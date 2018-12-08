using System;

namespace NScheduler.Core.Schedules
{
    /// <summary>
    /// Daily schedule with fires at specified time of day
    /// </summary>
    public class DailySchedule : Schedule<DailySchedule>
    {
        private DayTime dayTime = DayTime.ZeroTime;

        public DayTime TimeOfDay => dayTime;

        public override void SetInitialFireTime()
        {
            nextFireTime = dayTime.GetAdjustedTime(createdOn);
        }

        public override DateTimeOffset? CalculateNextFireTime()
        {
            if (!prevFireTime.HasValue)
                  return null;
            if (finalFireTime.HasValue && finalFireTime < Time.Now())
                  return null;

            DateTimeOffset result = prevFireTime.Value.AddDays(1);
            return dayTime.GetAdjustedTime(result).Value;
        }

        public virtual DailySchedule SetTimeOfDay(int hour, int minute, int second)
        {
            dayTime = new DayTime(hour, minute, second);
            return this;
        }

        public virtual DailySchedule SetTimeOfDay(int hour, int minute)
        {
            dayTime = new DayTime(hour, minute, second: 0);
            return this;
        }

        public virtual DailySchedule SetTimeOfDay(string time)
        {
            DayTime dt;
            if (!DayTime.TryParse(time, out dt))
                throw new ArgumentException("Invalid time format", nameof(time));
            dayTime = dt;
            return this;
        }

        public virtual DailySchedule SetTimeOfDay(DateTimeOffset offset)
        {
            dayTime = new DayTime(offset.TimeOfDay);
            return this;
        }

        public virtual DailySchedule SetTimeOfDay(TimeSpan ts)
        {
            dayTime = new DayTime(ts);
            return this;
        }
    }
}
