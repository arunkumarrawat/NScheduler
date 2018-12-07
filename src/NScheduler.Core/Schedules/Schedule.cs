﻿using System;

namespace NScheduler.Core.Schedules
{
    /// <summary>
    /// Base class defining <see cref="IJob"/>'s runtime schedule
    /// </summary>
    public abstract class Schedule
    {
        protected DateTimeOffset? nextFireTime;
        protected DateTimeOffset? previousFireTime;
        protected JobContext context;
        protected int reTryAttempts;

        /// <summary>
        /// Sets job's context
        /// </summary>
        /// <param name="context"></param>
        internal void SetContext(JobContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Sets exact date & time of the next fire
        /// </summary>
        internal void SetNextFireTime()
        {
            previousFireTime = nextFireTime;
            nextFireTime = CalculateNextFireTime();
        }

        /// <summary>
        /// Allows a schedule to set initial fire time and 
        /// prepare itself before being enqueued into engine
        /// </summary>
        public virtual void SetInitialFireTime()
        {
            nextFireTime = DateTimeOffset.Now;
        }

        /// <summary>
        /// Calculates exact date & time of the next fire
        /// </summary>
        /// <returns></returns>
        public abstract DateTimeOffset? CalculateNextFireTime();
        
        /// <summary>
        /// Gets exact date & time of scheduled fire 
        /// </summary>
        public DateTimeOffset? GetNextFireTime() => nextFireTime;

        /// <summary>
        /// Gets max count of re-try attempts before job is considered faulted
        /// </summary>
        public int ReTryAttempts => reTryAttempts;
    }
}