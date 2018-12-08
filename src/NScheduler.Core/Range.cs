using System;
using System.Diagnostics;

namespace NScheduler.Core
{
    [DebuggerDisplay("Min = {Min,nq}, Max = {Max,nq}")]
    public sealed class Range<T> where T: IComparable<T>
    {
        public Range(T min, T max)
        {
            if (min == null)
            {
                throw new ArgumentNullException(nameof(min), "MIN value is NULL");
            }
            
            if (max == null)
            {
                throw new ArgumentNullException(nameof(max), "Max value is NULL");
            }

            int cr = min.CompareTo(max);
            if (cr >= 0)
            {
                throw new ArgumentException("MIN value is greater than or equals to MAX value", nameof(min));
            }

            Min = min;
            Max = max;
        }

        /// <summary>
        /// Gets minimal value within the range
        /// </summary>
        public T Min { get; }

        /// <summary>
        /// Gets maximal value within the range
        /// </summary>
        public T Max { get; }

        /// <summary>
        /// Gets a boolean value whether specified item is located
        /// within the range
        /// </summary>
        /// <param name="item"></param>
        public bool Contains(T item)
        {
            if (item == null)
                  return false;
            return item.CompareTo(Min) >= 0 &&
                   item.CompareTo(Max) <= 0;
        }
    }
}
