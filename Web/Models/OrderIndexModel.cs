using Core;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class OrderIndexModel
    {
        public List<SelectItem> StoreList { get; set; }

        public List<SelectItem> ThemeList { get; set; }


        public List<Order> OrderList { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }
    }
}