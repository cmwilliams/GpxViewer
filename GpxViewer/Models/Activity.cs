using System;
using System.Collections.Generic;

namespace GpxViewer.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ActivityDate { get; set; }
        
        public decimal? Distance { get; set; }
        public decimal? AscendingDistance { get; set; }
        public decimal? DescendingDistance { get; set; }
        public decimal? FlatDistance { get; set; }

        public decimal? ElevationGain { get; set; }
        public decimal? ElevationLoss { get; set; }
        public decimal? ElevationChange { get; set; }
        public decimal? MaximumElevation { get; set; }
        public decimal? MinimumElevation { get; set; }

        public decimal? Duration { get; set; }
        public decimal? ActiveDuration { get; set; }
        public decimal? AscendingDuration { get; set; }
        public decimal? DescendingDuration { get; set; }
        public decimal? FlatDuration { get; set; }

        public decimal? AveragePace { get; set; }
        public decimal? AverageAscendingPace { get; set; }
        public decimal? AverageDescendingPace { get; set; }
        public decimal? AverageFlatPace { get; set; }

        public decimal? AverageSpeed { get; set; }
        public decimal? AverageAscendingSpeed { get; set; }
        public decimal? AverageDescendingSpeed { get; set; }
        public decimal? AverageFlatSpeed { get; set; }
        public decimal? MaximumSpeed { get; set; }

        public int? MaximumHeartRate { get; set; }
        public decimal? AverageHeartRate { get; set; }

        public decimal? AverageCadence { get; set; }

        public virtual ICollection<Point> Points { get; set; }


    }
}