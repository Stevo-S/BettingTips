using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BettingTips.Models;
using Hangfire;
using BettingTips.Tasks;

namespace BettingTips.Controllers
{
    public class MatchSpecificTipsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MatchSpecificTips
        public ActionResult Index()
        {
            return View(db.MatchSpecificTips.ToList());
        }

        // GET: MatchSpecificTips/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatchSpecificTip matchSpecificTip = db.MatchSpecificTips.Find(id);
            if (matchSpecificTip == null)
            {
                return HttpNotFound();
            }
            return View(matchSpecificTip);
        }

        // GET: MatchSpecificTips/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MatchSpecificTips/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Tip,SendTime,Expiration")] MatchSpecificTip matchSpecificTip)
        {
            if (ModelState.IsValid)
            {
                db.MatchSpecificTips.Add(matchSpecificTip);
                db.SaveChanges();
                BackgroundJob.Schedule(() => Jobs.ScheduleMatchSpecificTip(matchSpecificTip.Id), matchSpecificTip.SendTime);
                return RedirectToAction("Index");
            }

            return View(matchSpecificTip);
        }

        // GET: MatchSpecificTips/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatchSpecificTip matchSpecificTip = db.MatchSpecificTips.Find(id);
            if (matchSpecificTip == null)
            {
                return HttpNotFound();
            }
            return View(matchSpecificTip);
        }

        // POST: MatchSpecificTips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Tip,SendTime,Expiration")] MatchSpecificTip matchSpecificTip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(matchSpecificTip).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(matchSpecificTip);
        }

        // GET: MatchSpecificTips/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatchSpecificTip matchSpecificTip = db.MatchSpecificTips.Find(id);
            if (matchSpecificTip == null)
            {
                return HttpNotFound();
            }
            return View(matchSpecificTip);
        }

        // POST: MatchSpecificTips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MatchSpecificTip matchSpecificTip = db.MatchSpecificTips.Find(id);
            db.MatchSpecificTips.Remove(matchSpecificTip);
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
