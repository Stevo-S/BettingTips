﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BettingTips.Models
{
    public class Tip
    {
        public int Id { get; set; }
        public string Message { get; set; }
    }
}