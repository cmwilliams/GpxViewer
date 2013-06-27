using System;
using System.Collections.Generic;
using System.Linq;
using GpxViewer.Models;

namespace GpxViewer.Helpers
{
    public static class NumericExtensions
    {
        public static double ToRadians(this double val)
        {
            return (Math.PI / 180) * val;
        }
    }

    public static class Statistics
    {
        public static Activity CalculateStatistics(Activity activity)
        {
            activity.ActivityDate = StartTime(activity);
            activity.Distance = Distance(activity);
            activity.AscendingDistance = AscendingDistance(activity);
            activity.DescendingDistance = DescendingDistance(activity);
            activity.FlatDistance = FlatDistance(activity);
            activity.ElevationGain = ElevationGain(activity);
            activity.ElevationLoss = ElevationLoss(activity);
            activity.ElevationChange = ElevationChange(activity);
            activity.MaximumElevation = MaximumElevation(activity);
            activity.MinimumElevation = MinimumElevation(activity);
            activity.Duration = Duration(activity);
            activity.ActiveDuration = ActiveDuration(activity);
            activity.AscendingDuration = AscendingDuration(activity);
            activity.DescendingDuration = DescendingDuration(activity);
            activity.FlatDuration = FlatDuration(activity);
            activity.AveragePace = AveragePace(activity);
            activity.AverageAscendingPace = AverageAscendingPace(activity);
            activity.AverageDescendingPace = AverageDescendingPace(activity);
            activity.AverageFlatPace = AverageFlatPace(activity);
            activity.AverageSpeed = AverageSpeed(activity);
            activity.AverageAscendingSpeed = AverageAscendingSpeed(activity);
            activity.AverageDescendingSpeed = AverageDescendingSpeed(activity);
            activity.AverageFlatSpeed = AverageFlatSpeed(activity);
            activity.MaximumSpeed = MaximumSpeed(activity);
            activity.AverageHeartRate = AverageHeartRate(activity);
            activity.MaximumHeartRate = MaximumHeartRate(activity);
            activity.MinimumHeartRate = MinimumHeartRate(activity);
            activity.AverageCadence = AverageCadence(activity);
            activity.MinimumCadence = MinimumCadence(activity);
            activity.MaximumCadence = MaximumCadence(activity);

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
            var pointPairs = AscendingPointPairs(activity);
            return pointPairs.Aggregate<PointPair, decimal?>(0, (current, pointPair) => current + (decimal?) DistanceBetween(pointPair.FirstPoint, pointPair.SecondPoint));
        }

        public static decimal? DescendingDistance(Activity activity)
        {
            var pointPairs = DescendingPointPairs(activity);
            return pointPairs.Aggregate<PointPair, decimal?>(0, (current, pointPair) => current + (decimal?)DistanceBetween(pointPair.FirstPoint, pointPair.SecondPoint));
        }

        public static decimal? FlatDistance(Activity activity)
        {
            var pointPairs = FlatPointPairs(activity);
            return pointPairs.Aggregate<PointPair, decimal?>(0, (current, pointPair) => current + (decimal?)DistanceBetween(pointPair.FirstPoint, pointPair.SecondPoint));

        }

        public static decimal? ElevationGain(Activity activity)
        {
            var pointPairs = AscendingPointPairs(activity);
            return pointPairs.Aggregate<PointPair, decimal?>(0, (current, pointPair) => current + (decimal?)ElevationBetween(pointPair.FirstPoint, pointPair.SecondPoint));
        }

        public static decimal? ElevationLoss(Activity activity)
        {
            var pointPairs = DescendingPointPairs(activity);
            return pointPairs.Aggregate<PointPair, decimal?>(0, (current, pointPair) => current + (decimal?)ElevationBetween(pointPair.FirstPoint, pointPair.SecondPoint)) * -1;
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
                total = (double) pointPair.SecondPoint.Duration;
            }


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
            foreach (var pointPair in pointPairs.Where(pointPair => IsActiveBetween(pointPair.FirstPoint, pointPair.SecondPoint)))
            {
                pointPair.FirstPoint.ActiveDuration = (decimal?) total;
                pointPair.SecondPoint.ActiveDuration =
                    (decimal?) (total + TimeBetween(pointPair.FirstPoint, pointPair.SecondPoint));
                total += TimeBetween(pointPair.FirstPoint, pointPair.SecondPoint);
            }

            return (decimal?)total;

           
        }

