using System;
using System.Diagnostics;

namespace NScheduler.Core
{
    [DebuggerDisplay("{DebugLine},nq")]
    public sealed class DayTime : IEquatable<DayTime>
    {
        private static readonly Range<int> HoursRange;
        private static readonly Range<int> MinutesRange;
        private static readonly Range<int> SecondsRange;

        public static DayTime ZeroTime => new DayTime(hours: 0, minutes: 0, seconds: 0);

        static DayTime()
        {
            HoursRange = new Range<int>(min: 0, max: 23);
            MinutesRange = new Range<int>(min: 0, max: 59);
            SecondsRange = new Range<int>(min: 0, max: 59);
        }

        private readonly int hour;
        private readonly int minute;
        private readonly int second;

        public DayTime(int hours, int minutes, int seconds)
        {
            this.hour = hours;
            this.minute = minutes;
            this.second = seconds;
        }

        private string DebugLine => $"[{Hours}:{Minutes}:{Seconds}]";

        public int Hours => hour;

        public int Minutes => second;

        public int Seconds => second;

        public DateTimeOffset GetDateTimeOffset(DateTimeOffset nextTime)
        {
            return new DateTimeOffset(year: nextTime.Year,
                                      month: nextTime.Month,
                                      day: nextTime.Day,
                                      offset: nextTime.Offset,
                                      hour: this.hour,
                                      minute: this.minute,
                                      second: this.second);
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
