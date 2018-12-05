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
        private volatile bool isRunning;
        private volatile bool isPaused;
        private volatile bool isShutDown;

        public Scheduler()
        {
            jobsQueue = new SortedSet<JobHolder>(NextFireTimeComparer.GetInstance());
            nextJobs = new List<JobHolder>();
        }

        /// <summary>
        /// Starts scheduler 
        /// </summary>
        /// <returns></returns>
        public Task Start()
        {         
            isRunning = true;
            return Task.Run(() =>
            {
                while (isRunning)
                {
                    if (isPaused)
                    {
                        SpinWait sw = new SpinWait();
                        while (isPaused && isRunning)
                            sw.SpinOnce();

                        if (!isRunning)
                              break;
                    }

                    var now = DateTime.Now;
                    nextJobs.Clear();

                    lock (jobsQueue)
                    {
                        while (true)
                        {
                            var jh = jobsQueue.FirstOrDefault();
                            if (jh == null)
                                  break;

                            //jobsQueue.RemoveWhere(x => x.Id == jh.Id);
                            var nextJobTime = jh.Schedule.GetNextFireTime();
           
                            if (!nextJobTime.HasValue)
                            {
                                jobsQueue.RemoveWhere(x => x.Id == jh.Id);
                                continue;
                            }

                            if (nextJobTime > now)
                                    break;
                            jobsQueue.RemoveWhere(x => x.Id == jh.Id);
                            jh.Schedule.CalculateNextFireTime();
                            nextJobs.Add(jh);
                        }

                        if (nextJobs.Count > 0)
                              foreach (var nextJob in nextJobs)
                                  jobsQueue.Add(nextJob);
                    } // end LOCK

                    if (nextJobs.Count > 0)
                    {
                        foreach (var nj in nextJobs)
                        {
                            try
                            {
                                // todo: make job context
                                // to keep history of job
                                nj.Job.Execute();
                            } catch { }
                        }
                    }
                }
            });
        }

        public Task EnqueueJob(IJob job, JobSchedule schedule)
        {
            lock (jobsQueue)
                jobsQueue.Add(new JobHolder(job, schedule));
            return Task.CompletedTask;
        }

        /// <summary>
        /// Stops scheduler and all pending jobs
        /// </summary>
        /// <returns></returns>
        public Task Stop()
        {
            isRunning = false;
            isShutDown = true;
            return Task.CompletedTask;
        }

        public Task Pause()
        {
            isPaused = true;
            return Task.CompletedTask;
        }
    }
}
