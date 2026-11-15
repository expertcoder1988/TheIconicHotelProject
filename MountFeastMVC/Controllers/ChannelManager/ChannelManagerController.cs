using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MountFeastMVC.Controllers.ChannelManager
{
    public class ChannelManagerController : Controller
    {
        // GET: ChannelManager
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddChannel()
        {
            return View();
        }
        public ActionResult ChannelManagerList()
        {
            return View();
        }
    }
}