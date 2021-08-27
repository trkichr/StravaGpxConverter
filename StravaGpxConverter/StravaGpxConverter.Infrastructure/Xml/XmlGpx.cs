using StravaGpxConverter.Core.Models.TrackPoint;
using System;
using System.Collections.Generic;
using System.Text;

namespace StravaGpxConverter.Infrastructure.Xml
{
    internal class XmlGpx : ITrackPointRepository
    {
        public List<TrackPointEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Load(string gpxFileName)
        {
            throw new NotImplementedException();
        }

        public void Save(List<TrackPointEntity> waitingTrackPointList)
        {
            throw new NotImplementedException();
        }
    }
}
