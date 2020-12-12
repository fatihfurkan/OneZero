using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportProject
{
    [Serializable]
    public class Formul
    {
        public int DesiMin { get; set; }
        public int DesiMax { get; set; }
        public decimal KSY { get; set; }
        public decimal UO { get; set; }
    }
}