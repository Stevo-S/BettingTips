using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace BettingTips.SMS
{
    public class Message
    {
        public string Destination { get; set; }
        public string Text { get; set; }
        public string TimeStamp { get; set; }

        public Message()
        {
            this.TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        }


        public string ToXML()
        {
            return buildSMSXML();
        }

        private string buildSMSXML(string serviceId = "6013252000099929", string sender = "20043")
        {
            //string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace v2 = "http://www.huawei.com.cn/schema/common/v2_1";
            XNamespace loc = "http://www.csapi.org/schema/parlayx/sms/send/v2_2/local";
            XElement soapEnvelope =
                new XElement(soapenv + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "soapenv", soapenv.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "v2", v2.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "loc", loc.NamespaceName),
                    new XElement(soapenv + "Header",
                        new XElement(v2 + "RequestSOAPHeader",
                            new XElement(v2 + "spId", ShortCode.GetSpID()),
                            new XElement(v2 + "spPassword", ShortCode.
                            HashPassword(ShortCode.GetSpID() + ShortCode.GetPassword() + TimeStamp)),
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
                                new XElement("endpoint", ShortCode.GetDeliveryNotificationEndpoint()),
                                new XElement("interfaceName", "SmsNotification"),
                                new XElement("correlator", ShortCode.correlatorGenerator.Next(1234567))
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
                client.BaseAddress = new Uri("http://196.201.216.13:8310");

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/SendSmsService/services/SendSms/");
                request.Content = new StringContent(buildSMSXML(), Encoding.UTF8, "text/xml");
                
                var result = client.SendAsync(request).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                return resultContent;
            }
        }
    }
}
