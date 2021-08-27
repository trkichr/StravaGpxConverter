using StravaGpxConverter.Core;
using StravaGpxConverter.Core.Models.Exceptions;
using StravaGpxConverter.Core.Models.TrackPoint;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace StravaGpxConverter.Infrastructure.Fake
{
    internal class FakeGpx : ITrackPointRepository
    {
        private string GpxFileName { get; set; }
        private XmlDocument Doc { get; set; }

        public FakeGpx()
        {
            Load("Fake.gpx");
        }

        public List<TrackPointEntity> GetAll()
        {
            if (string.IsNullOrEmpty(GpxFileName))
            {
                throw new GpxFileNotLoadedException("GPXファイルがロードされていません");
            }
            var trackSegmentList = new List<TrackPointEntity>();

            var gpx = Doc.ChildNodes[1];
            var trk = gpx.ChildNodes[1];
            uint index = 0;
            foreach (XmlNode node in trk.ChildNodes)
            {
                if (node.Name == "trkseg")
                {
                    var trkptList = node.ChildNodes;

                    foreach (XmlNode trkpt in trkptList)
                    {
                        var ele = trkpt.FirstChild.InnerText;
                        var time = trkpt.LastChild.InnerText;
                        var lat = trkpt.Attributes["lat"].InnerText;
                        var lon = trkpt.Attributes["lon"].InnerText;
                        trackSegmentList.Add(new TrackPointEntity(index, lat, lon, ele, time, trkpt));
                        index++;
                    }
                }
            }

            return trackSegmentList;
        }

        public void Load(string gpxFileName)
        {
            if (string.IsNullOrEmpty(gpxFileName))
            {
                throw new GpxFileNotSelectedException("GPXファイルが選択されていません");
            }

            GpxFileName = Shared.FakePath + gpxFileName;
            Doc = new XmlDocument();
            try
            {
                Doc.Load(GpxFileName);
                var gpx = Doc.ChildNodes[1];
                if (!GpxFileName.Contains("_bak")
                    && gpx.Attributes.Count == 6)
                {
                    Doc.Save(GpxFileName + "_bak");
                }
            }
            catch (Exception ex)
            {
                throw new GpxFileNotLoadedException("GPXファイルのロードに失敗しました", ex);
            }
        }

        public void Save(List<TrackPointEntity> waitingTrackPointList)
        {
            throw new NotImplementedException();
        }
    }
}
