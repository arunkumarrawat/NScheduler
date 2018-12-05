using NSchduler.Tests.Jobs;
using NScheduler.Core;
using NUnit.Framework;

namespace NSchduler.Tests
{
    [TestFixture]
    public class SchedulerTests
    {
        private Scheduler scheduler;

        [Test]
        public void Test()
        {
            scheduler = new Scheduler();

            JobSchedule schedule = new JobSchedule();
            schedule.SetRepeatInterval(10, TimeInterval.Seconds);

            WriteDebugTextJob job = new WriteDebugTextJob("Hello!");

            scheduler.EnqueueJob(job, schedule);
            scheduler.Start().Wait();
        }
    }
}
