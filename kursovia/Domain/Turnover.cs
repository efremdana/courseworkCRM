using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovia.Domain
{
    public class Turnover
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Denomination { get; set; }
        public string TypeTurnover { get; set; }
        public Turnover() { }

    }
}
