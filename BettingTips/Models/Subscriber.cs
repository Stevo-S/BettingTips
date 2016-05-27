using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BettingTips.Models
{
    public class Subscriber
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string ServiceId { get; set; }
        public DateTime FirstSubscriptionDate { get; set; }
        public DateTime LastSubscriptionDate { get; set; }
        public int NextTip { get; set; }

        [DefaultValue(1)]
        public int NextMatchTip { get; set; }
        public bool isActive { get; set; }
    }
}