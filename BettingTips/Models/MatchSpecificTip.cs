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
        [StringLength(900), Required]
        public string Tip { get; set; }

        [Index, Required]
        public DateTime SendTime { get; set; }

        [Index, Required]
        public DateTime Expiration { get; set; }
    }
}