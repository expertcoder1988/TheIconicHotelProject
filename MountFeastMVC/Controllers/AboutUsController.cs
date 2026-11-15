using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MountFeastMVC.Controllers
{
    public class AboutUsController : Controller
    {
       [Route("~/about-hotel-booking-naran", Name = "abouthotelnaran")]
        // GET: AboutUs
        public ActionResult Index()
        {
            return View();
        }
    }
}