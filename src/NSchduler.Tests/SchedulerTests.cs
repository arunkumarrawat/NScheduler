using NSchduler.Tests.Jobs;
using NScheduler.Core;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NSchduler.Tests
{
    [TestFixture]
    public class SchedulerTests
    {
        private Scheduler scheduler;

        [Test]
        public async Task Test()
        {
            scheduler = new Scheduler();

            JobSchedule schedule = new JobSchedule();
            schedule.SetRepeatInterval(1, TimeInterval.Seconds);

            WriteDebugTextJob job = new WriteDebugTextJob("Hello!");

            await scheduler.ScheduleJob(job, schedule);
            await scheduler.Run();
        }
    }
}
