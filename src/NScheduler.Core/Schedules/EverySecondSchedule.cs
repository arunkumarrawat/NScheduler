using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NScheduler.Core.Schedules
{
    public class EverySecondSchedule : PeriodicSchedule<EverySecondSchedule>
    {
        public override TimeInterval Period => TimeInterval.Seconds;
    }
}
