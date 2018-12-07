using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NScheduler.Core.Schedules
{
    public abstract class Schedule<TSchedule> : Schedule where TSchedule : Schedule
    {
        protected int reTryAttempts;

        public virtual TSchedule SetReTryAttempts(int reTry)
        {
            this.reTryAttempts = reTry;
            return this as TSchedule;
        }

        public virtual TSchedule SetInfinite()
        {
            return this as TSchedule;
        }
    }
}
