﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Quiz_System.Models
{
    public class ScoreViewModel
    {
        public score score { get; set; }
        public student student { get; set; }
        public test test { get; set; }
    }
}