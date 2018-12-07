using System;
using System.Diagnostics;

namespace NScheduler.Core
{
    [DebuggerDisplay("{DebugString},nq")]
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

        private string DebugString => $"[{Hours}:{Minutes}:{Seconds}]";

        public int Hours => hour;

        public int Minutes => second;

        public int Seconds => second;

        public DateTimeOffset AdjustTime(DateTimeOffset time)
        {
            return new DateTimeOffset(year: time.Year,
                                      month: time.Month,
                                      day: time.Day,
                                      offset: time.Offset,
                                      hour: hour,
                                      minute: minute,
                                      second: second);
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
            return other.Hours == Hours &&
                   other.Minutes == Minutes &&
                   other.Seconds == Seconds;
        }
    }
}
