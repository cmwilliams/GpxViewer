using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GpxViewer.Models;

namespace GpxViewer.Helpers
{
    public static class Statistics
    {
        public static Activity CalculateStatistics(Activity activity)
        {
            activity.ActivityDate = StartTime(activity);
            activity.Distance = Distance(activity);
            //activity.AscendingDistance = AscendingDistance(activity);
            //activity.DescendingDistance = DescendingDistance(activity);
            //activity.FlatDistance = FlatDistance(activity);
            //activity.ElevationGain = ElevationGain(activity);
            //activity.ElevationLoss = ElevationLoss(activity);
            //activity.ElevationChange = ElevationChange(activity);
            //activity.MaximumElevation = MaximumElevation(activity);
            //activity.MinimumElevation = MinimumElevation(activity);
            activity.Duration = Duration(activity);
            activity.ActiveDuration = ActiveDuration(activity);
            //activity.AscendingDuration = AscendingDuration(activity);
            //activity.DescendingDuration = DescendingDuration(activity);
            //activity.FlatDuration = FlatDuration(activity);

            return activity;
        }

        private static DateTime? StartTime(Activity activity)
        {
            return activity.Points.Any() ? activity.Points.First().Time : null;
        }

        public static decimal? Distance(Activity activity)
        {
            decimal? total = 0;
            var pointPairs = PointPairs(activity);
            foreach (var pointPair in pointPairs)
            {
                pointPair.FirstPoint.Distance = total;
                pointPair.SecondPoint.Distance = total + (decimal?) DistanceBetween(pointPair.FirstPoint, pointPair.SecondPoint);
                total = pointPair.SecondPoint.Distance;
            }
            return total;
        }

        public static decimal? AscendingDistance(Activity activity)
        {
            decimal? distanceTotal = 0;

            var pointPairs = AscendingPointPairs(activity);
            for (var i = 0; i < pointPairs.Count - 1; i++)
            {
                for (var j = 0; j < ((ArrayList)pointPairs[i]).Count - 1; j++)
                {
                    decimal? total = 0;
                    var firstPoint = (Point)((ArrayList)pointPairs[i])[j];
                    var secondPoint = (Point)((ArrayList)pointPairs[i])[j + 1];

                    
                    distanceTotal += total + (decimal)DistanceBetween(firstPoint, secondPoint);
                }

            }

            return distanceTotal;
        }

        public static decimal? DescendingDistance(Activity activity)
        {
            decimal? distanceTotal = 0;

            var pointPairs = DescendingPointPairs(activity);
            for (var i = 0; i < pointPairs.Count - 1; i++)
            {
                for (var j = 0; j < ((ArrayList)pointPairs[i]).Count - 1; j++)
                {
                    decimal? total = 0;
                    var firstPoint = (Point)((ArrayList)pointPairs[i])[j];
                    var secondPoint = (Point)((ArrayList)pointPairs[i])[j + 1];


                    distanceTotal += total + (decimal)DistanceBetween(firstPoint, secondPoint);
                }

            }

            return distanceTotal;
        }

        public static decimal? FlatDistance(Activity activity)
        {
            decimal? distanceTotal = 0;

            var pointPairs = FlatPointPairs(activity);
            for (var i = 0; i < pointPairs.Count - 1; i++)
            {
                for (var j = 0; j < ((ArrayList)pointPairs[i]).Count - 1; j++)
                {
                    decimal? total = 0;
                    var firstPoint = (Point)((ArrayList)pointPairs[i])[j];
                    var secondPoint = (Point)((ArrayList)pointPairs[i])[j + 1];


                    distanceTotal += total + (decimal)DistanceBetween(firstPoint, secondPoint);
                }

            }

            return distanceTotal;
        }

        public static decimal? ElevationGain(Activity activity)
        {
            decimal? elevationTotal = 0;

            var pointPairs = AscendingPointPairs(activity);
            for (var i = 0; i < pointPairs.Count - 1; i++)
            {
                for (var j = 0; j < ((ArrayList)pointPairs[i]).Count - 1; j++)
                {
                    decimal? total = 0;
                    var firstPoint = (Point)((ArrayList)pointPairs[i])[j];
                    var secondPoint = (Point)((ArrayList)pointPairs[i])[j + 1];


                    elevationTotal += total + (decimal)ElevationBetween(firstPoint, secondPoint);
                }

            }

            return elevationTotal;
        }

        public static decimal? ElevationLoss(Activity activity)
        {
            decimal? elevationTotal = 0;

            var pointPairs = DescendingPointPairs(activity);
            for (var i = 0; i < pointPairs.Count - 1; i++)
            {
                for (var j = 0; j < ((ArrayList)pointPairs[i]).Count - 1; j++)
                {
                    decimal? total = 0;
                    var firstPoint = (Point)((ArrayList)pointPairs[i])[j];
                    var secondPoint = (Point)((ArrayList)pointPairs[i])[j + 1];


                    elevationTotal += total + (decimal)ElevationBetween(firstPoint, secondPoint);
                }

            }

            return elevationTotal;
        }

