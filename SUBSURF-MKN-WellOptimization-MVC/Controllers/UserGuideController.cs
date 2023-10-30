using SUBSURF_MKN_WellOptimization_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUBSURF_MKN_WellOptimization_MVC.Controllers
{
    public class UserGuideController : Controller
    {

        public UserGuideController() {


            ViewBag.role = DataAccess.Role;

        }

        // GET: UserGuide
        public ActionResult Index()
        {
            return View();
        }
    }
}