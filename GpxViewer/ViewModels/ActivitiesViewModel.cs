using System.Collections.Generic;
using GpxViewer.Models;

namespace GpxViewer.ViewModels
{
    public class ActivitiesViewModel
    {
        public Track Track { get; set; }
        public List<TrackSegment> TrackSegments { get; set; }
    }
}