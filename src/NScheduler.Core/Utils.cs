using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NScheduler.Core
{
    public static class Utils
    {
        public static void SafeDispose(this object obj)
        {
            IDisposable disposable = obj as IDisposable;
            if (disposable != null)
            {
                try
                {
                    disposable.Dispose();
                } catch {}
            }
        }
    }
}