        public static decimal? ElevationChange(Activity activity)
        {
            return MaximumElevation(activity) - MinimumElevation(activity);
        }

        public static decimal? MaximumElevation(Activity activity)
        {
            return activity.Points.Any() ? activity.Points.Max(p => p.Elevation) : null;
        }

        public static decimal? MinimumElevation(Activity activity)
        {
            return activity.Points.Any() ? activity.Points.Min(p => p.Elevation) : null;
        }


       
        public static decimal? Duration(Activity activity)
        {
            if (activity.Points.Count < 2)
            {
                foreach (var point in activity.Points)
                {
                    point.Duration = 0;
                }
                return 0;
            }
            
            
         

            double total = 0;

            var pointPairs = PointPairs(activity);
            foreach (var pointPair in pointPairs)
            {
                pointPair.FirstPoint.Duration = (decimal?) total;
                pointPair.SecondPoint.Duration = (decimal?) (total + TimeBetween(pointPair.FirstPoint, pointPair.SecondPoint));
                total += TimeBetween(pointPair.FirstPoint, pointPair.SecondPoint);
            }


            
            //for (var i = 0; i < pointPairs.Count - 1; i++)
            //{
            //    for (var j = 0; j < ((ArrayList)pointPairs[i]).Count-1; j++)
            //    {
            //        var firstPoint = (Point)((ArrayList)pointPairs[i])[j];
            //        var secondPoint = (Point)((ArrayList)pointPairs[i])[j + 1];

            //        firstPoint.Duration = total;
            //        secondPoint.Duration = total + TimeBetween(firstPoint, secondPoint);
            //        total = secondPoint.Duration;
            //    }
            //}

            return (decimal?) total;
        }

        public static decimal? ActiveDuration(Activity activity)
        {
            if (activity.Points.Count < 2)
            {
                foreach (var point in activity.Points)
                {
                    point.Duration = 0;
                }
                return 0;
            }


            double total = 0;

            var pointPairs = PointPairs(activity);
            foreach (var pointPair in pointPairs)
            {
                if (IsActiveBetween(pointPair.FirstPoint, pointPair.SecondPoint))
                {
                    pointPair.FirstPoint.ActiveDuration = (decimal?) total;
                    pointPair.SecondPoint.ActiveDuration =
                        (decimal?) (total + TimeBetween(pointPair.FirstPoint, pointPair.SecondPoint));
                    total += TimeBetween(pointPair.FirstPoint, pointPair.SecondPoint);
                }
            }

            return (decimal?)total;

           
        }

        public static decimal? AscendingDuration(Activity activity)
        {
            
            decimal? durationTotal = 0;

            var pointPairs = PointPairs(activity);
            //for (var i = 0; i < pointPairs.Count - 1; i++)
            //{
            //    for (var j = 0; j < ((ArrayList)pointPairs[i]).Count - 1; j++)
            //    {

            //        var firstPoint = (Point)((ArrayList)pointPairs[i])[j];
            //        var secondPoint = (Point)((ArrayList)pointPairs[i])[j + 1];

            //        if (IsActiveBetween(firstPoint, secondPoint) && IsAscendingBetween(firstPoint,secondPoint))
            //        {
            //            decimal? total = 0;
            //            firstPoint.Duration = total;
            //            secondPoint.Duration = total + TimeBetween(firstPoint, secondPoint);
            //            durationTotal += firstPoint.Duration + secondPoint.Duration;
            //        }
            //    }

            //}


            return durationTotal;
        }

        public static decimal? DescendingDuration(Activity activity)
        {

            decimal? durationTotal = 0;

            var pointPairs = PointPairs(activity);
            //for (var i = 0; i < pointPairs.Count - 1; i++)
            //{
            //    for (var j = 0; j < ((ArrayList)pointPairs[i]).Count - 1; j++)
            //    {

            //        var firstPoint = (Point)((ArrayList)pointPairs[i])[j];
            //        var secondPoint = (Point)((ArrayList)pointPairs[i])[j + 1];

            //        if (IsActiveBetween(firstPoint, secondPoint) && IsDescendingBetween(firstPoint, secondPoint))
            //        {
            //            decimal? total = 0;
            //            firstPoint.Duration = total;
            //            secondPoint.Duration = total + TimeBetween(firstPoint, secondPoint);
            //            durationTotal += firstPoint.Duration + secondPoint.Duration;
            //        }
            //    }

            //}


            return durationTotal;
        }

        public static decimal? FlatDuration(Activity activity)
        {

            decimal? durationTotal = 0;

            var pointPairs = PointPairs(activity);
            //for (var i = 0; i < pointPairs.Count - 1; i++)
            //{
            //    for (var j = 0; j < ((ArrayList)pointPairs[i]).Count - 1; j++)
            //    {

            //        var firstPoint = (Point)((ArrayList)pointPairs[i])[j];
            //        var secondPoint = (Point)((ArrayList)pointPairs[i])[j + 1];

            //        if (IsActiveBetween(firstPoint, secondPoint) && IsFlatBetween(firstPoint, secondPoint))
            //        {
            //            decimal? total = 0;
            //            firstPoint.Duration = total;
            //            secondPoint.Duration = total + TimeBetween(firstPoint, secondPoint);
            //            durationTotal += firstPoint.Duration + secondPoint.Duration;
            //        }
            //    }

            //}


            return durationTotal;
        }

