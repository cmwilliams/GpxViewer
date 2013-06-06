using System;

namespace GpxViewer.Models
{
    public class Point
    {
        public int PointId { get; set; }
        public int TrackSegmentId { get; set; }
        public TrackSegment TrackSegment { get; set; }
        public string Name { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float Elevation { get; set; }
        public string Description { get; set; }
        public DateTime PointCreatedAt { get; set; }
    }
}
