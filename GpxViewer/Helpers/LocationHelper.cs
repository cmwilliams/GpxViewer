using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using GpxViewer.Models;
using Polylines;

namespace GpxViewer.Helpers
{
    public static class LocationHelper
    {
        public static string GetPolyline(this IEnumerable<Point> points)
        {
            var polylinePoints =
                points.OrderBy(p => p.PointCreatedAt)
                      .Select(
                          point =>
                          new PolylineCoordinate
                              {
                                  Latitude = Convert.ToDouble(point.Latitude),
                                  Longitude = Convert.ToDouble(point.Longitude)
                              })
                      .ToList();

            var polylineForPoints = Polyline.EncodePoints(polylinePoints);

            if (!string.IsNullOrEmpty(polylineForPoints))
            {
                polylineForPoints = polylineForPoints.Replace("\\", "\\\\");
            }

            return polylineForPoints;
        }

        public static ElevationProfile GetElevation(this IEnumerable<Point> points)
        {
            ElevationProfile elevationProfile = null;

            var enumerable = points as IList<Point> ?? points.ToList();
            if (enumerable.Any())
            {
                elevationProfile = new ElevationProfile();
                double min = 1000000;
                double max = 0;
                double gain = 0;
                double loss = 0;
                double last = 0;

                foreach (var cur in enumerable.Select(point => point.Elevation != null ? (float) point.Elevation : 0))
                {
                    if (cur > max)
                    {
                        max = cur;
                    }
                    if (cur < min)
                    {
                        min = cur;
                    }

                    if (last != 0)
                    {
                        if (cur > last)
                        {
                            gain = gain + (cur - last);
                        }
                        else if (cur < last)
                        {
                            loss = loss + (last - cur);
                        }
                    }

                    last = cur;
                }

                elevationProfile.MaxElevation = Math.Round(((float) (max*3.2808399)),0);
                elevationProfile.MinElevation = Math.Round(((float)(min * 3.2808399)), 0);
                elevationProfile.Gain = Math.Round(((float)(gain * 3.2808399)), 0);
                elevationProfile.Loss = Math.Round(((float)(loss * 3.2808399)), 0);
            }


            return elevationProfile;

        }

        public static double Distance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            const double r = 3960;
 
            var dLat = ToRadian(latitude2 - latitude1);
            var dLon = ToRadian(longitude2 - longitude1);

            var a = Math.Sin(dLat/2)*Math.Sin(dLat/2) +
                    Math.Cos(ToRadian(latitude1))*Math.Cos(ToRadian(latitude2))*
                    Math.Sin(dLon/2)*Math.Sin(dLon/2);
            var c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            var d = r * c;

            return d;
        }

        private static double ToRadian(double val)
        {
            return (Math.PI/180)*val;
        }

        public static double GetDistance(this IEnumerable<Point> points)
        {
            double distance = 0;
            var segmentPoints = points.ToList();
            for (var i = 0; i < segmentPoints.Count - 1; i++)
            {
                distance += Distance(Convert.ToDouble(segmentPoints[i].Latitude),
                                     Convert.ToDouble(segmentPoints[i].Longitude),
                                     Convert.ToDouble(segmentPoints[i + 1].Latitude),
                                     Convert.ToDouble(segmentPoints[i + 1].Longitude));


            }
            return Math.Round(distance,2);
        }

        public static double GetAvgCadence(this IEnumerable<Point> points)
        {
            var cadence = points.Where(p => p.Cadence != null && p.Cadence > 0).Average(p => p.Cadence);
            if (cadence == null)
            {
                return 0;
            }

            return Math.Round(Convert.ToDouble(cadence),0);

        }

        public static double GetMaxCadence(this IEnumerable<Point> points)
        {
            var cadence = points.Where(p => p.Cadence != null && p.Cadence > 0).Max(p => p.Cadence);
            if (cadence == null)
            {
                return 0;
            }

            return Math.Round(Convert.ToDouble(cadence), 0);

        }



