using System;

namespace StravaGpxConverter.Core.Models.TrackSegment
{
    public class TimeSpan : ValueObject<TimeSpan>, IComparable
    {
        public System.TimeSpan Value { get; }
        public string DisplayValue { get; }

        public TimeSpan(DateTime startTime, DateTime endTime)
        {
            Value = endTime - startTime;
            DisplayValue = Value.ToString(@"mm\:ss");
        }

        protected override bool EqualCore(TimeSpan other)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return DisplayValue;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (!(obj is TimeSpan))
            {
                throw new ArgumentException();
            }

            TimeSpan timeSpan = (TimeSpan)obj;
            if (Value.TotalSeconds > timeSpan.Value.TotalSeconds) return 1;
            if (Value.TotalSeconds < timeSpan.Value.TotalSeconds) return -1;
            return 0;
        }
    }
}
