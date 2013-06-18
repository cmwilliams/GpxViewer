using System;

namespace GpxViewer.Helpers
{
    public static class FormatHelper
    {
        private static decimal MetersPerSecondToMilesPerHour(decimal metersPerSecond)
        {
            return metersPerSecond*(decimal) 2.23693629;
        }

        private static decimal MetersToMiles(decimal meters)
        {
            return FeetToMiles(MetersToFeet(meters));
        }

        private static decimal MetersToFeet(decimal meters)
        {
            return meters * (decimal) 3.2808399;
        }


        private static decimal FeetToMiles(decimal feet)
        {
            return feet / 5280;
        }

        private static decimal SecondsPerMeterToSecondsPerMile(decimal secondsPerMeter)
        {
            return secondsPerMeter * (decimal) 1609.344;
        }


        public static string FormatPace(this decimal? pace)
        {
            return pace == null ? string.Empty : FormatTime(SecondsPerMeterToSecondsPerMile((decimal) pace));
        }


        public static decimal FormatSpeed(this decimal? speed)
        {
            return speed == null ? 0 : Math.Round(MetersPerSecondToMilesPerHour(Convert.ToDecimal(speed)), 1);
        }

        public static decimal FormatDistance(this decimal? distance)
        {
            return distance == null ? 0 : Math.Round(MetersToMiles(Convert.ToDecimal(distance)), 1);
        }

        public static string FormatTime(this decimal? time)
        {
            if (time == null)
                return string.Empty;

            var ts = TimeSpan.FromSeconds((double) time);
            var formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                              ts.Hours,
                                              ts.Minutes,
                                              ts.Seconds);

            return formattedTime;
        }

        public static string FormatPaceTime(this decimal? time)
        {
            if (time == null)
                return string.Empty;

            var t = TimeSpan.FromSeconds((double)time);
            var formattedTime = string.Format("{0:D2}:{1:D2}",
                                              t.Minutes,
                                              t.Seconds);

            return formattedTime;
        }

        public static decimal FormatElevation(this decimal? elevation)
        {
            return elevation == null ? 0 : Math.Round(MetersToFeet(Convert.ToDecimal(elevation)), 0);
        }

       
    }
}