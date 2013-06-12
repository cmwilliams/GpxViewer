using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Gpx;
using GpxViewer.Helpers;
using GpxViewer.Models;
using GpxViewer.ViewModels;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;

namespace GpxViewer.Controllers
{
    public class TracksController : Controller
    {
        private readonly GpxViewerContext _db = new GpxViewerContext();

        public ActionResult Index()
        {
            return View(_db.Tracks.ToList());
        }

        public ActionResult Details(int id = 0)
        {
            var track = _db.Tracks.Find(id);
            if (track == null)
            {
                return HttpNotFound();
            }

            var trackSegment = _db.TrackSegments.SingleOrDefault(s => s.TrackId == track.TrackId);
            var polyline = trackSegment == null ? string.Empty : trackSegment.Points.GetPolyline();
            var distance = trackSegment == null ? 0 : trackSegment.Points.GetDistance();
            var elevationProfile = trackSegment.Points.GetElevation();

            var viewModel = new TrackDetailViewModel
                {
                    Track = track,
                    Polyline = polyline,
                    Distance = distance,
                    ElevationProfile = elevationProfile
                };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        private static XDocument LoadFromStream(Stream stream)
        {
            using (var reader = XmlReader.Create(stream))
            {
                return XDocument.Load(reader);
            }
        }

        private XNamespace GetGpxNameSpace()
        {
            var gpx = XNamespace.Get("http://www.topografix.com/GPX/1/1");
            return gpx;
        }

        private double Double(string value)
        {
            return double.Parse(value, CultureInfo.InvariantCulture);
        }

        private static string DefaultStringValue(XContainer element, XNamespace ns ,string elementName)
        {
            var xElement = element.Element(ns + elementName);
            return xElement != null ? xElement.Value : null;
        }

        private static double? DefaultDoubleValue(XContainer element, XNamespace ns, string elementName)
        {
            var xElement = element.Element(ns + elementName);
            return xElement != null ? Convert.ToDouble(xElement.Value) : (double?)null;
        }

        private static DateTime? DefaultDateTimeValue(XContainer element, XNamespace ns, string elementName)
        {
            var xElement = element.Element(ns + elementName);
            return xElement != null ? Convert.ToDateTime(xElement.Value) : (DateTime?)null;
        }

        public Track LoadGpxTracks(Track exitingTrack, Stream document)
        {
            var gpxDoc = LoadFromStream(document);
            var gpx = GetGpxNameSpace();

            var tracks = from track in gpxDoc.Descendants(gpx + "trk")
                         select new
                                    {
                                        Name = DefaultStringValue(track, gpx, "name"),
                                        Description = DefaultStringValue(track, gpx, "desc"),
                                        Segments = (from trkSegment in track.Descendants(gpx + "trkseg")
                                                    select new
                                                               {
                                                                   TrackSegment = trkSegment,
                                                                   Points = (from trackpoint in trkSegment.Descendants(gpx + "trkpt")
                                                                             select new
                                                                             {
                                                                                 Lat = Double(trackpoint.Attribute("lat").Value),
                                                                                 Lng = Double(trackpoint.Attribute("lon").Value),
                                                                                 Ele = DefaultDoubleValue(trackpoint, gpx, "ele"),
                                                                                 Time = DefaultDateTimeValue(trackpoint, gpx, "time")
                                                                             })
                                                               })
                                    };

            foreach (var trk in tracks)
            {
                exitingTrack.Name = trk.Name;
                exitingTrack.Description = trk.Description;

                foreach (var segment in trk.Segments)
                {
                    var trackSegment = new TrackSegment { Points = new Collection<Point>() };

                    foreach (var point in segment.Points)
                    {
                        trackSegment.Points.Add(new Point
                        {
                            Elevation = point.Ele,
                            Latitude = point.Lat,
                            Longitude = point.Lng,
                            PointCreatedAt = point.Time
                        });
                    }

                    exitingTrack.TrackSegments.Add(trackSegment);
                }
            }
            return exitingTrack;
        }



        private static Track ParseGpx(Track track, Stream document)
        {
            try
            {
                using (var reader = new GpxReader(document))
                {
                    while (reader.Read())
                    {
                        switch (reader.ObjectType)
                        {
                           

                            case GpxObjectType.Track:

                                track.Name = reader.Track.Name;
                                track.Description = reader.Track.Description;

                                foreach (var segment in reader.Track.Segments)
                                {
                                    var trackSegment = new TrackSegment {Points = new Collection<Point>()};

                                    foreach (var point in segment.TrackPoints)
                                    {
                                        trackSegment.Points.Add(new Point
                                                                    {
                                                                        Elevation = point.Elevation,
                                                                        Latitude = point.Latitude,
                                                                        Longitude = point.Longitude,
                                                                        PointCreatedAt = point.Time
                                                                    });
                                    }

                                    track.TrackSegments.Add(trackSegment);
                                }
                                break;
                        }
                    }
                }

                return track;
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format("Unable to parse file. Error: {0}", exception.Message));
            }
           
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTrackViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var track = new Track { TrackSegments = new Collection<TrackSegment>() };

                    if (viewModel.GpxFile != null && viewModel.GpxFile.ContentLength > 0)
                    {
                        track.FileContentType = viewModel.GpxFile.ContentType;
                        track.FileName = viewModel.GpxFile.FileName;
                        track.FileSize = viewModel.GpxFile.ContentLength;
                        track.FileUpdatedAt = DateTime.Now;

                        track = LoadGpxTracks(track, viewModel.GpxFile.InputStream);

                        _db.Tracks.Add(track);
                        _db.SaveChanges();
                        return RedirectToAction("Details", new {@id = track.TrackId});
                    }             
                }
                catch (Exception exception)
                {
                    Flash.Instance.Error("", string.Format("The following error occurred: {0}", exception.Message));
                    return View(viewModel);
                }
            
            }

            return View(viewModel);
        }

      
        public ActionResult Delete(int id = 0)
        {
            Track track = _db.Tracks.Find(id);
            if (track == null)
            {
                return HttpNotFound();
            }
            return View(track);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Track track = _db.Tracks.Find(id);
            _db.Tracks.Remove(track);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
