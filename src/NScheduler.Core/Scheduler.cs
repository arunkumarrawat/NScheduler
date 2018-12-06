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
        private CancellationTokenSource stopSrc;
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
            this.stopSrc = new CancellationTokenSource();
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
                 stopSrc.Cancel();
                 execTask.Wait();
                 stopSrc = new CancellationTokenSource();
            }

            execTask = Task.Run(() =>
            {
                while (!stopSrc.IsCancellationRequested)
                {
                    if (isPaused)
                    {
                        SpinWait sw = new SpinWait();
                        while (isPaused && !stopSrc.IsCancellationRequested)
                            sw.SpinOnce();

                        if (stopSrc.IsCancellationRequested)
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

                            DateTime? scheduledFireTime = jh.Schedule.GetScheduledFireTime();

                            if (!scheduledFireTime.HasValue)
                            {
                                jobsQueue.RemoveWhere(x => x.Id == jh.Id);
                                continue;
                            }

                            if (scheduledFireTime > now)
                                    break;

                            nextJobs.Add(jh);
                            jobsQueue.RemoveWhere(x => x.Id == jh.Id);
                        }                                                                        
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

                                    lock (jobsQueue)
                                    {
                                        jh.Schedule.SetNextFireTime();
                                        jobsQueue.Add(jh);
                                    }
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
            stopSrc.Cancel();
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
            isPaused = false;
            return Task.CompletedTask;
        }
    }
}
