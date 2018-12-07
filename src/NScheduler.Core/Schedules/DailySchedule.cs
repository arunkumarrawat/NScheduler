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
            DateTimeOffset now = DateTimeOffset.Now;
            nextFireTime = dayTime.AdjustTime(now);
        }

        public override DateTimeOffset? CalculateNextFireTime()
        {
            if (!nextFireTime.HasValue)
                  return null;

            nextFireTime = nextFireTime.Value.AddDays(1);
            nextFireTime = dayTime.AdjustTime(nextFireTime.Value);
            return nextFireTime;
        }

        public virtual DailySchedule SetTimeOfDay(int hour, int minute, int second)
        {
            dayTime = new DayTime(hour, minute, second);
            return this as DailySchedule;
        }

        public virtual DailySchedule SetTimeOfDay(int hour, int minute)
        {
            dayTime = new DayTime(hour, minute, second: 0);
            return this as DailySchedule;
        }

        public virtual DailySchedule SetTimeOfDay(int hour)
        {
            throw new NotImplementedException();
        }

        public virtual DailySchedule SetTimeOfDay(DateTimeOffset offset)
        {
            throw new NotImplementedException();
        }
    }
}
