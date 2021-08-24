using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace StravaGpxConverter.Core.Models.TrackPoint
{
    public sealed class TrackPointEntity
    {
        public uint Index { get; }
        public Coordinate Lat { get; }
        public Coordinate Lon { get; }
        public string Ele { get; }
        public UTCDatetime Time { get; }
        public XmlNode Node { get; }

        public TrackPointEntity(uint index, string lat, string lon, string ele, string time, string startTime, XmlNode node)
        {
            Index = index;
            Lat = new Coordinate(lat);
            Lon = new Coordinate(lon);
            Ele = ele;
            Time = new UTCDatetime(time);
            Node = node;
        }

        public bool IsRelated(TrackPointEntity segment)
        {
            var distance = Math.Sqrt(
                Math.Pow(Math.Abs(Lat.Value - segment.Lat.Value), 2)
                + Math.Pow(Math.Abs(Lon.Value - segment.Lon.Value), 2)) * 10000 * 1000 / 90;
            var speed = distance / segment.Time.Time.Subtract(Time.Time).TotalSeconds;

            return speed < 1;
        }
    }
}
