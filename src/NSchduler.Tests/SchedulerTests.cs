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

            WriteDebugTextJob job = new WriteDebugTextJob("Hello - 1 second interval");
            WriteDebugTextJob job2 = new WriteDebugTextJob("Hello - 5 seconds interval");

            JobSchedule schedule2 = new JobSchedule();
            schedule2.SetRepeatInterval(5, TimeInterval.Seconds);

            await scheduler.ScheduleJob(job, schedule);
            //await scheduler.ScheduleJob(job2, schedule2);
            await scheduler.Run();
        }
    }
}
