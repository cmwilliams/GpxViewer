using GpxViewer.Models;

namespace GpxViewer.ViewModels
{
    public class TrackDetailViewModel
    {
        public Track Track { get; set; }
        public string Polyline { get; set; }
        public double? Distance { get; set; }
        public ElevationProfile ElevationProfile { get; set; }
        public CadenceProfile CadenceProfile { get; set; }
        public HeartRateProfile HeartRateProfile { get; set; }
        public TimingProfile TimingProfile { get; set; }
    
    }
}