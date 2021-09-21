using StravaGpxConverter.Core.Models;
using StravaGpxConverter.Core.Models.Exceptions;
using StravaGpxConverter.Core.Models.TrackPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace StravaGpxConverter.Infrastructure.Xml
{
    internal class XmlGpx : ITrackPointRepository
    {
        private string GpxFileName { get; set; }
        private XmlDocument Doc { get; set; }

        public XmlGpx()
        {
        }

        public void Load(List<string> gpxFileNameList)
        {
            if (gpxFileNameList == null || gpxFileNameList.Count == 0)
            {
                throw new GpxFileNotSelectedException("GPXファイルが選択されていません");
            }

            try
            {
                CombineGpxFileList(gpxFileNameList);
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

        private void CombineGpxFileList(List<string> gpxFileNameList)
        {
            var docList = new Dictionary<UTCDatetime, XmlDocument>();

            foreach (string gpxFileName in gpxFileNameList)
            {
                var doc = new XmlDocument();
                doc.Load(gpxFileName);
                var gpx = doc.ChildNodes[1];
                var trk = gpx.ChildNodes[1];
                foreach (XmlNode node in trk.ChildNodes)
                {
                    if (node.Name == "trkseg")
                    {
                        var time = node.ChildNodes[0].LastChild.InnerText;
                        docList.Add(new UTCDatetime(time), doc);
                        break;
                    }
                }
            }

            var orderedDocList = docList.OrderBy(x => x.Key.Time).Select(x => x.Value).ToList();
            Doc = orderedDocList.First();
            var fileName = Doc.BaseURI.Split("/").Last();
            GpxFileName = gpxFileNameList.Where(x => x.Contains(fileName)).First();
            if (orderedDocList.Count == 1)
            {
                return;
            }

            for (int i = 1; i < orderedDocList.Count(); i++)
            {
                var gpx = orderedDocList[i].ChildNodes[1];
                var trk = gpx.ChildNodes[1];
                foreach (XmlNode trksegNode in trk.ChildNodes)
                {
                    if (trksegNode.Name == "trkseg")
                    {
                        var xx = Doc.ChildNodes[1];
                        var node = Doc.ImportNode(trksegNode, true);
                        Doc.DocumentElement.ChildNodes[1].InsertAfter(node, Doc.DocumentElement.ChildNodes[1].LastChild);
                    }
                }
            }
        }

        public List<TrackPointEntity> GetAll()
        {
            if (string.IsNullOrEmpty(GpxFileName))
            {
                throw new GpxFileNotLoadedException("Gpxファイルの読込に失敗しました");
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

        public void Save(List<TrackPointEntity> waitingTrackPointList)
        {
            var gpx = Doc.ChildNodes[1];
            var trk = gpx.ChildNodes[1];
            var extensions = trk.ChildNodes[3];

            if (gpx.Attributes.Count != 6)
            {
                return;
            }

            gpx.Attributes.RemoveAt(3);
            gpx.Attributes["creator"].InnerText = "StravaGPX";
            gpx.Attributes["xsi:schemaLocation"].InnerText =
                "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd http://www.garmin.com/xmlschemas/GpxExtensions/v3 http://www.garmin.com/xmlschemas/GpxExtensionsv3.xsd http://www.garmin.com/xmlschemas/TrackPointExtension/v1 http://www.garmin.com/xmlschemas/TrackPointExtensionv1.xsd http://www.garmin.com/xmlschemas/GpxExtensions/v3 http://www.garmin.com/xmlschemas/GpxExtensionsv3.xsd http://www.garmin.com/xmlschemas/TrackPointExtension/v1 http://www.garmin.com/xmlschemas/TrackPointExtensionv1.xsd http://www.garmin.com/xmlschemas/GpxExtensions/v3 http://www.garmin.com/xmlschemas/GpxExtensionsv3.xsd http://www.garmin.com/xmlschemas/TrackPointExtension/v1 http://www.garmin.com/xmlschemas/TrackPointExtensionv1.xsd http://www.garmin.com/xmlschemas/GpxExtensions/v3 http://www.garmin.com/xmlschemas/GpxExtensionsv3.xsd http://www.garmin.com/xmlschemas/TrackPointExtension/v1 http://www.garmin.com/xmlschemas/TrackPointExtensionv1.xsd http://www.garmin.com/xmlschemas/GpxExtensions/v3 http://www.garmin.com/xmlschemas/GpxExtensionsv3.xsd http://www.garmin.com/xmlschemas/TrackPointExtension/v1 http://www.garmin.com/xmlschemas/TrackPointExtensionv1.xsd http://www.garmin.com/xmlschemas/GpxExtensions/v3 http://www.garmin.com/xmlschemas/GpxExtensionsv3.xsd http://www.garmin.com/xmlschemas/TrackPointExtension/v1 http://www.garmin.com/xmlschemas/TrackPointExtensionv1.xsd";

            trk.RemoveChild(extensions);

            foreach (XmlNode node in waitingTrackPointList.Select(x => x.Node))
            {
                var waitingDateTime = DateTime.Parse(node.LastChild.InnerText);
                foreach (XmlNode trksegNode in trk.ChildNodes)
                {
                    if (trksegNode.Name == "trkseg")
                    {
                        if (DateTime.Parse(trksegNode.FirstChild.LastChild.InnerText) > waitingDateTime
                            || DateTime.Parse(trksegNode.LastChild.LastChild.InnerText) < waitingDateTime)
                        {
                            continue;
                        }

                        try
                        {
                            trksegNode.RemoveChild(node);
                        }
                        catch (Exception ex)
                        {
                            throw new TrackPointNotDeleteException("TrackPointの削除に失敗しました", ex);
                        }
                        break;
                    }
                }
            }

            Doc.Save(GpxFileName.Replace(".gpx", "_converted") + ".gpx");
        }
    }
}
