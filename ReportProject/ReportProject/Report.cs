using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportProject
{
    [Serializable]
    public class Report
    {
        public int SıraNo { get; set; }
        public int Adet { get; set; }
        public int KgDesi { get; set; }
        public string Mesafe { get; set; }
        public decimal Ucret { get; set; }
    }
}