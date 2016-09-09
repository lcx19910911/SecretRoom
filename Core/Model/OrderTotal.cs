using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class OrderTotal
    {
        public DateTime Date { get; set; }
        public int OrderCount { get; set; }
        public int PeopleCount { get; set; }
        public decimal Money { get; set; }
    }

    public class ThemeTotal
    {
        public string ThemeId { get; set; }
        public string ThemeName { get; set; }
        public decimal Money { get; set; }

        public int OrderCount { get; set; }
        public int PeopleCount { get; set; }
    }

    public class PayTotal
    {
        public string PayId { get; set; }
        public string PayName { get; set; }
        public decimal Money { get; set; }

        public int OrderCount { get; set; }
        public int PeopleCount { get; set; }
    }
}
