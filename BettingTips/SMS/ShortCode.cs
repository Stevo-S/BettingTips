using BettingTips.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BettingTips.SMS
{
    public class ShortCode
    {
        private static string username = "admin@BettersmsM";
        private static string password = "bet3r!sM$";
        private static string deliveryNotificationEndpoint = "http://172.31.183.74/SDP/services/NotifySmsDeliveryReceipt.aspx";
        private static string spID = "601325";
        public static Random correlatorGenerator = new Random();
        //private string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

        public static string GetUsername()
        {
            return username;
        }

        public static string GetPassword()
        {
            return password;
        }

        public static string GetSpID()
        {
            return spID;
        }

        public static string GetDeliveryNotificationEndpoint()
        {
            return deliveryNotificationEndpoint;
        }
        public static void sendScheduledTips()
        {

            using (var db = new ApplicationDbContext())
            {
                var scheduledTips = db.ScheduledTips.Where(st => st.DateScheduled > DateTime.Today);

                foreach (var tip in scheduledTips)
                {
                    Message sms = new Message() { Destination = tip.Destination, Text = tip.Tip };
                    sms.Send();
                }
            }
        }
        

        public static string HashPassword(string input)
        {
            return string.Join("", MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input)).Select(s => s.ToString("x2")));
        }
    }
    
}