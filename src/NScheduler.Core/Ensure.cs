using System;

namespace NScheduler.Core
{
    /// <summary>
    /// Set of validation utils
    /// </summary>
    public static class Ensure
    {
        public static void ArgNotNull(object arg, string msg, string argName)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(argName, msg);
            }
        }

        public static void ArgNotNull(object arg, string msg) => ArgNotNull(arg, msg, argName: null);
    }
}
