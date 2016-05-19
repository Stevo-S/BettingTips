using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BettingTips.Models;
using BettingTips.SMS;
using System.Xml.Linq;
using System.Net;

namespace BettingTips.Controllers
{
    public class SMSController : Controller
    {
        // GET: SMS
        public ActionResult Index()
        {
            return View();
        }

        // GET 
        public ActionResult Send()
        {
            return View();
        }

        // POST 
        [HttpPost]
        public string Send(string destination, string message)
        {
            Message sms = new Message() { Destination = destination, Text = message };
            return sms.Send() + sms.ToXML();
        }

        // Receive Delivery Notifications
        public ActionResult ReceiveDeliveryNotification(string notificationSoapString)
        {
            XElement soapEnvelope = XElement.Parse(notificationSoapString);

            string destination = (string)
                                    (from el in soapEnvelope.Descendants("address")
                                     select el).First();
            destination = destination.Substring(4);

            string deliveryStatus = (string)
                                        (from el in soapEnvelope.Descendants("deliveryStatus")
                                         select el).First();

            string serviceId = (string)
                                    (from el in soapEnvelope.Descendants("serviceId")
                                     select el).First();

            int correlator = (int)
                                (from el in soapEnvelope.Descendants("correlator")
                                 select el).First();

            string traceUniqueId = (string)
                                        (from el in soapEnvelope.Descendants("traceUniqueID")
                                         select el).First();

            var deliveryNotification = new Delivery()
            {
                Destination = destination,
                DeliveryStatus = deliveryStatus,
                ServiceId = serviceId,
                Correlator = correlator,
                TimeStamp = DateTime.Now,
                TraceUniqueId = traceUniqueId
            };

            using (var db = new ApplicationDbContext())
            {
                db.Deliveries.Add(deliveryNotification);
                db.SaveChanges();
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}