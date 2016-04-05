using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BettingTips.Models
{
    public class ScheduledTip
    {
        public int Id { get; set; }
        public int TipNumber { get; set; }
        public string Tip { get; set; }
        public DateTime DateScheduled { get; set; }
        public string Destination { get; set; }
        public string ServiceId { get; set; }
    }
}