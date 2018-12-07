using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NScheduler.Core.Schedules
{
    public class AnnualSchedule : PeriodicSchedule<AnnualSchedule>
    {
        public sealed override TimeInterval Period => TimeInterval.Years;
    }
}
