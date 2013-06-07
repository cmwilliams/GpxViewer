using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Gpx;
using GpxViewer.Models;
using GpxViewer.ViewModels;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;

namespace GpxViewer.Controllers
{
    public class TracksController : Controller
    {
        private readonly GpxViewerContext db = new GpxViewerContext();

        public ActionResult Index()
        {
            return View(db.Tracks.ToList());
        }

        public ActionResult Details(int id = 0)
        {
            Track track = db.Tracks.Find(id);
            if (track == null)
            {
                return HttpNotFound();
            }
            return View(track);
        }

        public ActionResult Create()
        {
            return View();
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

                                foreach (var segment in reader.Track.Segments)
                                {
                                    var trackSegment = new TrackSegment { Points = new Collection<Point>() };

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

                        track = ParseGpx(track, viewModel.GpxFile.InputStream);
                        var path = Server.MapPath("~/Uploads");
                        viewModel.GpxFile.SaveAs(Path.Combine(path, viewModel.GpxFile.FileName));

                        db.Tracks.Add(track);
                        db.SaveChanges();
                        return RedirectToAction("Index");
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
            Track track = db.Tracks.Find(id);
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
            Track track = db.Tracks.Find(id);
            db.Tracks.Remove(track);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
