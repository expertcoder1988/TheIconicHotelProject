using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MountFeastMVC.Controllers
{
    public class GalleryController : Controller
    {
        [Route("~/naran-hotel-photo-gallery", Name = "galleryhotelnaran")]
        // GET: Gallery
        public ActionResult Index()
        {
            return View();
        }
    }
}