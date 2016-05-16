using BettingTips.Models;
using System;
using System.Linq;

namespace BettingTips.Tasks
{
    public class Jobs
    {
        public static void ScheduleTipMessages()
        {
            // Insert new Scheduled Messages containing tips of the day
            // Each subscriber gets a different tip depending on their progress
            // on the list of tips
            using (var db = new ApplicationDbContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;
                db.Configuration.ValidateOnSaveEnabled = false;

                var subscribers = db.Subscribers.Where(s => s.isActive).ToList();

                foreach (var subscriber in subscribers)
                {
                    var tip = db.Tips.Find(subscriber.NextTip);
                    if (tip != null)
                    {
                        var tipMessage = new ScheduledTip()
                        {
                            Destination = subscriber.PhoneNumber,
                            TipNumber = subscriber.NextTip,
                            Tip = tip.Message,
                            ServiceId = subscriber.ServiceId,
                            DateScheduled = DateTime.Now
                        };

                        db.ScheduledTips.Add(tipMessage);

                        subscriber.NextTip += 1;
                        db.Entry(subscriber).State = System.Data.Entity.EntityState.Modified;

                    }
                }
                db.SaveChanges();
            }
        }
    }
}