using System;

namespace NScheduler.Core
{
    internal sealed class JobHolder
    {
        private readonly IJob job;
        private readonly JobContext context;
        private readonly JobSchedule schedule;
        private readonly Guid id;

        public JobHolder(IJob job, JobSchedule schedule)
        {
            this.job = job;
            this.context = new JobContext();
            this.schedule = schedule;
            this.schedule.SetContext(this.context);
            this.id = Guid.NewGuid();
        }

        /// <summary>
        /// Gets runtime schedule of the associated <see cref="IJob"/> instance
        /// </summary>
        public JobSchedule Schedule => schedule;

        /// <summary>
        /// Gets the associated <see cref="IJob"/> instance to execute
        /// </summary>
        public IJob Job => job;

        /// <summary>
        /// Gets context of the job
        /// </summary>
        public JobContext Context => context;

        /// <summary>
        /// Gets a unique identifier of the associated <see cref="IJob"/> instance
        /// </summary>
        public Guid Id => id;
    }
}
