using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NScheduler.Core
{
    /// <summary>
    /// Time utility
    /// </summary>
    public static class Time
    {
        public static DateTimeOffset Now() => DateTimeOffset.Now;
    }
}
