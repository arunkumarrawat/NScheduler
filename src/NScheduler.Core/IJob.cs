using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NScheduler.Core
{
    /// <summary>
    /// Interface of an abstract job
    /// </summary>
    public interface IJob
    {
        void Execute();
    }
}
