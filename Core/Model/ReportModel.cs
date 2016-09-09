using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class ReportModel
    {
        public string tbody { get; set; }

        public List<string> time { get; set; }

        public List<Series> series { get; set; }

        public List<PieSeries> pieseries { get; set; }
    }
    

    public class Series
    {
        public string name { get; set; }
        public List<decimal> data { get; set; }
        public decimal y { get; set; }
    }

    public class PieSeries
    {
        public string name { get; set; }

        public List<Series> data { get; set; }

        public string type { get; set; }
    }
    

}