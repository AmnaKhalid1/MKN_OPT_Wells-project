using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SUBSURF_MKN_WellOptimization_MVC.Models;

namespace SUBSURF_MKN_WellOptimization_MVC.Controllers
{
    public class ActionLogsController : Controller
    {
        private WelloptContext db = new WelloptContext();

        public ActionLogsController()
        {
            ViewBag.role = DataAccess.Role;

        }

        // GET: ActionLogs
        public ActionResult Index()
        {
            if (DataAccess.Role != "Contributor")
            {
                return RedirectToRoute(new
                {
                    controller = "AccessDenied",
                    action = "AccessDenied"

                });

            }
            return View(db.ActionLogs.ToList());
        }

        

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionLog actionLog = db.ActionLogs.Find(id);
            if (actionLog == null)
            {
                return HttpNotFound();
            }
            if (actionLog.actionType != ActionType.create ) { 
                ViewBag.Previous = actionLog.PreviousRecord;
               
            }
            else
            {
                ViewBag.Created = actionLog.PreviousRecord;
            }
            if ((ViewBag.Created!= null || ViewBag.Previous != null )&& actionLog.PreviousRecord != null) {

                string[] splited = actionLog.PreviousRecord.Split('|');
                ViewBag.WellName = splited[0];
                ViewBag.Pump = splited[1];
                ViewBag.comment = splited[2];
                ViewBag.optType = splited[3];
            }
            return View(actionLog);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
