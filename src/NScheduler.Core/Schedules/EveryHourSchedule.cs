using System;

namespace NScheduler.Core.Schedules
{
    public class EveryHourSchedule : PeriodicSchedule<EveryHourSchedule>
    {
        public sealed override TimeInterval Period => TimeInterval.Hours;

        public EveryHourSchedule()
        {
        }
    }
}
