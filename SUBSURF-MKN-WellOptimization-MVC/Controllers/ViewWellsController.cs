using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUBSURF_MKN_WellOptimization_MVC.Controllers
{
    public class ViewWellsController : Controller
    {
        // GET: ViewWells
        public ActionResult Index()
        {
            return View("~/Views/ViewWells/ViewWells.cshtml");
        }
    }
}