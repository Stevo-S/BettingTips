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
    public class SubscribersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Subscribers
        public ActionResult Index(string phoneNumber, DateTime? startDate, DateTime? endDate, string subscriptionStatus, int? page)
        {
            var subscribers = from s in db.Subscribers
                              select s;

            // Filter by phoneNumber
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                subscribers = subscribers.Where(s => s.PhoneNumber.Contains(phoneNumber));
                ViewBag.phoneFilter = phoneNumber;
            }

            // Filter by subscription status
            if (!string.IsNullOrEmpty(subscriptionStatus))
            {
                if (subscriptionStatus == "active")
                {
                    subscribers = subscribers.Where(s => s.isActive == true);
                }
                else if (subscriptionStatus == "inactive")
                {
                    subscribers = subscribers.Where(p => !p.isActive == true);
                }

                ViewBag.subscriptionStatusFilter = subscriptionStatus;
            }

            if (startDate != null)
            {
                if (endDate < startDate || endDate == null)
                {
                    endDate = DateTime.Now;
                }
                subscribers = subscribers.Where(s => s.FirstSubscriptionDate > startDate && s.FirstSubscriptionDate < endDate);
                ViewBag.startDateFilter = startDate;
                ViewBag.endDateFilter = endDate;
            }

            subscribers = subscribers.OrderByDescending(s => s.FirstSubscriptionDate);
            ViewBag.Total = subscribers.Count();
            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return View(subscribers.ToPagedList(pageNumber, pageSize));
        }

        // GET: Subscribers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscriber subscriber = db.Subscribers.Find(id);
            if (subscriber == null)
            {
                return HttpNotFound();
            }
            return View(subscriber);
        }

        // GET: Subscribers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subscribers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PhoneNumber,ServiceId,FirstSubscriptionDate,LastSubscriptionDate,NextTip,isActive")] Subscriber subscriber)
        {
            if (ModelState.IsValid)
            {
                db.Subscribers.Add(subscriber);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subscriber);
        }

        // GET: Subscribers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscriber subscriber = db.Subscribers.Find(id);
            if (subscriber == null)
            {
                return HttpNotFound();
            }
            return View(subscriber);
        }

        // POST: Subscribers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PhoneNumber,ServiceId,FirstSubscriptionDate,LastSubscriptionDate,NextTip,isActive")] Subscriber subscriber)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subscriber).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subscriber);
        }

        // GET: Subscribers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscriber subscriber = db.Subscribers.Find(id);
            if (subscriber == null)
            {
                return HttpNotFound();
            }
            return View(subscriber);
        }

        // POST: Subscribers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subscriber subscriber = db.Subscribers.Find(id);
            db.Subscribers.Remove(subscriber);
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
