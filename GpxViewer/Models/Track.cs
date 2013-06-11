using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GpxViewer.Models
{
    public class Track
    {
        public int TrackId { get; set; }
       
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "File Name")]
        public string FileName { get; set; }
         [Display(Name = "Content Type")]
        public string FileContentType { get; set; }
         [Display(Name = "File Size")]
        public int FileSize { get; set; }
         [Display(Name = "File Updated At")]
        public DateTime FileUpdatedAt { get; set; }
        public virtual ICollection<TrackSegment> TrackSegments { get; set; }

    }
}
