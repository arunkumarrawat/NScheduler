using NSchduler.Tests.Jobs;
using NScheduler.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NSchduler.Tests
{
    public class SchedulerTests
    {
        private Scheduler scheduler;

        [Fact]
        public void test()
        {
            scheduler = new Scheduler();

            JobSchedule schedule = new JobSchedule();
            schedule.SetRepeatInterval(10, TimeInterval.Seconds);

            WriteTextToConsoleJob job = new WriteTextToConsoleJob("Hello!");

            scheduler.EnqueueJob(job, schedule);
            scheduler.Start().Wait();
        }
    }
}
