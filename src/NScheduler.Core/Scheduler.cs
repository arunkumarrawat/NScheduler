using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NScheduler.Core
{
    public class Scheduler
    {
        private readonly SortedSet<JobHolder> jobsQueue;
        private readonly List<JobHolder> nextJobs;
        private readonly ITimeService timeService;
        private CancellationTokenSource cancelSrc;
        private Task execTask;
        private volatile bool isPaused;
        private volatile bool isShutDown;

        public Scheduler() : this(null)
        {
        }

        public Scheduler(ITimeService timeService)
        {
            this.jobsQueue = new SortedSet<JobHolder>(NextFireTimeComparator.GetInstance());
            this.nextJobs = new List<JobHolder>();
            this.cancelSrc = new CancellationTokenSource();
            this.timeService = timeService;
        }

        /// <summary>
        /// Runs scheduler
        /// </summary>
        /// <returns></returns>
        public virtual Task Run()
        {
            if (execTask != null)
            {
                 cancelSrc.Cancel();
                 execTask.Wait();
                 cancelSrc = new CancellationTokenSource();
            }

            execTask = Task.Run(() =>
            {
                while (!cancelSrc.IsCancellationRequested)
                {
                    if (isPaused)
                    {
                        SpinWait sw = new SpinWait();
                        while (isPaused && !cancelSrc.IsCancellationRequested)
                            sw.SpinOnce();

                        if (cancelSrc.IsCancellationRequested)
                               break;
                    }

                    DateTime now = timeService?.Now() ?? DateTime.Now;
                    nextJobs.Clear();

                    lock (jobsQueue)
                    {
                        while (true)
                        {
                            JobHolder jh = jobsQueue.FirstOrDefault();
                            if (jh == null)
                                  break;

                            DateTime? nextJobTime = jh.Schedule.GetNextFireTime();
           
                            if (!nextJobTime.HasValue)
                            {
                                jobsQueue.RemoveWhere(x => x.Id == jh.Id);
                                continue;
                            }

                            if (nextJobTime > now)
                                    break;

                            jobsQueue.RemoveWhere(x => x.Id == jh.Id);
                            jh.Schedule.CalculateNextFireTime(now);
                            nextJobs.Add(jh);
                        }

                        if (nextJobs.Count > 0)
                              foreach (var nextJob in nextJobs)
                                 jobsQueue.Add(nextJob);
                    } // end LOCK

                    if (nextJobs.Count > 0)
                    {
                        foreach (JobHolder jh in nextJobs)
                        {
                            Task.Run(() => 
                            {
                                try
                                {
                                    jh.Job.Execute(jh.Context);
                                    jh.Context.OnJobExecuted(jh);
                                } catch (Exception ex)
                                {
                                    jh.Context.OnJobFaulted(ex);
                                    throw;
                                }
                            });
                        }
                    }
                }
            });

            return execTask;
        }

        public virtual Task ScheduleJob(IJob job, JobSchedule schedule)
        {
            if (isShutDown)
                  throw new InvalidOperationException("Scheduler is shut down. Cannot schedule a new job");

            lock (jobsQueue)
                jobsQueue.Add(new JobHolder(job, schedule));
            return Task.CompletedTask;
        }

        /// <summary>
        /// Un-schedules specified job
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public virtual Task<bool> UnScheduleJob(IJob job)
        {
            lock (jobsQueue)
            {
                int res = jobsQueue.RemoveWhere(jh => jh.Job.Equals(job));
                return Task.FromResult(res > 0);
            }
        } 

        /// <summary>
        /// Stops scheduler and all pending jobs
        /// </summary>
        /// <returns></returns>
        public virtual Task Stop()
        {
            cancelSrc.Cancel();
            isShutDown = true;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Pauses scheduler until it gets resumed
        /// </summary>
        /// <returns></returns>
        public virtual Task Pause()
        {
            isPaused = true;
            return Task.CompletedTask;
        }

        public virtual Task Resume()
        {
            return Task.CompletedTask;
        }
    }
}
