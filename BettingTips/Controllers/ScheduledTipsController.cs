using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BettingTips.Models;
using PagedList;

namespace BettingTips.Controllers
{
    [Authorize]
    public class ScheduledTipsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ScheduledTips
        public ActionResult Index(int? page)
        {
            var scheduledTips = from s in db.ScheduledTips
                                select s;
            scheduledTips = scheduledTips.OrderByDescending(s => s.Id);
            int pageNumber = (page ?? 1);
            int pageSize = 25;
            return View(scheduledTips.ToPagedList(pageNumber, pageSize));
        }

        // GET: ScheduledTips/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScheduledTip scheduledTip = db.ScheduledTips.Find(id);
            if (scheduledTip == null)
            {
                return HttpNotFound();
            }
            return View(scheduledTip);
        }

        // GET: ScheduledTips/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScheduledTips/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TipNumber,Tip,DateScheduled,Destination,ServiceId")] ScheduledTip scheduledTip)
        {
            if (ModelState.IsValid)
            {
                db.ScheduledTips.Add(scheduledTip);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scheduledTip);
        }

        // GET: ScheduledTips/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScheduledTip scheduledTip = db.ScheduledTips.Find(id);
            if (scheduledTip == null)
            {
                return HttpNotFound();
            }
            return View(scheduledTip);
        }

        // POST: ScheduledTips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TipNumber,Tip,DateScheduled,Destination,ServiceId")] ScheduledTip scheduledTip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scheduledTip).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scheduledTip);
        }

        // GET: ScheduledTips/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScheduledTip scheduledTip = db.ScheduledTips.Find(id);
            if (scheduledTip == null)
            {
                return HttpNotFound();
            }
            return View(scheduledTip);
        }

        // POST: ScheduledTips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScheduledTip scheduledTip = db.ScheduledTips.Find(id);
            db.ScheduledTips.Remove(scheduledTip);
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
