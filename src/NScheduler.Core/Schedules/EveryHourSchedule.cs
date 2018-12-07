using System;

namespace NScheduler.Core.Schedules
{
    public class EveryHourSchedule : PeriodicSchedule<EveryHourSchedule>
    {
        protected override TimeInterval Period => TimeInterval.Hours;

        public EveryHourSchedule()
        {
        }
    }
}
