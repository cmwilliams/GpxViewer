using System;
using System.Linq;
using System.Web.Mvc;
using GpxViewer.Helpers;
using GpxViewer.Models;
using GpxViewer.ViewModels;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;

namespace GpxViewer.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly GpxViewerContext _db = new GpxViewerContext();

        public ActionResult Index()
        {
            return View(_db.Activities.ToList());
        }

        public ActionResult Details(int id = 0)
        {
            var activity = _db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }

            activity.MinimumCadence = activity.MinimumCadence ?? 0;
            activity.MaximumCadence = activity.MaximumCadence ?? 0;
            activity.AverageCadence = activity.AverageCadence ?? 0;
            activity.MinimumHeartRate = activity.MinimumHeartRate ?? 0;
            activity.MaximumHeartRate = activity.MaximumHeartRate ?? 0;
            activity.AverageHeartRate = activity.AverageHeartRate ?? 0;


            var polyline = activity.Points == null ? string.Empty : activity.Points.GetPolyline();
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            var gpxData = (from p in activity.Points
                           select new
                                      {
                                          Duration = p.Duration.FormatTime(),
                                          Data = new
                                                     {
                                                         ele = p.Elevation.FormatElevation(),
                                                         speed = p.Speed.FormatSpeed(),
                                                         distance = p.Distance.FormatDistance(),
                                                         cad = p.Cadence,
                                                         hr = p.HeartRate,
                                                         time = p.Duration.FormatTime(),
                                                         lat = p.Latitude,
                                                         lon = p.Longitude
                                                     }
                                      }).ToList();

            var jsonData = oSerializer.Serialize(gpxData);

            
            var viewModel = new ActivityDetailViewModel
                {
                    Activity = activity,
                    Polyline = polyline,
                    GpxJsonData = jsonData
                };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }


      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateActivityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (viewModel.GpxFile != null && viewModel.GpxFile.ContentLength > 0)
                    {
                        var activities = Parser.ParseGpx(viewModel.GpxFile.InputStream);

                        foreach (var activity in activities)
                        {
                            var a = Statistics.CalculateStatistics(activity);
                            _db.Activities.Add(activity);
                        }

                     
                        _db.SaveChanges();
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
            var activity = _db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var activity = _db.Activities.Find(id);
            _db.Activities.Remove(activity);
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
