using NScheduler.Core;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NSchduler.Tests.Jobs
{
    internal class WriteDebugTextJob : IJob
    {
        private readonly string text;
        private readonly bool includeTime;
 
        internal WriteDebugTextJob(string text, bool includeTime)
        {
            this.text = text;
            this.includeTime = includeTime;
        }

        internal WriteDebugTextJob(string text)
            : this(text, true)
        {
        }

        public virtual Task Execute(JobContext context)
        {
            string line = text;
            if (includeTime)
                line = $"{line} ({DateTime.Now.ToString("HH:mm:ss.fff")})";
            Debug.WriteLine(line);
            return Task.CompletedTask;
        }
    }                                                                                  
}
