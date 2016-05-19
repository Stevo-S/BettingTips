using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BettingTips.Models
{
    public class Delivery
    {
        public int Id { get; set; }

        [Index]
        [StringLength(20)]
        public string Destination { get; set; }

        [Index]
        [StringLength(100)]
        public string DeliveryStatus { get; set; }

        [StringLength(50)]
        public string ServiceId { get; set; }

        public int Correlator { get; set; }

        [StringLength(100)]
        public string TraceUniqueId { get; set; }

        [Index]
        public DateTime TimeStamp { get; set; }

    }
}