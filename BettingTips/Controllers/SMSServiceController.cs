using BettingTips.Models;
using BettingTips.SMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;

namespace BettingTips.Controllers
{
    public class SMSServiceController : ApiController
    {
        [Route("SMSService/ReceiveDeliveryNotification")]
        [HttpPost]
        public IHttpActionResult ReceiveDeliveryNotification()
        {
            XNamespace ns1 = ShortCode.SOAPRequestNamespaces["ns1"];
            XNamespace ns2 = ShortCode.SOAPRequestNamespaces["ns2"];
            XNamespace loc = ShortCode.SOAPRequestNamespaces["loc"];
            XNamespace v2 = ShortCode.SOAPRequestNamespaces["v2"];

            string notificationSoapString = Request.Content.ReadAsStringAsync().Result;
            XElement soapEnvelope = XElement.Parse(notificationSoapString);

            string destination = (string)
                                    (from el in soapEnvelope.Descendants("address")
                                     select el).First();
            destination = destination.Substring(4);


            string deliveryStatus = (string)
                                        (from el in soapEnvelope.Descendants("deliveryStatus")
                                         select el).First();

            string serviceId = (string)
                                    (from el in soapEnvelope.Descendants(ns1 + "serviceId")
                                     select el).First();

            int correlator = (int)
                                (from el in soapEnvelope.Descendants(ns2 + "correlator")
                                 select el).First();

            string traceUniqueId = (string)
                                        (from el in soapEnvelope.Descendants(ns1 + "traceUniqueID")
                                         select el).First();

            using (var db = new ApplicationDbContext())
            {
                
                var deliveryNotification = new Delivery()
                {
                    Destination = destination,
                    DeliveryStatus = deliveryStatus,
                    ServiceId = serviceId,
                    Correlator = correlator,
                    TimeStamp = DateTime.Now,
                    TraceUniqueId = traceUniqueId
                };

                if (deliveryStatus.ToLower().Equals("deliveredtoterminal"))
                {
                    var subscribers = db.Subscribers.Where(s => s.PhoneNumber.Equals(destination));
                    if (subscribers.Any() && subscribers.First().isActive)
                    {
                        subscribers.First().NextTip += 1;
                    }
                }

                db.Deliveries.Add(deliveryNotification);
                db.SaveChanges();

                //return CreatedAtRoute("DefaultApi", new { id = deliveryNotification.Id }, deliveryNotification);
                return Ok(ShortCode.GetNotifySmsResponse()); 
            }
        }
    }
}
