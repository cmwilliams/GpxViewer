using System.Data.Entity;

namespace GpxViewer.Models
{
    public class GpxViewerContext : DbContext
    {
        public DbSet<Track> Tracks { get; set; }
        public DbSet<TrackSegment> TrackSegments { get; set; }
        public DbSet<Point> Points { get; set; }
    }
}