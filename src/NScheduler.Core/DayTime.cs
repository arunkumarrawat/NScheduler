using System;
using System.Diagnostics;

namespace NScheduler.Core
{
    [DebuggerDisplay("{DebugString,nq}")]
    public sealed class DayTime : IEquatable<DayTime>
    {
        private static readonly Range<int> HoursRange;
        private static readonly Range<int> MinutesRange;
        private static readonly Range<int> SecondsRange;

        public static DayTime ZeroTime => new DayTime(hour: 0, minute: 0, second: 0);

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
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }

        private string DebugString => $"[{Hour:00}:{Minute:00}:{Second:00}]";

        public int Hour => hour;

        public int Minute => minute;

        public int Second => second;

        public DateTimeOffset? GetAdjustedTime(DateTimeOffset? time)
        {
            if (!time.HasValue)
                return null;

            DateTimeOffset newTime = new DateTimeOffset(time.Value.Date, time.Value.Offset);
            TimeSpan diff = new TimeSpan(0, Hour, Minute, Second);
            newTime = newTime.Add(diff);
            return newTime;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DayTime other))
                return false;
            return Equals(other);
        }

        public bool Equals(DayTime other)
        {
            if (other == null)
                  return false;
            return other.Hour == Hour &&
                   other.Minute == Minute &&
                   other.Second == Second;
        }
    }
}
