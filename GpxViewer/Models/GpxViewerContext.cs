using System.Data.Entity;

namespace GpxViewer.Models
{
    public class GpxViewerContext : DbContext
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Point> Points { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>().Property(x => x.Distance).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.AscendingDistance).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.DescendingDistance).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.FlatDistance).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.ElevationGain).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.ElevationLoss).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.ElevationChange).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.MaximumElevation).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.MinimumElevation).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.Duration).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.ActiveDuration).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.AscendingDuration).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.DescendingDuration).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.FlatDuration).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.AveragePace).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.AverageAscendingPace).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.AverageDescendingPace).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.AverageFlatPace).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.AverageSpeed).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.AverageAscendingSpeed).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.AverageDescendingSpeed).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.AverageFlatSpeed).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.MaximumSpeed).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.AverageHeartRate).HasPrecision(20, 10);
            modelBuilder.Entity<Activity>().Property(x => x.AverageCadence).HasPrecision(20, 10);

            modelBuilder.Entity<Point>().Property(x => x.Latitude).HasPrecision(20, 10);
            modelBuilder.Entity<Point>().Property(x => x.Longitude).HasPrecision(20, 10);
            modelBuilder.Entity<Point>().Property(x => x.Elevation).HasPrecision(20, 10);
            modelBuilder.Entity<Point>().Property(x => x.Distance).HasPrecision(20, 10);
            modelBuilder.Entity<Point>().Property(x => x.Duration).HasPrecision(20, 10);
            modelBuilder.Entity<Point>().Property(x => x.ActiveDuration).HasPrecision(20, 10);
            modelBuilder.Entity<Point>().Property(x => x.Pace).HasPrecision(20, 10);
            modelBuilder.Entity<Point>().Property(x => x.Speed).HasPrecision(20, 10);

            
        }

       
    }
}