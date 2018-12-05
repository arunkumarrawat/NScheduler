using System.Collections.Generic;

namespace NScheduler.Core
{
    internal sealed class NextFireTimeComparer : IComparer<JobHolder>
    {
        public static NextFireTimeComparer GetInstance() => new NextFireTimeComparer();

        public int Compare(JobHolder x, JobHolder y)
        {
            var xFireTime = x.Schedule.GetNextFireTime();
            var yFireTime = y.Schedule.GetNextFireTime();

            if (xFireTime != null || yFireTime != null)
            {
                // if the first value is not defined,
                // then it should go before next value,
                // so the Scheduler could un-schedule associated 
                // job faster
                if (xFireTime == null)
                    return -1;

                if (yFireTime == null)
                    return 1;

                if (xFireTime < yFireTime)
                    return -1;
                if (xFireTime > yFireTime)
                    return 1;
            }

            return x.Id.CompareTo(y.Id);
        }
    }
}
