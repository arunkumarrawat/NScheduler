using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NScheduler.Core
{
    internal sealed class JobHolder
    {
        private readonly IJob job;
        private readonly JobSchedule schedule;
        private readonly Guid id;

        public JobHolder(IJob job, JobSchedule schedule)
        {
            this.job = job;
            this.schedule = schedule;
            this.id = Guid.NewGuid();
        }

        public override bool Equals(object obj)
        {
            var jh = obj as JobHolder;
            if (jh == null)
                return false;
            return id == jh.id;
        }

        /// <summary>
        /// Gets schedule of the job
        /// </summary>
        public JobSchedule Schedule => schedule;

        public IJob Job => job;


        public Guid Id => id;


    }
}
