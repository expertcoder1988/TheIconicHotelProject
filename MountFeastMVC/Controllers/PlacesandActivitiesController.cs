using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MountFeastMVC.Controllers
{
    public class PlacesandActivitiesController : Controller
    {
        [Route("~/activities-tourist-places-in-naran-valley", Name = "placestovisitinnaran")]
        // GET: PopularActivities
        public ActionResult Index()
        {
            return View();
        }
    }
}