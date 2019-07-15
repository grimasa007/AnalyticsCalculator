using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Double;

namespace AnalyticsCalculator
{
    public class Calculator
    {
        private const string MarketPositionLong = "Long";
        private const string MarketPositionShort = "Short";

        private List<Trade> _mockTrades = new List<Trade>()
        {
            new Trade(){ Profit = -100, MarketPosition = "Short", EntryDate = new DateTime(2018, 01, 12)},
            new Trade(){ Profit = -150, MarketPosition = "Long"},
            new Trade(){ Profit = -30, MarketPosition = "Long", EntryDate = new DateTime(2019, 01, 01)},
            new Trade(){ Profit = -50, MarketPosition = "Short"},
            new Trade(){ Profit = -200, MarketPosition = "Long"},
            new Trade(){ Profit = 80, MarketPosition = "Short"},
            new Trade(){ Profit = 60, MarketPosition = "Long"},
            new Trade(){ Profit = 70, MarketPosition = "Short"},
            new Trade(){ Profit = 100, MarketPosition = "Short", ExitDate = new DateTime(2019, 02, 01)},
            new Trade(){ Profit = -70, MarketPosition = "Long", EntryDate = new DateTime(2019, 03, 01)},
            new Trade(){ Profit = 20, MarketPosition = "Long"},
            new Trade(){ Profit = -300, MarketPosition = "Long" , ExitDate = new DateTime(2019, 03, 28)},
            new Trade(){ Profit = 500, MarketPosition = "Short", ExitDate = new DateTime(2019, 04, 01)},

        };

        private List<DrawDown> _drawDowns;

        private double _netProfit;
        private double _netProfitLong;
        private double _netProfitShort;

        private double _profitFactor;
        private double _profitFactorLong;
        private double _profitFactorShort;

        private double _maxDrawDown;
        private DateTime _maxDrawDownDate;
        private double _averageMaxDrawDown;
        private double _maxDrawDownPeriodDays;
        private double _avDrawDownPeriodDays;
        private double _maxDDBelowZero;
        private string _netProfitMaxDrawDownRatio;

        private int _trades;
        private int _tradesLong;
        private int _tradesShort;
        private double _tradesPerYear;
        private double _tradesPerMonth;

        //остально делать не слат думаю там все понятно
        

        public double NetProfit
        {
            get { return _netProfit; }
            set { _netProfit = value; }
        }

        public double NetProfitLong
        {
            get { return _netProfitLong; }
            set { _netProfitLong = value; }
        }

        public double NetProfitShort
        {
            get { return _netProfitShort; }
            set { _netProfitShort = value; }
        }

        public double ProfitFactor
        {
            get { return _profitFactor; }
            set { _profitFactor = value; }
        }

        public double ProfitFactorLong
        {
            get { return _profitFactorLong; }
            set { _profitFactorLong = value; }
        }

        public double ProfitFactorShort
        {
            get { return _profitFactorShort; }
            set { _profitFactorShort = value; }
        }

        public double MaxDrawDown
        {
            get { return _maxDrawDown; }
            set { _maxDrawDown = value; }
        }

        public List<Trade> MockTrades
        {
            get { return _mockTrades; }
            set { _mockTrades = value; }
        }

        public List<DrawDown> DrawDowns
        {
            get { return _drawDowns; }
            set { _drawDowns = value; }
        }

        public DateTime MaxDrawDownDate
        {
            get { return _maxDrawDownDate; }
            set { _maxDrawDownDate = value; }
        }

        public double AverageMaxDrawDown
        {
            get { return _averageMaxDrawDown; }
            set { _averageMaxDrawDown = value; }
        }

        public double MaxDrawDownPeriodDays
        {
            get { return _maxDrawDownPeriodDays; }
            set { _maxDrawDownPeriodDays = value; }
        }

        public double AvDrawDownPeriodDays
        {
            get { return _avDrawDownPeriodDays; }
            set { _avDrawDownPeriodDays = value; }
        }

        public double MaxDdBelowZero
        {
            get { return _maxDDBelowZero; }
            set { _maxDDBelowZero = value; }
        }

