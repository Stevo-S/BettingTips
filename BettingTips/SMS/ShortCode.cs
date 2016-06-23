﻿using BettingTips.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BettingTips.SMS
{
    public class ShortCode
    {
        private static string username = ConfigurationManager.AppSettings["SPUserName"];
        private static string password = ConfigurationManager.AppSettings["SPPassword"];
        private static string HostPPPAddress = ConfigurationManager.AppSettings["ServerPPPIPAddress"];
        private static string spID = ConfigurationManager.AppSettings["SPID"];
        public static Random correlatorGenerator = new Random();
        //private string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

        public static readonly Dictionary<string, string> SOAPRequestNamespaces = new Dictionary<string, string>()
        {
            { "soapenv",  "http://schemas.xmlsoap.org/soap/envelope/" },
            { "v2",  "http://www.huawei.com.cn/schema/common/v2_1" },
            { "loc", "http://www.csapi.org/schema/parlayx/sms/send/v2_2/local" },
            { "ns1", "http://www.huawei.com.cn/schema/common/v2_1" },
            { "ns2", "http://www.csapi.org/schema/parlayx/sms/notification/v2_2/local" }
        };

        private static string notifyDeliverySMSResponse = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"" +
                                "xmlns:loc=\"http://www.csapi.org/schema/parlayx/sms/notification/v2_2/local\">" +
                                "<soapenv:Header/>" +
                                "<soapenv:Body>" +
                                    "<loc:notifySmsDeliveryReceiptResponse/>" +
                                "</soapenv:Body>" +
                                "</soapenv:Envelope>";

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
        
        public static string GetHostPPPAddress()
        {
            return HostPPPAddress;
        }

        //public static void sendScheduledTips()
        //{

        //    using (var db = new ApplicationDbContext())
        //    {
        //        var scheduledTips = db.ScheduledTips.Where(st => st.DateScheduled > DateTime.Today);

        //        foreach (var tip in scheduledTips)
        //        {
        //            Message sms = new Message() { Destination = tip.Destination, Text = tip.Tip, Correlator = tip.Id.ToString() };
        //            sms.Send();
        //        }
        //    }
        //}
        

        public static string HashPassword(string input)
        {
            return string.Join("", MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input)).Select(s => s.ToString("x2")));
        }

        public static string GetNotifySmsResponse()
        {
            return notifyDeliverySMSResponse;
        }
    }
    
}