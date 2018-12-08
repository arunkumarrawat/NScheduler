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
            if (!previousFireTime.HasValue)
                  return null;

            DateTimeOffset result = previousFireTime.Value.AddDays(1);
            result = dayTime.GetAdjustedTime(result).Value;
            return result;
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
            dayTime = new DayTime(hour, minute: 0, second: 0);
            return this as DailySchedule;
        }

        public virtual DailySchedule SetTimeOfDay(DateTimeOffset offset)
        {
            throw new NotImplementedException();
        }
    }
}
