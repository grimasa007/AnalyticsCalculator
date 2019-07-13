using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsCalculator
{
    public class DrawDown
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<double> Values { get; set; }
        public double PeriodMaxDrawDown { get; set; }
        public double DrawDownPeriodDays { get; set; }

    }
}
