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
        private CancellationTokenSource stopSrc;
        private Task execTask;
        private volatile bool isPaused;
        private volatile bool isShutDown;

        public Scheduler()
        {
            this.jobsQueue = new SortedSet<JobHolder>(NextFireTimeComparator.GetInstance());
            this.nextJobs = new List<JobHolder>();
            this.stopSrc = new CancellationTokenSource();
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

                    DateTimeOffset now = DateTimeOffset.Now;
                    nextJobs.Clear();

                    lock (jobsQueue)
                    {
                        while (true)
                        {
                            JobHolder jh = jobsQueue.FirstOrDefault();
                            if (jh == null)
                                  break;

                            DateTimeOffset? scheduledFireTime = jh.Schedule.GetScheduledFireTime();

                            if (!scheduledFireTime.HasValue)
                            {
                                jobsQueue.Remove(jh);
                                continue;
                            }

                            if (scheduledFireTime > now)
                                    break;

                            nextJobs.Add(jh);
                            jobsQueue.Remove(jh);
                        }                                                                        
                    } // end LOCK

                    if (nextJobs.Count > 0)
                    {
                        foreach (JobHolder nj in nextJobs)
                        {
                            Task.Run(() => 
                            {
                                try
                                {
                                    nj.Job.Execute(nj.Context);
                                    nj.Context.OnJobExecuted(nj);

                                    if (nj.Schedule.GetScheduledFireTime() != null)
                                    {
                                        lock (jobsQueue)
                                            jobsQueue.Add(nj);
                                    }
                                } catch (Exception ex)
                                {
                                    nj.Context.OnJobFaulted(ex, nj);
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
