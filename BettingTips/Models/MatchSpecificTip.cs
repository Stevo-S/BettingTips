using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BettingTips.Models
{
    public class MatchSpecificTip
    {
        public int Id { get; set; }
        
        [DataType(DataType.MultilineText)]
        [StringLength(900)]
        public string Tip { get; set; }

        [Index]
        public DateTime SendTime { get; set; }

        [Index]
        public DateTime Expiration { get; set; }
    }
}