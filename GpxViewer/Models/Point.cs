using System;

namespace GpxViewer.Models
{
    public class Point
    {
        public int PointId { get; set; }
        public int TrackSegmentId { get; set; }
        public TrackSegment TrackSegment { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Elevation { get; set; }
        public int? Cadence { get; set; }
        public int? HeartRate { get; set; }
        public DateTime? PointCreatedAt { get; set; }
    }
}
