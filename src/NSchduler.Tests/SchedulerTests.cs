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

            WriteDebugTextJob job = new WriteDebugTextJob("Hello 1s span");
            WriteDebugTextJob job2 = new WriteDebugTextJob("Hello 5s span");
            WriteDebugTextJob job3 = new WriteDebugTextJob("Hello 10s span");

            JobSchedule schedule2 = new JobSchedule();
            schedule2.SetRepeatInterval(5, TimeInterval.Seconds);

            JobSchedule schedule3 = new JobSchedule();
            schedule3.SetRepeatInterval(10, TimeInterval.Seconds);

            await scheduler.ScheduleJob(job, schedule);
            //await scheduler.ScheduleJob(job2, schedule2);
            //await scheduler.ScheduleJob(job3, schedule3);
            await scheduler.Run();
        }              
    }
}
