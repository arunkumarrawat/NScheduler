using NScheduler.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSchduler.Tests.Jobs
{
    internal class WriteDebugTextJob : IJob
    {
        private string text;
        private bool includeTime;
 
        internal WriteDebugTextJob(string text, bool includeTime)
        {
            this.text = text;
            this.includeTime = includeTime;
        }

        internal WriteDebugTextJob(string text)
            : this(text, true)
        {
        }

        public void Execute()
        {
            string line = text;
            if (includeTime)
                line = $"{line} ({DateTime.Now.ToString("HH:mm:ss")})";
            Debug.WriteLine(line);
        }
    }                                                                                  
}
