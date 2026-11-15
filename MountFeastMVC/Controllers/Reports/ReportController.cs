using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MountFeastMVC.Controllers.Reports
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ArrivalReport()
        {
            return View();
        }
        public ActionResult DepartureReport()
        {
            return View();
        }

    }
}