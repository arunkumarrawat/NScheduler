using System;
using System.Diagnostics;

namespace NScheduler.Core
{
    [DebuggerDisplay("{DebugString,nq}")]
    public sealed class DayTime
    {
        private static readonly Range<int> HoursRange;
        private static readonly Range<int> MinutesRange;
        private static readonly Range<int> SecondsRange;

        public static DayTime ZeroTime => new DayTime(hour: 0, minute: 0, second: 0);

        public static bool TryParse(string time, out DayTime dayTime)
        {
            dayTime = null;
            if (time == null || time.Trim().Length == 0)
                return false;

            string[] parts = time.Split(':');

            if (parts.Length < 2)
                return false;

            int hh, mm, ss = 0;
            if (!int.TryParse(parts[0], out hh))
                return false;
            if (!int.TryParse(parts[1], out mm))
                return false;

            if (parts.Length > 2)
            {
                if (!int.TryParse(parts[2], out ss))
                    return false;
            }

            dayTime = new DayTime(hh, mm, ss);
            return true;
        }

        static DayTime()
        {
            HoursRange = new Range<int>(min: 0, max: 23);
            MinutesRange = new Range<int>(min: 0, max: 59);
            SecondsRange = new Range<int>(min: 0, max: 59);
        }

        private readonly int hour;
        private readonly int minute;
        private readonly int second;

        public DayTime(int hour, int minute, int second)
        {
            if (!HoursRange.Contains(hour))
                  throw new ArgumentException("Value of hour should be on range [0..23]", nameof(hour));
            if (!MinutesRange.Contains(minute))
                  throw new ArgumentException("Value of minute should be on range [0..59]", nameof(minute));
            if (!SecondsRange.Contains(second))
                  throw new ArgumentException("Valud of second should be on range [0..59]", nameof(second));

            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }

        public DayTime(TimeSpan ts)
        {
            hour = ts.Hours;
            minute = ts.Minutes;
            second = ts.Seconds;
        }

        public DayTime(int hour, int minute): this(hour, minute, second: 0)
        {
        }

        private string DebugString => $"[{Hour:00}:{Minute:00}:{Second:00}]";

        /// <summary>
        /// Gets hour component of day time
        /// </summary>
        public int Hour => hour;

        /// <summary>
        /// Gets minute component of day time
        /// </summary>
        public int Minute => minute;

        /// <summary>
        /// Gets second component of day time
        /// </summary>
        public int Second => second;

        /// <summary>
        /// Gets time adjusted to current time of day
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DateTimeOffset? GetAdjustedTime(DateTimeOffset? time)
        {
            if (!time.HasValue)
                  return null;

            DateTimeOffset newTime = new DateTimeOffset(time.Value.Date, time.Value.Offset);
            TimeSpan diff = new TimeSpan(0, Hour, Minute, Second);
            return newTime.Add(diff);
        }

        public override bool Equals(object obj)
        {
            DayTime other = obj as DayTime;
            if (other == null)
                  return false;
            return other.Hour == Hour &&
                   other.Minute == Minute &&
                   other.Second == Second;
        }
    }
}
