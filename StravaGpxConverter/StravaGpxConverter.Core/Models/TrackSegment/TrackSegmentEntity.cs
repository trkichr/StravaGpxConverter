using StravaGpxConverter.Core.Models.TrackPoint;
using System;
using System.Collections.Generic;
using System.Text;

namespace StravaGpxConverter.Core.Models.TrackSegment
{
    public sealed class TrackSegmentEntity
    {
        public TimeSpan WaitingTime { get; }
        public Coordinate StartLat { get; }
        public Coordinate StartLon { get; }
        public string StartEle { get; }
        public UTCDatetime StartTime { get; }
        public Coordinate EndLat { get; }
        public Coordinate EndLon { get; }
        public string EndEle { get; }
        public UTCDatetime EndTime { get; }
        public BackGroundColor Color { get; }

        public TrackSegmentEntity(TrackPointEntity startTrackPoint, TrackPointEntity endTrackPoint)
        {
            WaitingTime = new TimeSpan(startTrackPoint.Time.Time, endTrackPoint.Time.Time);
            StartLat = startTrackPoint.Lat;
            StartLon = startTrackPoint.Lon;
            StartEle = startTrackPoint.Ele;
            StartTime = startTrackPoint.Time;
            EndLat = endTrackPoint.Lat;
            EndLon = endTrackPoint.Lon;
            EndEle = endTrackPoint.Ele;
            EndTime = endTrackPoint.Time;
            Color = new BackGroundColor(WaitingTime.Value);
        }
    }
}
