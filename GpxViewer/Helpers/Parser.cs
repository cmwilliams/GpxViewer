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
        public static IList<Activity> ParseGpx(Stream document)
        {
            
            XDocument gpxDoc;
            using (var reader = XmlReader.Create(document))
            {
               
                gpxDoc = XDocument.Load(reader);
               
            }

            if (gpxDoc.Root != null)
            {
                var gpxNamespace = gpxDoc.Root.GetDefaultNamespace();
                var gpxtpx = XNamespace.Get("http://www.garmin.com/xmlschemas/TrackPointExtension/v1");
                var activities = (from trk in gpxDoc.Descendants(gpxNamespace + "trk")
                                  select new Activity
                                             {
                                                 Name =
                                                     trk.Element(gpxNamespace + "name") != null
                                                         ? trk.Element(gpxNamespace + "name").Value
                                                         : null,
                                                 Description =
                                                     trk.Element(gpxNamespace + "desc") != null
                                                         ? trk.Element(gpxNamespace + "desc").Value
                                                         : null,
                                                 Points = (from pt in trk.Descendants(gpxNamespace + "trkpt")
                                                           select new Point
                                                                      {
                                                                          Latitude =
                                                                              Convert.ToDecimal(pt.Attribute("lat").Value),
                                                                          Longitude =
                                                                              Convert.ToDecimal(pt.Attribute("lon").Value),
                                                                          Elevation =
                                                                              pt.Element(gpxNamespace + "ele") != null
                                                                                  ? Convert.ToDecimal(
                                                                                      pt.Element(gpxNamespace + "ele").Value)
                                                                                  : (decimal?) null,
                                                                          Time =
                                                                              pt.Element(gpxNamespace + "time") != null
                                                                                  ? Convert.ToDateTime(
                                                                                      pt.Element(gpxNamespace + "time").Value)
                                                                                  : (DateTime?) null,
                                                                          HeartRate =
                                                                              pt.Element(gpxNamespace + "extensions") != null
                                                                                  ? pt.Element(gpxNamespace + "extensions")
                                                                                      .Element(gpxtpx + "TrackPointExtension") !=null
                                                                                        ? pt.Element(gpxNamespace + "extensions").Element(gpxtpx + "TrackPointExtension").Element(gpxtpx + "hr") != null
                                                                                              ? Convert.ToInt32(pt.Element(gpxNamespace + "extensions").Element(gpxtpx + "TrackPointExtension").Element(gpxtpx + "hr").Value) : (int?)null : (int?)null : (int?)null,

                                                                          Cadence =
                                                                              pt.Element(gpxNamespace + "extensions") != null
                                                                                  ? pt.Element(gpxNamespace + "extensions")
                                                                                      .Element(gpxtpx + "TrackPointExtension") != null
                                                                                        ? pt.Element(gpxNamespace + "extensions").Element(gpxtpx + "TrackPointExtension").Element(gpxtpx + "cad") != null
                                                                                              ? Convert.ToInt32(pt.Element(gpxNamespace + "extensions").Element(gpxtpx + "TrackPointExtension").Element(gpxtpx + "cad").Value) : (int?)null : (int?)null : (int?)null,
                                                                      }
                                                          ).ToList()
                                             });

                return activities.ToList();
            }

            return new List<Activity>();
        }
    }
}