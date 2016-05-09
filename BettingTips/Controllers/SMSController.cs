using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BettingTips.Models;
using BettingTips.SMS;

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
    }
}