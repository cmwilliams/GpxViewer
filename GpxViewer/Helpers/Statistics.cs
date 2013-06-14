using System;
using System.Collections.Generic;
using System.Linq;
using GpxViewer.Models;

namespace GpxViewer.Helpers
{
    public class Statistics
    {
        private Activity CalculateStatistics(Activity activity)
        {
            return activity;
        }

        private DateTime? StartTime(Activity activity)
        {
            return activity.Points.Any() ? activity.Points.First().Time : null;
        }



        //private Activity[] PointPairs(Activity activity)
        //{
        //    Di
        //    for (var i = 0; i < activity.Points.Count-1; i++)
        //    {
        //       pairs[i] =
                
        //    }

        //}



        private static double DegreesToRadian(decimal? degrees)
        {
            return Convert.ToDouble(degrees) * (Math.PI / 180);
        }

        private static double EarthRadius(Point point)
        {
            const double a = 3963.19; // EARTH_EQUATORIAL_RADIUS
            const double b = 3949.9; // EARTH_POLAR_RADIUS
            if (point.Latitude != null)
            {
                var latitude = (double)point.Latitude;
                var c = Math.Sqrt(Math.Pow(a, 2) * Math.Pow(Math.Cos(latitude), 2) +
                                  (Math.Pow(b, 2)*Math.Pow(Math.Sin(latitude), 2))/Math.Pow(a*Math.Cos(latitude), 2) +
                                  Math.Pow(b*Math.Sin(latitude), 2));
                return c;
            }
            return 0;
        }

        private static double DistanceBetween(Point fromPoint, Point toPoint)
        {
            if (fromPoint.Latitude == null || fromPoint.Longitude == null || toPoint.Latitude == null ||
                toPoint.Longitude == null)
                return 0;


            var deltaLatitude = DegreesToRadian(toPoint.Latitude - fromPoint.Latitude);
            var deltaLongitude = DegreesToRadian(toPoint.Longitude - fromPoint.Longitude);
            var fromLatitude = DegreesToRadian(fromPoint.Latitude);
            var toLatitude = DegreesToRadian(toPoint.Latitude);

            var a = Math.Sin(deltaLatitude/2)*Math.Sin(deltaLatitude/2) +
                    Math.Sin(deltaLongitude/2)*Math.Sin(deltaLongitude/2)*Math.Cos(fromLatitude)*Math.Cos(toLatitude);

            var c = 2*Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EarthRadius(fromPoint)*c;
        }

        private static double SpeedBetween(Point startPoint, Point endPoint)
        {
            var time = TimeBetween(startPoint, endPoint);
            if (time == 0)
                return 0;

            var distance = DistanceBetween(startPoint, endPoint);
            if (distance == 0)
                return 0;

            return distance/time;

        }

        private double PaceBetween(Point startPoint, Point endPoint)
        {
            if (!IsActiveBetween(startPoint, endPoint))
                return 0;

            var time = TimeBetween(startPoint, endPoint);
            if (time == 0)
                return 0;

            var distance = DistanceBetween(startPoint, endPoint);
            if (distance == 0)
                return 0;

            return time/distance;
        }

        private static int TimeBetween(Point startPoint, Point endPoint)
        {
            if (startPoint.Time == null && endPoint.Time == null)
                return 0;

            var endPointTime = Convert.ToDateTime(endPoint.Time);
            var startPointTime = Convert.ToDateTime(startPoint.Time);
            return endPointTime.Subtract(startPointTime).Seconds;
        }

        private static decimal ElevationBetween(Point startPoint, Point endPoint)
        {
            if (startPoint.Elevation == null && endPoint.Elevation == null)
                return 0;

            return (decimal) (endPoint.Elevation - startPoint.Elevation);
        }

        private static bool IsActiveBetween(Point startPoint, Point endPoint)
        {
            return SpeedBetween(startPoint, endPoint) > 0.5;
        }
        
        private bool IsAscendingBetween(Point startPoint, Point endPoint)
        {
            return (ElevationBetween(startPoint, endPoint) / (decimal) DistanceBetween(startPoint, endPoint)) > (decimal) 0.003;
        }

        private bool IsDescendingBetween(Point startPoint, Point endPoint)
        {
            return (ElevationBetween(endPoint, startPoint)/(decimal) DistanceBetween(startPoint, endPoint)) > (decimal) 0.003;
        }

        private bool IsFlatBetween(Point startPoint, Point endPoint)
        {
            return Math.Abs(ElevationBetween(startPoint, endPoint)/(decimal) DistanceBetween(startPoint, endPoint)) <= (decimal) 0.003;
        }
    }
}