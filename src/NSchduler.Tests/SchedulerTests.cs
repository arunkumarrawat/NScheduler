using NSchduler.Tests.Jobs;
using NScheduler.Core;
using NScheduler.Core.Schedules;
using NUnit.Framework;
using System;
using System.Diagnostics;
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
            DailySchedule schedule = new DailySchedule();

            schedule.SetTimeOfDay(06, 48, 00);

            WriteDebugTextJob job = new WriteDebugTextJob("Hello 1s span");
            WriteDebugTextJob job2 = new WriteDebugTextJob("Hello 5s span");
            WriteDebugTextJob job3 = new WriteDebugTextJob("Hello 10s span");

            await scheduler.ScheduleJob(job, schedule);

            //await scheduler.ScheduleJob(job2, schedule2);
            //await scheduler.ScheduleJob(job3, schedule3);
            await scheduler.Run();
        }              
    }
}
