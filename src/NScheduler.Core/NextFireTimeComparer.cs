using System;
using System.Collections.Generic;

namespace NScheduler.Core
{
    internal sealed class NextFireTimeComparer : IComparer<JobHolder>
    {
        public static NextFireTimeComparer GetInstance() => new NextFireTimeComparer();

        public int Compare(JobHolder x, JobHolder y)
        {
            var xNow = x.Schedule.GetNextFireTime() ?? DateTime.MaxValue;
            var yNow = y.Schedule.GetNextFireTime() ?? DateTime.MaxValue;

            if (xNow < yNow)
                return -1;
            if (xNow > yNow)
                return 1;

            return 0;
        }
    }
}
