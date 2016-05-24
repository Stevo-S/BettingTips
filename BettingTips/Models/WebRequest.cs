using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BettingTips.Models
{
    public class WebRequest
    {
        public int Id { get; set; }

        [Index]
        [StringLength(50)]
        public string Type { get; set; }

        public string Content { get; set; }

        [Index]
        public DateTime Timestamp { get; set; }
    }
}