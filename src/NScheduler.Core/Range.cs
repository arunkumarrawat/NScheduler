using System;

namespace NScheduler.Core
{
    public sealed class Range<T> where T: IComparable<T>
    {
        public Range(T min, T max)
        {
            Ensure.ArgNotNull(min, "MIN value is NULL");
            Ensure.ArgNotNull(max, "MAX value is NULL");

            int cr = min.CompareTo(max);
            if (cr >= 0)
            {
                throw new ArgumentException("MIN value is greater than or equals to MAX value", nameof(min));
            }

            Min = min;
            Max = max;
        }

        public bool Contains(T item)
        {
            if (item == null)
                return false;
            return item.CompareTo(Min) >= 0 && 
                   item.CompareTo(Max) <= 0;
        }

        /// <summary>
        /// Gets minimal value within the range
        /// </summary>
        public T Min { get; }

        /// <summary>
        /// Gets maximal value within the range
        /// </summary>
        public T Max { get; }
    }
}
