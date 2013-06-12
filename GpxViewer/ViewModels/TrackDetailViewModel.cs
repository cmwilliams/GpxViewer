using GpxViewer.Models;

namespace GpxViewer.ViewModels
{
    public class TrackDetailViewModel
    {
        public Track Track { get; set; }
        public string Polyline { get; set; }
        public double? Distance { get; set; }
        public double? AvgCadence { get; set; }
        public double? MaxCadence { get; set; }
        public ElevationProfile ElevationProfile { get; set; }
    }
}