        public string NetProfitMaxDrawDownRatio
        {
            get { return _netProfitMaxDrawDownRatio; }
            set { _netProfitMaxDrawDownRatio = value; }
        }

        public int Trades
        {
            get { return _trades; }
            set { _trades = value; }
        }

        public int TradesLong
        {
            get { return _tradesLong; }
            set { _tradesLong = value; }
        }

        public int TradesShort
        {
            get { return _tradesShort; }
            set { _tradesShort = value; }
        }

        public double TradesPerMonth
        {
            get { return _tradesPerMonth; }
            set { _tradesPerMonth = value; }
        }

        public double TradesPerYear
        {
            get { return _tradesPerYear; }
            set { _tradesPerYear = value; }
        }


        public void Calculate()
        {

            //Net Profit ----------
            _netProfit = _mockTrades.Sum(t => t.Profit);
            _netProfitLong = _mockTrades.Where(t => t.MarketPosition == MarketPositionLong).Sum(t => t.Profit);
            _netProfitShort = _mockTrades.Where(t => t.MarketPosition == MarketPositionShort).Sum(t => t.Profit);

            //Profit Factor -------------
            _profitFactor = GetProfitFactor();
            _profitFactorLong = GetProfitFactor(MarketPositionLong);
            _profitFactorShort = GetProfitFactor(MarketPositionShort);


            //Draw Downs ---------------------
            _drawDowns = new List<DrawDown>();
            double currentDD = 0;
            double lastEquityHigh = 0;

            for (int i = 0; i < _mockTrades.Count; i++)
            {
                
                if (i == 0)
                {
                    _mockTrades[i].CumProfit = _mockTrades[i].Profit;

                    if (_mockTrades[i].Profit > 0)
                        lastEquityHigh = _mockTrades[i].CumProfit;

                    else if (_mockTrades[i].Profit < 0)
                    {
                        lastEquityHigh = 0;
                        _mockTrades[i].CurrentDrawDown = _mockTrades[i].Profit;
                    }

                    if (_mockTrades[i].CumProfit < _maxDDBelowZero)
                    {
                        _maxDDBelowZero = _mockTrades[i].CumProfit;
                    }

                    if (lastEquityHigh == 0)
                    {
                        var dd = new DrawDown()
                        {
                            StartDate = _mockTrades[i].EntryDate,
                            Values = new List<double>()
                            {
                                _mockTrades[i].CurrentDrawDown
                            }

                        };

                        _drawDowns.Add(dd);
                    }

                }
                if (i > 0)
                {
                    _mockTrades[i].CumProfit = _mockTrades[i].Profit + _mockTrades[i-1].CumProfit;

                    if (_mockTrades[i].CumProfit > lastEquityHigh)
                    {
                        //add draw down period end date and calculate period Max Draw Down
                        if (_mockTrades[i - 1].CumProfit < lastEquityHigh)
                        {
                            _drawDowns[_drawDowns.Count - 1].EndDate = _mockTrades[i].ExitDate;

                            _drawDowns[_drawDowns.Count - 1].PeriodMaxDrawDown =
                                _drawDowns[_drawDowns.Count - 1].Values.Min();

                            var period = _drawDowns[_drawDowns.Count - 1].EndDate -
                                         _drawDowns[_drawDowns.Count - 1].StartDate;

                            _drawDowns[_drawDowns.Count - 1].DrawDownPeriodDays = period.TotalHours / 24;

                        }

                        lastEquityHigh = _mockTrades[i].CumProfit;
                        _mockTrades[i].CurrentDrawDown = 0;

                        


                    }
                    else if (_mockTrades[i].CumProfit < lastEquityHigh)
                    {
                        
                        _mockTrades[i].CurrentDrawDown = _mockTrades[i].CumProfit - lastEquityHigh;

                        //draw down started
                        if (_mockTrades[i - 1].CumProfit > lastEquityHigh)
                        {
                            var dd = new DrawDown()
                            {
                                StartDate = _mockTrades[i].EntryDate,
                                Values = new List<double>()
                                {
                                    _mockTrades[i].CurrentDrawDown
                                }

                            };

                            _drawDowns.Add(dd);
                        }
                        else
                        {
                            //adding current draw down values to draw down object list of values
                            _drawDowns[_drawDowns.Count-1].Values.Add(_mockTrades[i].CurrentDrawDown);
                        }

                        //evaluating maximum draw down and max draw down date
                        if (_mockTrades[i].CurrentDrawDown < MaxDrawDown)
                        {
                            _maxDrawDown = _mockTrades[i].CurrentDrawDown;
                            _maxDrawDownDate = _mockTrades[i].ExitDate;
                        }
                    }

                    if (_mockTrades[i].CumProfit < _maxDDBelowZero)
                    {
                        _maxDDBelowZero = _mockTrades[i].CumProfit;
                    }

                }
            }

            _averageMaxDrawDown = _drawDowns.Average(v => v.PeriodMaxDrawDown);
            _maxDrawDownPeriodDays = _drawDowns.Max(d => d.DrawDownPeriodDays);
            _avDrawDownPeriodDays = _drawDowns.Average(d => d.DrawDownPeriodDays);
            _netProfitMaxDrawDownRatio = CalcMaxDrawDownNetProfitRatio();


            //Trades ----------------------
            _trades = _mockTrades.Count;
            _tradesLong = _mockTrades.Count(l => l.MarketPosition == MarketPositionLong);
            _tradesShort = _mockTrades.Count(l => l.MarketPosition == MarketPositionShort);

            _tradesPerMonth = GetTradesPerDay() * 30;
            _tradesPerYear = GetTradesPerDay() * 365;

        }

