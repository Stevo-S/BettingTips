using BettingTips.Models;
using BettingTips.SMS;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace BettingTips.Tasks
{
    public class Jobs
    {
        [DisableConcurrentExecution(10000000)]
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
                            Type = "General",
                            ServiceId = subscriber.ServiceId,
                            DateScheduled = DateTime.Now,
                            ExpirationDate = DateTime.Now.AddHours(23)
                        };

                        db.ScheduledTips.Add(tipMessage);
                        
                    }
                }
                db.SaveChanges();
            }
        }

        // Send/re-send scheduled messages
        [DisableConcurrentExecution(3600)]
        [AutomaticRetry(Attempts = 3)]
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
                    //Correlator =  (tip.Type == "General" ? "G" : "M") + tip.Id.ToString()
                };

                try
                {
                    tipMessage.Send();
                }
                catch (System.AggregateException ae)
                {
                    continue;
                }
            }
        }

        // Schedule match specific tips to be sent 
        public static void ScheduleMatchSpecificTip(int matchSpecificTipId)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;
                db.Configuration.ValidateOnSaveEnabled = false;

                db.Database.ExecuteSqlCommand("UPDATE dbo.Subscribers SET NextMatchTip = @tipId", 
                    new SqlParameter("@tipId", matchSpecificTipId));

                var subscribers = db.Subscribers.Where(s => s.isActive).ToList();

                foreach (var subscriber in subscribers)
                {
                    var tip = db.MatchSpecificTips.Find(subscriber.NextMatchTip);
                    if (tip != null)
                    {
                        var tipMessage = new ScheduledTip()
                        {
                            Destination = subscriber.PhoneNumber,
                            TipNumber = subscriber.NextTip,
                            Tip = tip.Tip,
                            Type = "Match",
                            ServiceId = subscriber.ServiceId,
                            DateScheduled = DateTime.Now,
                            ExpirationDate = tip.Expiration
                        };

                        db.ScheduledTips.Add(tipMessage);
                        
                    }
                }
                db.SaveChanges();
            }
        }
    }
}