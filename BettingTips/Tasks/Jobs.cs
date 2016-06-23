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
        [DisableConcurrentExecution(3600)]
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
        // Scheduled for today whose expiration dates have not yet lapsed
        [DisableConcurrentExecution(3600)]
        [AutomaticRetry(Attempts = 3)]
        public static void SendScheduledMessages()
        {
            IEnumerable<ScheduledTip> queuedTips;

            // Collect all messages scheduled to be sent to day
            // that do not as yet have successful deliveries
            // and whose expiry date has not lapsed
            using (var db = new ApplicationDbContext())
            {

                queuedTips = (from tip in db.ScheduledTips.Where(st => st.DateScheduled > DateTime.Today)
                              join delivery in db.Deliveries.Where(d => d.DeliveryStatus.ToLower().Equals("deliveredtoterminal"))
                              on tip.Id equals delivery.Correlator
                              into gj
                              from delivery in gj.DefaultIfEmpty()
                              where delivery == null
                              select tip).ToList();
            }

            // Send the collected messages if the expiration date has not yet lapsed
            // Send the message asynchronously in batches
            int skipFactor = 512;
            int iterations = (queuedTips.Count() / skipFactor) + 1;
            for (int i = 0; i < iterations; i++)
            {
                Message.SendMany((from tip in queuedTips select tip).Skip(i * skipFactor).Take(skipFactor));
               
            }
            
        }

        // Schedule match specific tips to be sent 
        public static void ScheduleMatchSpecificTip(int matchSpecificTipId)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;
                db.Configuration.ValidateOnSaveEnabled = false;

                //db.Database.ExecuteSqlCommand("UPDATE dbo.Subscribers SET NextMatchTip = @tipId WHERE NextMatchTip < @tipId",
                //    new SqlParameter("@tipId", matchSpecificTipId));

                var subscribers = db.Subscribers.Where(s => s.isActive).ToList();

                foreach (var subscriber in subscribers)
                {
                    int nextMatchTip = subscriber.NextMatchTip < matchSpecificTipId ? matchSpecificTipId : subscriber.NextMatchTip;
                    var tip = db.MatchSpecificTips.Find(nextMatchTip);
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

                        subscriber.NextMatchTip = matchSpecificTipId + 1;
                        db.ScheduledTips.Add(tipMessage);

                    }
                }
                db.SaveChanges();
            }
        }
    }
}