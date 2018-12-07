using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NScheduler.Core
{
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

        private readonly int hours;
        private readonly int minutes;
        private readonly int seconds;

        public DayTime(int hours, int minutes, int seconds)
        {

        }

        public int Hours => hours;

        public int Minutes => seconds;

        public int Seconds => seconds;


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
