using SUBSURF_MKN_WellOptimization_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SUBSURF_MKN_WellOptimization_MVC.Controllers
{
    public class ReportsController : Controller
    {
        public ReportsController() {

            ViewBag.role = DataAccess.Role;


        }



        private WelloptContext db = new WelloptContext();
       
        [HttpGet]
        // GET: Reports
        public ActionResult DBActivity()
        {
            ViewBag.isPost = false;
            return View(); 
        
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DBActivity([Bind(Include = "startDate,endDate")] DBActivity DBA )
        {
            ViewBag.isPost = false;
            if (ModelState.IsValid)
            {
                ViewBag.isPost = true;

                DateTime startDate = DBA.startDate;
                DateTime endDate= DBA.endDate;
                
                //if the user did not follow the labels on the form fix it
                if (startDate > endDate)
                {
                    DateTime temp;
                    temp = startDate;
                    startDate = endDate;
                    endDate = temp;
                }

                ViewBag.DatesRange = Math.Abs((endDate - startDate).Days);

               

                var Filtered = db.Wellopts.Where(obj => obj.RecordDate >= startDate && obj.RecordDate <= endDate).OrderBy(obj => obj.RecordDate).ToList();

                List<string> DatesStr = new List<string>();
                List<int> count = new List<int>();

                foreach (Wellopt well in Filtered)
                {
                    DatesStr.Add(well.RecordDate.ToString("dd /MM /yyyy"));
                }

                List<string> unique = DatesStr.Distinct().ToList();
                ViewBag.labels = string.Join(",", unique);


                foreach (string date in unique)
                {
                    int singlecount = DatesStr.Where(x => x.Equals(date)).Count();
                    count.Add(singlecount);

                }


                ViewBag.nums = string.Join(",", count);


                return View();
            }


            return View();
        }

        public ActionResult WeeklyOptReportPerALS()
        {
            return View();
        }


        public ActionResult WellOptByArea()
        {
            return View();
        }
    }
}