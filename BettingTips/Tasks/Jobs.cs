using BettingTips.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BettingTips.Tasks
{
    public class Jobs
    {
        public static void ScheduleTipMessages()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var subscribers = db.Subscribers;

            foreach(var subscriber in subscribers)
            {
                var tip = new ScheduledTip()
                {
                    Destination = subscriber.PhoneNumber,
                    TipNumber = subscriber.NextTip,
                    Tip = db.Tips.Where(t => t.TipNumber == subscriber.NextTip).First().Message,
                    ServiceId = subscriber.ServiceId,
                    DateScheduled = DateTime.Now
                };

                db.ScheduledTips.Add(tip);

                subscriber.NextTip += 1; 
                db.Entry(subscriber).State = System.Data.Entity.EntityState.Modified;
            }

            db.SaveChanges();
        }
    }
}