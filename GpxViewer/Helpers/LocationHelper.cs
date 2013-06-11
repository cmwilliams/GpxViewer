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
            var polylinePoints = points.OrderBy(p => p.PointCreatedAt).Select(point => new PolylineCoordinate {Latitude = point.Latitude, Longitude = point.Longitude}).ToList();

            var polylineForPoints = Polyline.EncodePoints(polylinePoints);

            if (!string.IsNullOrEmpty(polylineForPoints))
            {
                polylineForPoints = polylineForPoints.Replace("\\", "\\\\");
            }

            return polylineForPoints;
        }
    }
}