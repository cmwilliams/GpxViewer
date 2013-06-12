using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}