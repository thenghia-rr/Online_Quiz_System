﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Quiz_System.Models
{
    public class StudentQuestViewModel
    {
        public test test { get; set; }
        public question question { get; set; }
        public student_test_detail student_test { get; set; }
    }
}