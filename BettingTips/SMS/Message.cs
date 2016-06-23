using BettingTips.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BettingTips.SMS
{
    public class Message
    {
        public string Destination { get; set; }
        public string Text { get; set; }
        public string Correlator { get; set; }
        public string TimeStamp { get; set; }

        public Message()
        {
            this.TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        public Message(ScheduledTip tip)
        {
            this.TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            this.Text = tip.Tip;
            this.Destination = tip.Destination;
            this.Correlator = (tip.Type == "General" ? "G" : "M") + tip.Id.ToString();
        }
        public string ToXML()
        {
            return buildSMSXML();
        }

        private string buildSMSXML(string serviceId = "6013252000099929", string sender = "20043", string timestampString = "")
        {
            //string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            XNamespace soapenv = ShortCode.SOAPRequestNamespaces["soapenv"];
            XNamespace v2 = ShortCode.SOAPRequestNamespaces["v2"];
            XNamespace loc = ShortCode.SOAPRequestNamespaces["loc"];
            XElement soapEnvelope =
                new XElement(soapenv + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "soapenv", soapenv.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "v2", v2.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "loc", loc.NamespaceName),
                    new XElement(soapenv + "Header",
                        new XElement(v2 + "RequestSOAPHeader",
                            new XElement(v2 + "spId", ShortCode.GetSpID()),
                            new XElement(v2 + "spPassword", ShortCode.HashPassword(ShortCode.GetSpID() + ShortCode.GetPassword() + (string.IsNullOrEmpty(timestampString) ? TimeStamp : timestampString))),
                            new XElement(v2 + "serviceId", "6013252000099929"),
                            new XElement(v2 + "timeStamp", TimeStamp)//,
                                                                     //new XElement(v2 + "linkid"),
                                                                     //new XElement(v2 + "OA", "tel:" + message.GetDestination()),
                                                                     //new XElement(v2 + "FA", "tel:" + message.GetDestination())
                        ), // End of RequestSOAPHeader
                    new XElement(soapenv + "Body",
                        new XElement(loc + "sendSms",
                            new XElement(loc + "addresses", "tel:" + Destination),
                            new XElement(loc + "senderName", sender),
                            new XElement(loc + "message", Text),
                            new XElement(loc + "receiptRequest",
                                new XElement("endpoint", "http://" + ShortCode.GetHostPPPAddress() + "/BettingTips/SMSService/ReceiveDeliveryNotification"),
                                new XElement("interfaceName", "SmsNotification"),
                                new XElement("correlator", Correlator)
                            ) // End of receiptRequest
                        ) // End of sendSms
                    ) // End of Soap Body
                ) // End of Soap Header
            ); // End of Soap Envelope

            return soapEnvelope.ToString();
        }

        public string Send()
        {
            using (var handler = new HttpClientHandler() { Credentials = new NetworkCredential(ShortCode.GetUsername(), ShortCode.HashPassword(ShortCode.GetSpID() + ShortCode.GetPassword() + TimeStamp)) })
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("http://192.168.9.177:8310");

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/SendSmsService/services/SendSms/");
                request.Content = new StringContent(buildSMSXML(), Encoding.UTF8, "text/xml");
                string requestContentString = request.Content.ReadAsStringAsync().Result;
                var result = client.SendAsync(request).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;

                // Log SendSMS Request.
                // Useful for debugging multiple delivery notifications for the same request
                //using (var db = new ApplicationDbContext())
                //{
                //    var webRequest = new Models.WebRequest()
                //    {
                //        Type = "SendSMS",
                //        Content = requestContentString,
                //        Timestamp = DateTime.Now
                //    };
                //    db.WebRequests.Add(webRequest);
                //    db.SaveChanges();
                //}

                return resultContent;
            }

        }

        public static void SendMany(IEnumerable<ScheduledTip> tips)
        {
            string currentTimestampString = DateTime.Now.ToString("yyyyMMddHHmmss");
            List<HttpRequestMessage> requests = new List<HttpRequestMessage>();
            using (var handler = new HttpClientHandler() { Credentials = new NetworkCredential(ShortCode.GetUsername(), ShortCode.HashPassword(ShortCode.GetSpID() + ShortCode.GetPassword() + currentTimestampString)) })
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("http://192.168.9.177:8310");

                foreach (var tip in tips)
                {

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/SendSmsService/services/SendSms/");

                    var message = new Message(tip);
                    request.Content = new StringContent(message.buildSMSXML(timestampString: currentTimestampString), Encoding.UTF8, "text/xml");
                    //string requestContentString = request.Content.ReadAsStringAsync().Result;
                    requests.Add(request);
                }

                //Task[] sendRequests = (from request in requests
                //                       select Task.Run(async () =>
                //                       {
                //                           await client.SendAsync(request);
                //                       })).ToArray();

                //Task.WaitAll(sendRequests, -1);

                var taskList = new List<Task>();
                foreach (var request in requests)
                {
                    taskList.Add(client.SendAsync(request));
                }

                try
                {
                    Task.WaitAll(taskList.ToArray());
                }
                catch (Exception)
                {
                    ;
                }
            }
        }
    }
}
