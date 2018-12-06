using NLog;
using NScheduler.Core.Schedules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NScheduler.Core
{
    public class Scheduler
    {
        private const int PauseWaitMs = 1000;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly SortedSet<JobHolder> jobsQueue;
        private readonly List<JobHolder> nextJobs;
        private readonly object pauseLock;
        private Task execTask;
        private bool paused;
        private volatile bool running;

        public Scheduler()
        {
            this.jobsQueue = new SortedSet<JobHolder>(NextFireTimeComparator.GetInstance());
            this.nextJobs = new List<JobHolder>();
            this.pauseLock = new object();

            this.running = true;
            this.paused = false;
        }

        /// <summary>
        /// Runs scheduler
        /// </summary>
        /// <returns></returns>
        public virtual Task Run()
        {
            Debug("Scheduler starting ...");

            execTask = Task.Run(() =>
            {
                Debug("Scheduler started");

                while (running)
                {
                    lock (pauseLock)
                    {
                        while (paused && running)
                        {
                            try
                            {
                                // wait until scheduler resumes
                                // processing loop
                                Monitor.Wait(pauseLock, PauseWaitMs);
                            } catch
                            {
                            }
                        }

                        if (!running)
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
                        // check if pause requested
                        // just after jobs fetched
                        bool pauseReq;

                        lock (pauseLock)
                           pauseReq = paused;

                        if (pauseReq)
                        {
                            // save jobs until resume
                            lock (jobsQueue)
                            {
                                foreach (JobHolder jh in nextJobs)
                                   jobsQueue.Add(jh);
                            }
                            continue;
                        }

                        foreach (JobHolder jh in nextJobs)
                        {
                            Task.Run(async () => 
                            {
                                try
                                {
                                    await jh.Job.Execute(jh.Context);
                                    jh.Context.OnJobExecuted(jh);
                                    lock (jobsQueue) jobsQueue.Add(jh);
                                } catch (Exception ex)
                                {
                                    Exception lastError = ex;
                                    int maxReTry = jh.Schedule.ReTryAttempts;

                                    if (maxReTry > 0)
                                    {
                                        while (maxReTry-- > 0)
                                        {
                                            try
                                            {
                                                jh.Context.IncrementReTryAttempt();
                                                await jh.Job.Execute(jh.Context);
                                                jh.Context.OnJobExecuted(jh);
                                                lock (jobsQueue) jobsQueue.Add(jh);
                                                return;
                                            } catch (Exception exOnReTry)
                                            {
                                                lastError = exOnReTry;
                                                jh.Context.SetLastError(lastError);
                                                continue;
                                            }
                                        }
                                    }

                                    jh.Context.OnJobFaulted(lastError, jh);
                                }                                  
                            });
                        }
                    }
                }

                Debug("Scheduler shutting down ...");
            });
            return execTask;
        }

        protected void Debug(string msg)
        {
            if (logger.IsDebugEnabled)
                  logger.Debug(msg);
        }

        /// <summary>
        /// Schedules a new job 
        /// </summary>
        /// <param name="job"></param>
        /// <param name="schedule"></param>
        /// <returns></returns>
        public virtual Task ScheduleJob(IJob job, Schedule schedule)
        {
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
        public async virtual Task Stop()
        {
            Task task = execTask;
            if (task == null) return;
            running = false;
            await task.ConfigureAwait(false);
        }

        /// <summary>
        /// Pauses scheduler until it gets resumed
        /// </summary>
        /// <returns></returns>
        public virtual Task Pause()
        {
            lock (pauseLock)
            {
                paused = true;
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Resumes scheduler after pause
        /// </summary>
        /// <returns></returns>
        public virtual Task Resume()
        {
            Task task = execTask;
            if (task == null || !running)
                return Task.CompletedTask;

            lock (pauseLock)
            {
                paused = false;
                Monitor.Pulse(pauseLock);
                return Task.CompletedTask;
            }
        }
    }
}