        private double GetTradesPerDay()
        {
            if (_mockTrades.Count == 0)
                return 0;

            var periodStart = _mockTrades[0].EntryDate;
            var periodEnd = _mockTrades[_mockTrades.Count - 1].ExitDate;

            var periodLengthDays = (periodEnd - periodStart).TotalDays;
            var tradesPerDay = _trades / periodLengthDays;

            return Math.Round(tradesPerDay, 2);
        }

        private string CalcMaxDrawDownNetProfitRatio()
        {
            try
            {
                var netProfit = Math.Abs(NetProfit / MaxDrawDown);
                string np = Math.Round(netProfit, 1).ToString();

                var dd = MaxDrawDown / MaxDrawDown;

                return np + ":" + dd;
            }
            catch (DivideByZeroException)
            {
                return "NaN";
            }

            
        }

        private double GetProfitFactor(string type = null)
        {
            try
            {
                if (type == null)
                {
                    var grossProfit = _mockTrades.Where(t => t.Profit > 0).Sum(t => t.Profit);
                    var grossLoss = _mockTrades.Where(t => t.Profit < 0).Sum(t => t.Profit);
                    var profitFactor = grossProfit / Math.Abs(grossLoss);

                    return Math.Round(profitFactor, 2);
                }
                if (type == MarketPositionLong)
                {
                    var grossProfit = _mockTrades.Where(t => t.Profit > 0 && t.MarketPosition == MarketPositionLong)
                        .Sum(t => t.Profit);

                    var grossLoss = _mockTrades.Where(t => t.Profit < 0 && t.MarketPosition == MarketPositionLong)
                        .Sum(t => t.Profit);

                    var profitFactor = grossProfit / Math.Abs(grossLoss);

                    return Math.Round(profitFactor, 2);
                }
                if (type == MarketPositionShort)
                {
                    var grossProfit = _mockTrades.Where(t => t.Profit > 0 && t.MarketPosition == MarketPositionShort)
                        .Sum(t => t.Profit);

                    var grossLoss = _mockTrades.Where(t => t.Profit < 0 && t.MarketPosition == MarketPositionShort)
                        .Sum(t => t.Profit);

                    var profitFactor = grossProfit / Math.Abs(grossLoss);

                    return Math.Round(profitFactor, 2);
                }
            }
            catch (DivideByZeroException)
            {
                return NaN;
            }

            return NaN;

        }
    }
}
