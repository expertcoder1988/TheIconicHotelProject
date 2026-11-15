using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MountFeastMVC.Controllers.Hall
{
    public class HallController : Controller
    {
        // GET: Hall
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddMainHall()
        {
            return View();
        }
        public ActionResult UpdateHallImages()
        {
            return View();
        }
    }
}