using StravaGpxConverter.Core.Models.TrackPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StravaGpxConverter.Core.Models.TrackSegment
{
    public static class TrackSegmentService
    {
        public static List<TrackSegmentEntity> CreateSegmentList(List<TrackPointEntity> waitingTrackPointList)
        {
            var waitingTrackSegmentList = new List<TrackSegmentEntity>();
            if(waitingTrackPointList == null)
            {
                return waitingTrackSegmentList;
            }

            var startTrackPoint = waitingTrackPointList.First();
            TrackPointEntity preTrackPoint = null;
            TrackPointEntity endTrackPoint = null;

            foreach (var trackPoint in waitingTrackPointList)
            {
                if (startTrackPoint == trackPoint)
                {
                    preTrackPoint = trackPoint;
                    continue;
                }

                if (startTrackPoint == null)
                {
                    startTrackPoint = trackPoint;
                    preTrackPoint = trackPoint;
                    continue;
                }

                if (trackPoint == waitingTrackPointList.Last())
                {
                    waitingTrackSegmentList.Add(new TrackSegmentEntity(startTrackPoint, trackPoint));
                }
                else if (trackPoint.Index < preTrackPoint.Index + 10)
                {
                    endTrackPoint = trackPoint;
                }
                else
                {
                    if (endTrackPoint != null)
                    {
                        waitingTrackSegmentList.Add(new TrackSegmentEntity(startTrackPoint, endTrackPoint));
                        startTrackPoint = trackPoint;
                    }
                    else if (trackPoint.Index < startTrackPoint.Index + 100)
                    {
                        waitingTrackSegmentList.Add(new TrackSegmentEntity(startTrackPoint, trackPoint));
                        startTrackPoint = null;
                    }
                    else
                    {
                        startTrackPoint = null;
                    }
                    endTrackPoint = null;
                }

                preTrackPoint = trackPoint;
            }

            return waitingTrackSegmentList;
        }
    }
}
