using System;

namespace NScheduler.Core.Schedules
{
    /// <summary>
    /// Daily schedule with fires at specified time of day
    /// </summary>
    public class DailySchedule : Schedule<DailySchedule>
    {
        private DayTime dayTime = DayTime.ZeroTime;

        /// <summary>
        /// Gets time of day when the job is triggered
        /// </summary>
        public DayTime TimeOfDay => dayTime;

        public DailySchedule SetTimeOfDay(DayTime dayTime)   
        {
            this.dayTime = dayTime ?? throw new ArgumentNullException(nameof(dayTime), "DayTime is NULL");
            return this;
        }

        public DailySchedule SetTimeOfDay(int hours, int minutes, int seconds)
        {
            this.dayTime = new DayTime(hours, minutes, seconds);
            return this;
        }

        internal override DateTimeOffset? CalculateNextFireTime()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            return null;
        }
    }
}
