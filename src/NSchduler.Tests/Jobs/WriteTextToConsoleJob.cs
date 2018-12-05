using NScheduler.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSchduler.Tests.Jobs
{
    internal class WriteTextToConsoleJob : IJob
    {
        private string text;
 
        internal WriteTextToConsoleJob(string text)
        {
            this.text = text;
        }

        public void Execute()
        {
            Debug.WriteLine(text);
        }
    }
}
