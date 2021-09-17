using System;
using System.Collections.Generic;
using System.Text;

namespace StravaGpxConverter.Core.Models.TrackPoint
{
    public interface ITrackPointRepository
    {
        public void Load(List<string> gpxFileName);
        public List<TrackPointEntity> GetAll();
        public void Save(List<TrackPointEntity> waitingTrackPointList);
    }
}
