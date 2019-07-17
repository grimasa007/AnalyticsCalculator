using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsCalculator
{
    public class Trade
    {
        public double EntryPrice { get; set; }
        public double ExitPrice { get; set; }
        public double Profit { get; set; }
        
        public DateTime EntryDate { get; set; }
        public DateTime ExitDate { get; set; }
        public string MarketPosition { get; set; }

        public double CumProfit { get; set; }
        public double CurrentDrawDown { get; set; } 
        public double CurrentDaysNoEquiteHigh { get; set; }

    }
}
