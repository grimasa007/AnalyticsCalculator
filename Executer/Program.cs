using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalyticsCalculator;

namespace Executer
{
    class Program
    {
        static void Main(string[] args)
        {
            var calc = new Calculator();
            calc.Calculate();

            Console.WriteLine(
                "NET PROFIT--------------------" + Environment.NewLine +
                "Net Profit: " + calc.NetProfit + Environment.NewLine +
                "Net Profit Long: " + calc.NetProfitLong + Environment.NewLine +
                "Net Profit Short: " + calc.NetProfitShort + Environment.NewLine +
                "PROFIT FACTOR--------------------" + Environment.NewLine +
                "Profit: " + calc.ProfitFactor + Environment.NewLine +
                "Profit Factor Long: " + calc.ProfitFactorLong + Environment.NewLine +
                "Profit Factor Short: " + calc.ProfitFactorShort + Environment.NewLine +
                "DRAW DOWNS--------------------" + Environment.NewLine +
                "Max Draw Down: " + calc.MaxDrawDown + Environment.NewLine +
                "Number of Draw Down Periods: " + calc.DrawDowns.Count + Environment.NewLine +
                "Max Draw Down Date: " + calc.MaxDrawDownDate + Environment.NewLine +
                "Average Max Draw Down: " + calc.AverageMaxDrawDown + Environment.NewLine +
                "Max Draw Down Period Days: " + calc.MaxDrawDownPeriodDays + Environment.NewLine +
                "Average Draw Down Period Days: " + calc.AvDrawDownPeriodDays + Environment.NewLine +
                "Max Draw Down Below Zero Line: " + calc.MaxDdBelowZero + Environment.NewLine +
                "Net Profit to DD Ratio: " + calc.NetProfitMaxDrawDownRatio + Environment.NewLine +
                "Trades: " + calc.Trades + Environment.NewLine +
                "Trades Long: " + calc.TradesLong + Environment.NewLine +
                "Trades Short: " + calc.TradesShort + Environment.NewLine +
                "Trades Per Month: " + calc.TradesPerMonth + Environment.NewLine +
                "Trades Per Year: " + calc.TradesPerYear + Environment.NewLine 

                );

            Console.WriteLine("Last Trade Cum Profit = Net Profit " + calc.MockTrades[calc.MockTrades.Count-1].CumProfit + Environment.NewLine);

            foreach (var d in calc.DrawDowns)
            {
                Console.WriteLine("Draw Down Period Break--------------- " + Environment.NewLine +
                                  "DD Start: " + d.StartDate + Environment.NewLine +
                                  "DD End: " + d.EndDate + Environment.NewLine +
                                  "Max Draw Down of Period: " + d.PeriodMaxDrawDown + Environment.NewLine +
                                  "Duration of Period: " + d.DrawDownPeriodDays + Environment.NewLine

                                  );


                foreach (var v in d.Values)
                {
                    Console.WriteLine(v);
                }
            }

            


            Console.ReadKey();
        }
    }
}
