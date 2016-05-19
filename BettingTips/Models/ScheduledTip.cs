using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BettingTips.Models
{
    public class ScheduledTip
    {
        public int Id { get; set; }
        public int TipNumber { get; set; }
        public string Tip { get; set; }

        [Index]
        public DateTime DateScheduled { get; set; }
        
        [Index]
        [DefaultValue("1970-01-01 00:00:00")]
        public DateTime ExpirationDate { get; set; }

        [StringLength(50)]
        [DefaultValue("General")]
        public string Type { get; set; }

        [Index]
        [StringLength(20)]
        public string Destination { get; set; }
        public string ServiceId { get; set; }
    }
}