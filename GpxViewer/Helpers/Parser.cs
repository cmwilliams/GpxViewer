using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using GpxViewer.Models;

namespace GpxViewer.Helpers
{
    public static class Parser
    {
        private static XDocument LoadFromStream(Stream stream)
        {
            using (var reader = XmlReader.Create(stream))
            {
                return XDocument.Load(reader);
            }
        }

        public static IList<Activity> ParseGpx(Stream document)
        {
            var gpxDoc = LoadFromStream(document);
            var gpx = XNamespace.Get("http://www.topografix.com/GPX/1/1");
            var gpxtpx = XNamespace.Get("http://www.garmin.com/xmlschemas/TrackPointExtension/v1");

            var activities = (from trk in gpxDoc.Descendants(gpx + "trk")
                              select new Activity
                                         {
                                             Name =
                                                 trk.Element(gpx + "name") != null
                                                     ? trk.Element(gpx + "name").Value
                                                     : null,
                                             Description =
                                                 trk.Element(gpx + "desc") != null
                                                     ? trk.Element(gpx + "desc").Value
                                                     : null,
                                             Points = (from pt in trk.Descendants(gpx + "trkpt")
                                                       select new Point
                                                                  {
                                                                      Latitude =
                                                                          Convert.ToDecimal(pt.Attribute("lat").Value),
                                                                      Longitude =
                                                                          Convert.ToDecimal(pt.Attribute("lon").Value),
                                                                      Elevation =
                                                                          pt.Element(gpx + "ele") != null
                                                                              ? Convert.ToDecimal(
                                                                                  pt.Element(gpx + "ele").Value)
                                                                              : (decimal?) null,
                                                                      Time =
                                                                          pt.Element(gpx + "time") != null
                                                                              ? Convert.ToDateTime(
                                                                                  pt.Element(gpx + "time").Value)
                                                                              : (DateTime?) null,
                                                                      HeartRate =
                                                                          pt.Element(gpx + "extensions") != null
                                                                              ? pt.Element(gpx + "extensions")
                                                                                  .Element(gpxtpx + "TrackPointExtension") !=null
                                                                                    ? pt.Element(gpx + "extensions") .Element(gpxtpx +"TrackPointExtension") .Element(gpxtpx + "hr") != null
                                                                                          ? Convert.ToInt32(pt.Element(gpx + "extensions").Element(gpxtpx + "TrackPointExtension").Element(gpxtpx + "hr").Value) : (int?)null : (int?)null : (int?)null,

                                                                      Cadence =
                                                                           pt.Element(gpx + "extensions") != null
                                                                              ? pt.Element(gpx + "extensions")
                                                                                  .Element(gpxtpx + "TrackPointExtension") != null
                                                                                    ? pt.Element(gpx + "extensions").Element(gpxtpx + "TrackPointExtension").Element(gpxtpx + "cad") != null
                                                                                          ? Convert.ToInt32(pt.Element(gpx + "extensions").Element(gpxtpx + "TrackPointExtension").Element(gpxtpx + "cad").Value) : (int?)null : (int?)null : (int?)null,
                                                                  }
                                                      ).ToList()
                                         });

            return activities.ToList();
        }
    }
}