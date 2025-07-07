using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Quiz_System.Models
{
    public class ClassViewModel
    {
        public @class Class { get; set; }
        public grade grade { get; set; }
        public speciality speciality { get; set; }
    }
}