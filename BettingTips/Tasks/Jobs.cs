using BettingTips.Models;
using BettingTips.SMS;
using System;
using System.Collections.Generic;
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

        // Send/re-send scheduled messages
        public static void SendScheduledMessages()
        {
            IEnumerable<ScheduledTip> queuedTips;

            using (var db = new ApplicationDbContext())
            {
                var successfulMessagesIdList = db.Deliveries.Where(d => d.TimeStamp > DateTime.Today && 
                                        d.DeliveryStatus.ToLower().Equals("deliveredtoterminal")).Select(d => d.Correlator).ToList();

                queuedTips = db.ScheduledTips.Where(st => st.DateScheduled > DateTime.Today &&
                                                !successfulMessagesIdList.Contains(st.Id)).ToList();
            }

            foreach (var tip in queuedTips)
            {
                var tipMessage = new Message()
                {
                    Destination = tip.Destination,
                    Text = tip.Tip,
                    Correlator =  tip.Id.ToString()
                };

                tipMessage.Send();
            }
        }
    }
}