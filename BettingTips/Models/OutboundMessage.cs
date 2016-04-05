using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BettingTips.Models
{
    public class OutboundMessage
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Message { get; set; }
        public DateTime MessageDate { get; set; }
        public string ServiceId { get; set; }
    }
}