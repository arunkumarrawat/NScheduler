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
        private Task _task;

        public Scheduler()
        {
            jobsQueue = new SortedSet<JobHolder>(NextFireTimeComparer.GetInstance());
            nextJobs = new List<JobHolder>();
        }

        /// <summary>
        /// Starts scheduler 
        /// </summary>
        /// <returns></returns>
        public void Start()
        {         
            isRunning = true;
            _task = Task.Run(() =>
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
                                Task.Run(() => {
                                    nj.Context.Refresh();
                                    nj.Job.Execute(nj.Context);
                                });
                            } catch { }
                        }
                    }
                }
            });
        }

        public virtual void ScheduleJob(IJob job, JobSchedule schedule)
        {
            if (isShutDown)
                  throw new InvalidOperationException("Scheduler is shut down. Cannot enqueue a new job");
            lock (jobsQueue)
                jobsQueue.Add(new JobHolder(job, schedule));
        }

        /// <summary>
        /// Stops scheduler and all pending jobs
        /// </summary>
        /// <returns></returns>
        public virtual void Stop()
        {
            isRunning = false;
            isShutDown = true;
        }

        public virtual void Pause()
        {
            if (isShutDown)
                  return;
            isPaused = true;
        }

        public void WaitShutDown()
        {
            if (_task == null)
                return;

            _task.Wait();
        }
    }
}
