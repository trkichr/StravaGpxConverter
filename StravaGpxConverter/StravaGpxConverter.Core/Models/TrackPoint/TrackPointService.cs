using System;
using System.Collections.Generic;
using System.Text;

namespace StravaGpxConverter.Core.Models.TrackPoint
{
    public static class TrackPointService
    {
        public static List<TrackPointEntity> GetWaitingTrackPointList(List<TrackPointEntity> allTrackPointList)
        {
            var waitingTrackPointList = new List<TrackPointEntity>();

            for (int i = 0; i < allTrackPointList.Count - 1; i++)
            {
                for (int j = i + 1; j < allTrackPointList.Count; j++)
                {
                    if (j != i + 1)
                    {
                        if (allTrackPointList[i].IsRelated(allTrackPointList[j])
                            && allTrackPointList[j].IsRelated(allTrackPointList[j - 1]))
                        {
                            waitingTrackPointList.Add(allTrackPointList[j]);
                        }
                        break;
                    }
                }
            }

            return waitingTrackPointList;
        }
    }
}
