using StravaGpxConverter.Core;
using StravaGpxConverter.Core.Models.TrackPoint;
using StravaGpxConverter.Infrastructure.Fake;
using StravaGpxConverter.Infrastructure.Xml;
using System;
using System.Collections.Generic;
using System.Text;

namespace StravaGpxConverter.Infrastructure
{
    public static class Factories
    {
        public static ITrackPointRepository CreateTrackPoint()
        {
#if DEBUG
            if (Shared.IsFake)
            {
                return new FakeGpx();
            }
#endif
            return new XmlGpx();
        }
    }
}