        #region Private

       

        public static void Each<T>(this IEnumerable<T> ie, Action<T, int> action)
        {
            var i = 0;
            foreach (var e in ie) action(e, i++);
        }

        private static List<PointPair> PointPairs(Activity activity)
        {
            var points = activity.Points.OrderBy(p => p.Time).ToList();
            var pairs = new List<PointPair>();
            points.Each((point, index) =>
                {
                    if (index >= points.Count() - 1) return;
                    var pair = new PointPair
                        {
                            FirstPoint = point,
                            SecondPoint = points[index + 1]
                        };
                    pairs.Add(pair);
                });


            return pairs;
        }

        private static ArrayList AscendingPointPairs(Activity activity)
        {
            var pairs = new ArrayList();

            var pointPairs = PointPairs(activity);
            //for (var i = 0; i < pointPairs.Count-1; i++)
            //{
            //    for (var j = 0; j < ((ArrayList)pointPairs[i]).Count - 1; j++)
            //    {
                    
            //        var firstPoint = (Point)((ArrayList)pointPairs[i])[j];
            //        var secondPoint = (Point)((ArrayList)pointPairs[i])[j + 1];

            //        if (IsAscendingBetween(firstPoint, secondPoint))
            //        {
            //            var pair = new ArrayList { firstPoint, secondPoint };
            //            pairs.Add(pair);
            //        }
            //    }
            //}
          
            return pairs;
        }

        private static ArrayList DescendingPointPairs(Activity activity)
        {
            var pairs = new ArrayList();

            var pointPairs = PointPairs(activity);
            //for (var i = 0; i < pointPairs.Count - 1; i++)
            //{
            //    for (var j = 0; j < ((ArrayList)pointPairs[i]).Count - 1; j++)
            //    {

            //        var firstPoint = (Point)((ArrayList)pointPairs[i])[j];
            //        var secondPoint = (Point)((ArrayList)pointPairs[i])[j + 1];

            //        if (IsDescendingBetween(firstPoint, secondPoint))
            //        {
            //            var pair = new ArrayList { firstPoint, secondPoint };
            //            pairs.Add(pair);
            //        }
            //    }
            //}

            return pairs;
        }

        private static ArrayList FlatPointPairs(Activity activity)
        {
            var pairs = new ArrayList();

            var pointPairs = PointPairs(activity);
            //for (var i = 0; i < pointPairs.Count - 1; i++)
            //{
            //    for (var j = 0; j < ((ArrayList)pointPairs[i]).Count - 1; j++)
            //    {

            //        var firstPoint = (Point)((ArrayList)pointPairs[i])[j];
            //        var secondPoint = (Point)((ArrayList)pointPairs[i])[j + 1];

            //        if (IsActiveBetween(firstPoint, secondPoint) && IsFlatBetween(firstPoint, secondPoint))
            //        {
            //            var pair = new ArrayList { firstPoint, secondPoint };
            //            pairs.Add(pair);
            //        }
            //    }
            //}

            return pairs;
        }

        private static double DegreesToRadian(decimal? degrees)
        {
            return Convert.ToDouble(degrees) * (Math.PI / 180);
        }

        private static double EarthRadius(Point point)
        {
            const double a = 6378137.0; // EARTH_EQUATORIAL_RADIUS
            const double b = 6356752.3; // EARTH_POLAR_RADIUS
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

        private static double PaceBetween(Point startPoint, Point endPoint)
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

        private static double TimeBetween(Point startPoint, Point endPoint)
        {
            if (startPoint.Time == null && endPoint.Time == null)
                return 0;

            var endPointTime = Convert.ToDateTime(endPoint.Time);
            var startPointTime = Convert.ToDateTime(startPoint.Time);
            return  endPointTime.Subtract(startPointTime).TotalSeconds;
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
        
        private static bool IsAscendingBetween(Point startPoint, Point endPoint)
        {
            return (ElevationBetween(startPoint, endPoint) / (decimal) DistanceBetween(startPoint, endPoint)) > (decimal) 0.003;
        }

        private static bool IsDescendingBetween(Point startPoint, Point endPoint)
        {
            return (ElevationBetween(endPoint, startPoint)/(decimal) DistanceBetween(startPoint, endPoint)) > (decimal) 0.003;
        }

        private static bool IsFlatBetween(Point startPoint, Point endPoint)
        {
            return Math.Abs(ElevationBetween(startPoint, endPoint)/(decimal) DistanceBetween(startPoint, endPoint)) <= (decimal) 0.003;
        }

        #endregion
    }
}