        public static decimal? AscendingDuration(Activity activity)
        {
            var pointPairs = PointPairs(activity);
            var total =
                pointPairs.Where(
                    pointPair =>
                    IsActiveBetween(pointPair.FirstPoint, pointPair.SecondPoint) &&
                    IsAscendingBetween(pointPair.FirstPoint, pointPair.SecondPoint))
                          .Aggregate<PointPair, double>(0,
                                                        (current, pointPair) =>
                                                        (current +
                                                         TimeBetween(pointPair.FirstPoint, pointPair.SecondPoint)));

            return (decimal?)total;
        }

        public static decimal? DescendingDuration(Activity activity)
        {
            var pointPairs = PointPairs(activity);
            var total =
                pointPairs.Where(
                    pointPair =>
                    IsActiveBetween(pointPair.FirstPoint, pointPair.SecondPoint) &&
                    IsDescendingBetween(pointPair.FirstPoint, pointPair.SecondPoint))
                          .Aggregate<PointPair, double>(0,
                                                        (current, pointPair) =>
                                                        (current +
                                                         TimeBetween(pointPair.FirstPoint, pointPair.SecondPoint)));

            return (decimal?)total;
        }

        public static decimal? FlatDuration(Activity activity)
        {
            var pointPairs = PointPairs(activity);
            var total =
                pointPairs.Where(
                    pointPair =>
                    IsActiveBetween(pointPair.FirstPoint, pointPair.SecondPoint) &&
                    IsFlatBetween(pointPair.FirstPoint, pointPair.SecondPoint))
                          .Aggregate<PointPair, double>(0,
                                                        (current, pointPair) =>
                                                        (current +
                                                         TimeBetween(pointPair.FirstPoint, pointPair.SecondPoint)));

            return (decimal?)total;
        }

        public static decimal? AveragePace(Activity activity)
        {
            if (activity.Points.Count < 2)
            {
                foreach (var point in activity.Points)
                {
                    point.Pace = 0;
                }
                return 0;
            }

            activity.Points.First().Pace = 0;

            var pointPairs = PointPairs(activity);
            foreach (var pointPair in pointPairs)
            {
                pointPair.SecondPoint.Pace = (decimal?)(PaceBetween(pointPair.FirstPoint, pointPair.SecondPoint));
            }

            var distance = Distance(activity);
            if (distance == 0)
                return 0;

            return ActiveDuration(activity)/distance;
        }

        public static decimal? AverageAscendingPace(Activity activity)
        {
            var distance = AscendingDistance(activity);
            if (distance == 0)
                return 0;

            return AscendingDuration(activity) / distance;
        }

        public static decimal? AverageDescendingPace(Activity activity)
        {
            var distance = DescendingDistance(activity);
            if (distance == 0)
                return 0;

            return DescendingDuration(activity) / distance;
        }

        public static decimal? AverageFlatPace(Activity activity)
        {
            var distance = FlatDistance(activity);
            if (distance == 0)
                return 0;

            return FlatDuration(activity) / distance;
        }

        public static decimal? AverageSpeed(Activity activity)
        {
            if (activity.Points.Count < 2)
            {
                foreach (var point in activity.Points)
                {
                    point.Speed = 0;
                }
                return 0;
            }

            activity.Points.First().Speed = 0;

            var pointPairs = PointPairs(activity);
            foreach (var pointPair in pointPairs)
            {
                pointPair.SecondPoint.Speed = (decimal?)(SpeedBetween(pointPair.FirstPoint, pointPair.SecondPoint));
            }

            var duration = ActiveDuration(activity);
            if (duration == 0)
                return 0;

            return Distance(activity) / duration;
        }

        public static decimal? AverageAscendingSpeed(Activity activity)
        {
            var duration = AscendingDuration(activity);
            if (duration == 0)
                return 0;

            return AscendingDistance(activity) / duration;
        }

        public static decimal? AverageDescendingSpeed(Activity activity)
        {
            var duration = DescendingDuration(activity);
            if (duration == 0)
                return 0;

            return DescendingDistance(activity) / duration;
        }

        public static decimal? AverageFlatSpeed(Activity activity)
        {
            var duration = FlatDuration(activity);
            if (duration == 0)
                return 0;

            return FlatDistance(activity) / duration;
        }

