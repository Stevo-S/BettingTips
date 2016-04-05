using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BettingTips.Models
{
    public class InboundMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string ServiceId { get; set; }
        public string ProductId { get; set; }
        public DateTime InDate { get; set; }
        public string UpdateDescription { get; set; }
        public string SyncOrderType { get; set; }
        public string traceUniqueId { get; set; }
    }
}