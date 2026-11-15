using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MountFeastMVC.Controllers
{
    public class TermsandConditionsController : Controller
    {
        [Route("~/terms-and-conditions", Name = "TermsandConditions")]
        // GET: TermsandConditions
        public ActionResult Index()
        {
            return View();
        }
    }
}