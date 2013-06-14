using System.ComponentModel.DataAnnotations;
using System.Web;
using GpxViewer.Models;

namespace GpxViewer.ViewModels
{
    public class CreateActivityViewModel
    {
        [Required(ErrorMessage = "Gpx File is required")]
        [Display(Name = "File Name")]
        public HttpPostedFileBase GpxFile { get; set; }
    }
}