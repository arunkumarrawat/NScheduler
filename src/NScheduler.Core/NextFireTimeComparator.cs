using System;
using System.Collections.Generic;

namespace NScheduler.Core
{
    internal sealed class NextFireTimeComparator : IComparer<JobHolder>
    {
        public static NextFireTimeComparator GetInstance() => new NextFireTimeComparator();

        public int Compare(JobHolder x, JobHolder y)
        {
            if (ReferenceEquals(x, y))
                return 0;

            DateTime? xFireTime = x.Schedule.GetScheduledFireTime();
            DateTime? yFireTime = y.Schedule.GetScheduledFireTime();

            if (xFireTime != null || yFireTime != null)
            {
                if (xFireTime == null)
                    return 1;

                if (yFireTime == null)
                    return -1;

                if (xFireTime < yFireTime)
                    return -1;
                if (xFireTime > yFireTime)
                    return 1;
            }

            return x.Id.CompareTo(y.Id);
        }
    }
}