        public static decimal? MaximumSpeed(Activity activity)
        {
            if (activity.Points.Count < 2)
            {
                return 0;
            }
            var pointPairs = PointPairs(activity);

            var pairs = pointPairs as IList<PointPair> ?? pointPairs.ToList();
            var total = pairs.Select(pointPair => SpeedBetween(pointPair.FirstPoint, pointPair.SecondPoint)).Max();

            
            return (decimal?) total;
        }

        public static decimal? AverageHeartRate(Activity activity)
        {
            var points = activity.Points.Where(p => p.HeartRate != null).ToList();
            return (decimal?) (points.Any() ? points.Average(p => p.HeartRate) : null);
        }

        public static int? MaximumHeartRate(Activity activity)
        {
            var points = activity.Points.Where(p => p.HeartRate != null).ToList();
            return points.Any() ? points.Max(p => p.HeartRate) : null;
        }

        public static int? MinimumHeartRate(Activity activity)
        {
            var points = activity.Points.Where(p => p.HeartRate != null ).ToList();
            return points.Any() ? points.Min(p => p.HeartRate) : null;
        }

        public static int? MinimumCadence(Activity activity)
        {
            var points = activity.Points.Where(p => p.Cadence != null && p.Cadence > 0).ToList();
            return points.Any() ? points.Min(p => p.Cadence) : null;
        }

        public static int? MaximumCadence(Activity activity)
        {
            var points = activity.Points.Where(p => p.Cadence != null && p.Cadence > 0).ToList();
            return points.Any() ? points.Max(p => p.Cadence) : null;
        }

        public static decimal? AverageCadence(Activity activity)
        {
            var points = activity.Points.Where(p => p.Cadence != null && p.Cadence > 0).ToList();
            return (decimal?) (points.Any() ? points.Average(p => p.Cadence) : null);
        }

        #region Private

        public static void Each<T>(this IEnumerable<T> ie, Action<T, int> action)
        {
            var i = 0;
            foreach (var e in ie) action(e, i++);
        }

        private static IEnumerable<PointPair> PointPairs(Activity activity)
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

        private static IEnumerable<PointPair> AscendingPointPairs(Activity activity)
        {
            var pointPairs = PointPairs(activity);
            return (from pointPair in pointPairs
                    where IsAscendingBetween(pointPair.FirstPoint, pointPair.SecondPoint)
                    select new PointPair
                               {
                                   FirstPoint = pointPair.FirstPoint,
                                   SecondPoint = pointPair.SecondPoint
                               }).ToList();
        }

        private static IEnumerable<PointPair> DescendingPointPairs(Activity activity)
        {
            var pointPairs = PointPairs(activity);
            return (from pointPair in pointPairs
                    where IsDescendingBetween(pointPair.FirstPoint, pointPair.SecondPoint)
                    select new PointPair
                               {
                                   FirstPoint = pointPair.FirstPoint,
                                   SecondPoint = pointPair.SecondPoint
                               }).ToList();
        }

        private static IEnumerable<PointPair> FlatPointPairs(Activity activity)
        {

            var pointPairs = PointPairs(activity);
            return (from pointPair in pointPairs
                    where
                        IsActiveBetween(pointPair.FirstPoint, pointPair.SecondPoint) &&
                        IsFlatBetween(pointPair.FirstPoint, pointPair.SecondPoint)
                    select new PointPair
                               {
                                   FirstPoint = pointPair.FirstPoint,
                                   SecondPoint = pointPair.SecondPoint
                               }).ToList();
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

            var a = Math.Sin(deltaLatitude / 2) * Math.Sin(deltaLatitude / 2) +
                    Math.Sin(deltaLongitude / 2) * Math.Sin(deltaLongitude / 2) * Math.Cos(fromLatitude) * Math.Cos(toLatitude);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EarthRadius(fromPoint) * c;
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
            var e = ElevationBetween(startPoint, endPoint);
            var t = (decimal) DistanceBetween(startPoint, endPoint);

            return t != 0 && (e/t) > (decimal) 0.003;
        }

        private static bool IsDescendingBetween(Point startPoint, Point endPoint)
        {
            var e = ElevationBetween(endPoint, startPoint);
            var t = (decimal)DistanceBetween(startPoint, endPoint);

            return t != 0 && (e / t) > (decimal)0.003;
        }

        private static bool IsFlatBetween(Point startPoint, Point endPoint)
        {
            var e = ElevationBetween(startPoint, endPoint);
            var t = (decimal)DistanceBetween(startPoint, endPoint);

            return t != 0 && ((Math.Abs((e / t))) <= (decimal)0.003);

        }

        #endregion
    }
}