using System;

namespace StravaGpxConverter.Core.Models
{
    public sealed class UTCDatetime : ValueObject<UTCDatetime>
    {
        public DateTime Time { get; }

        public UTCDatetime(string time)
        {
            Time = DateTime.Parse(time);
        }

        public override string ToString()
        {
            return Time.ToLocalTime().ToString("HH:mm:ss yyyy-MM-dd");
            //return Time.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        protected override bool EqualCore(UTCDatetime other)
        {
            return this.ToString() == other.ToString();
        }
    }
}
