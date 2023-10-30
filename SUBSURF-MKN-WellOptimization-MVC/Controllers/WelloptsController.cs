using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using SUBSURF_MKN_WellOptimization_MVC.Models;

namespace SUBSURF_MKN_WellOptimization_MVC.Controllers
{

    public class WelloptsController : Controller
    {
        public string Row = null;
         
        private WelloptContext db = new WelloptContext();
        public WelloptsController() {
           
            ViewBag.role = DataAccess.Role;
        }


        // GET: Wellopts
        public ActionResult Index()
        {
            return View(db.Wellopts.ToList());
        }

        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }


            Wellopt wellopt = db.Wellopts.Find(id);
            if (wellopt == null)
            {
                return HttpNotFound();
            }
            else
            {
                Row = DataAccess.GetROW(wellopt.WellName);
                ViewBag.Row = Row;
                ViewBag.Well = wellopt;
                return View(wellopt);

            }
        }
           

        // GET: Wellopts/Create
        public ActionResult Create()
        {
            if (DataAccess.Role != "Contributor")
            {
                return RedirectToRoute(new
                {
                    controller = "AccessDenied",
                    action = "AccessDenied"

                });

            }
            return View();
        }

        // POST: Wellopts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RecordDate,UWI,WellName,PumpType,OPTIMIZATIONTYPES,Comments")] Wellopt wellopt)
        {
            string UWI = DataAccess.GetUWI(wellopt.WellName);
            if(wellopt.UWI == null) { wellopt.UWI = UWI; }
            ViewBag.UWIErr = "";
            ViewBag.WellNameErr = "";
            if (ModelState.IsValid && DataAccess.checkWellName(wellopt.WellName) && UWI == wellopt.UWI)
            {

                wellopt.UWI = DataAccess.GetUWI(wellopt.WellName);
                db.Wellopts.Add(wellopt);
                db.SaveChanges();

                //add the edit action to the database
                ActionLog actionLog = new ActionLog()
                {
                    LogDate = DateTime.Now,
                    RecordId = wellopt.Id,
                    Username = DataAccess.Username,
                    actionType = ActionType.create,
                    UWI = wellopt.UWI,
                    PreviousRecord = wellopt.WellToString()
                };
                string error = "";
                try
                {
                    db.ActionLogs.Add(actionLog);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        error += $"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:";

                        foreach (var ve in eve.ValidationErrors)
                        {
                            error += $"- Property: \"{ ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"";


                        }
                    }
                    ViewBag.error = error;

                }

                return RedirectToAction("Index");
            }
            if (!DataAccess.checkWellName(wellopt.WellName)) {

                string simi = DBActivity.GetSimilarWell(wellopt.WellName);
                if (simi != "") { ViewBag.WellNameErr = "Unkown well name!"+" A similar may be the "+simi; }
                else { ViewBag.WellNameErr = "Unkown well name!"; }

            }
            if(UWI != wellopt.UWI) { ViewBag.UWIErr = "Unkown UWI or UWI does not match the well name!"; }
            return View(wellopt);
        }


        // GET: Wellopts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (DataAccess.Role != "Contributor")
            {
                return RedirectToRoute(new
                {
                    controller = "AccessDenied",
                    action = "AccessDenied"

                });

            }

             Records.welloptBefore = db.Wellopts.Find(id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wellopt wellopt = db.Wellopts.Find(id);
            if (wellopt == null)
            {
                return HttpNotFound();
            }

            
            return View(wellopt);
        }

        // POST: Wellopts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RecordDate,UWI,WellName,PumpType,OPTIMIZATIONTYPES,Comments")] Wellopt wellopt)
        {
            string UWI = DataAccess.GetUWI(wellopt.WellName);
            if (wellopt.UWI == null) { wellopt.UWI = UWI; }
            ViewBag.UWIErr = "";
            ViewBag.WellNameErr = "";
            if (ModelState.IsValid && DataAccess.checkWellName(wellopt.WellName) && UWI == wellopt.UWI)
            {
                
                //add the edit action to the database
               


                wellopt.UWI = DataAccess.GetUWI(wellopt.WellName);
                db.Entry(wellopt).State = EntityState.Modified;
                db.SaveChanges();


                if (Records.welloptBefore != null)
                {
                    ActionLog actionLog = new ActionLog()
                    {
                        LogDate = DateTime.Now,
                        RecordId = Records.welloptBefore.Id,
                        Username = DataAccess.Username,
                        actionType = ActionType.Edit,
                        UWI = Records.welloptBefore.UWI,
                        PreviousRecord = Records.welloptBefore.WellToString()
                    };
                    db.ActionLogs.Add(actionLog);
                    db.SaveChanges();

                }


                return RedirectToAction("Index");
            }
            if (!DataAccess.checkWellName(wellopt.WellName)) 
            {
                string simi = DBActivity.GetSimilarWell(wellopt.WellName);
                ViewBag.WellNameErr = "Unkown well name!"+" a similar well name may be "+simi; 
            }
            if (UWI != wellopt.UWI) { ViewBag.UWIErr = "Unkown UWI or UWI does not match the well name!"; }
            return View(wellopt);
        }

        // GET: Wellopts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (DataAccess.Role != "Contributor")
            {
                return RedirectToRoute(new
                {
                    controller = "AccessDenied",
                    action = "AccessDenied"

                });

            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wellopt wellopt = db.Wellopts.Find(id);
            if (wellopt == null)
            {
                return HttpNotFound();
            }
            return View(wellopt);
        }

        // POST: Wellopts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Wellopt wellopt = db.Wellopts.Find(id);
            db.Wellopts.Remove(wellopt);
            db.SaveChanges();

            //add the action to the ActionLog database
            ActionLog actionLog = new ActionLog() {
                LogDate = DateTime.Now,
                RecordId = id,
                Username = DataAccess.Username,
                actionType = ActionType.Delete,
                UWI = wellopt.UWI,
                PreviousRecord = wellopt.WellToString()
            };

            db.ActionLogs.Add(actionLog);
            db.SaveChanges();
            return RedirectToAction("Index");
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
