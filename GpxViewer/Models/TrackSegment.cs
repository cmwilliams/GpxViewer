using System.Collections.Generic;

namespace GpxViewer.Models
{
    public class TrackSegment
    {
        public int TrackSegmentId { get; set; }
        public int TrackId { get; set; }
        public Track Track { get; set; }
        public virtual ICollection<Point> Points { get; set; }
    }
}
