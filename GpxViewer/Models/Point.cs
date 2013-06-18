using System;
using GpxViewer.Helpers;

namespace GpxViewer.Models
{
    public class Point
    {
        public int PointId { get; set; }
        public DateTime? Time { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Elevation { get; set; }
        public decimal? Distance { get; set; }
        public decimal? Duration { get; set; }
        public decimal? ActiveDuration { get; set; }
        public decimal? Pace { get; set; }
        public decimal? Speed { get; set; }
        public int? HeartRate { get; set; }
        public int? Cadence { get; set; }

        public int ActivityId { get; set; }
        public Activity Activity { get; set; }

    }
}