        private static XDocument LoadFromStream(Stream stream)
        {
            using (var reader = XmlReader.Create(stream))
            {
                return XDocument.Load(reader);
            }
        }

        private static XNamespace GetGpxNameSpace()
        {
            var gpx = XNamespace.Get("http://www.topografix.com/GPX/1/1");
            return gpx;
        }

        private static XNamespace GetGpxtpxNameSpace()
        {
            var gpxtpx = XNamespace.Get("http://www.garmin.com/xmlschemas/TrackPointExtension/v1");
            return gpxtpx;
        }

        private static double Double(string value)
        {
            return double.Parse(value, CultureInfo.InvariantCulture);
        }

        private static string DefaultStringValue(XContainer element, XNamespace ns, string elementName)
        {
            var xElement = element.Element(ns + elementName);
            return xElement != null ? xElement.Value : null;
        }

        private static double? DefaultDoubleValue(XContainer element, XNamespace ns, string elementName)
        {
            var xElement = element.Element(ns + elementName);
            return xElement != null ? Convert.ToDouble(xElement.Value) : (double?)null;
        }

        private static int? DefaultIntValue(XContainer element, XNamespace ns, string elementName)
        {
            var xElement = element.Element(ns + elementName);
            return xElement != null ? Convert.ToInt32(xElement.Value) : (int?)null;
        }

        private static DateTime? DefaultDateTimeValue(XContainer element, XNamespace ns, string elementName)
        {
            var xElement = element.Element(ns + elementName);
            return xElement != null ? Convert.ToDateTime(xElement.Value) : (DateTime?)null;
        }

        public static Track ParseGpx(Track exitingTrack, Stream document)
        {
            var gpxDoc = LoadFromStream(document);
            var gpx = GetGpxNameSpace();
            var gpxtpx = GetGpxtpxNameSpace();

            var tracks = from track in gpxDoc.Descendants(gpx + "trk")
                         select new
                         {
                             Name = DefaultStringValue(track, gpx, "name"),
                             Description = DefaultStringValue(track, gpx, "desc"),
                             Segments = (from trkSegment in track.Descendants(gpx + "trkseg")
                                         select new
                                         {
                                             TrackSegment = trkSegment,
                                             Points = (from trackpoint in trkSegment.Descendants(gpx + "trkpt")
                                                       select new
                                                       {
                                                           Lat = Double(trackpoint.Attribute("lat").Value),
                                                           Lng = Double(trackpoint.Attribute("lon").Value),
                                                           Ele = DefaultDoubleValue(trackpoint, gpx, "ele"),
                                                           Time = DefaultDateTimeValue(trackpoint, gpx, "time"),
                                                           Extensions = (
                                                                  from ext in trackpoint.Descendants(gpx + "extensions").Descendants(gpxtpx + "TrackPointExtension")
                                                                  select new
                                                                  {
                                                                      Cad = DefaultIntValue(ext, gpxtpx, "cad"),
                                                                      Hr = DefaultIntValue(ext, gpxtpx, "hr"),
                                                                  }).SingleOrDefault()
                                                       })
                                         })
                         };

            foreach (var trk in tracks)
            {
                exitingTrack.Name = trk.Name;
                exitingTrack.Description = trk.Description;

                foreach (var segment in trk.Segments)
                {
                    var trackSegment = new TrackSegment { Points = new Collection<Point>() };

                    foreach (var point in segment.Points)
                    {
                        trackSegment.Points.Add(new Point
                        {
                            Elevation = point.Ele,
                            Latitude = point.Lat,
                            Longitude = point.Lng,
                            PointCreatedAt = point.Time,
                            Cadence = point.Extensions != null ? point.Extensions.Cad : null,
                            HeartRate = point.Extensions != null ? point.Extensions.Hr : null
                        });
                    }

                    exitingTrack.TrackSegments.Add(trackSegment);
                }
            }
            return exitingTrack;
        }
    }
}