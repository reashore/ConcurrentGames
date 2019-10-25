using System;
using System.Collections.Generic;
using System.Configuration;
using SportsIq.Models.Constants.Ncaafb;
using SportsIq.Models.Markets;
using SportsIq.Utilities;
using static System.Convert;

namespace SportsIq.Analytica.Ncaafb
{
    public interface IAnalyticaNcaafb
    {
        bool IsTeamMode { get; set; }
        Dictionary<int, List<Market>> RunModel(Dictionary<string, Dictionary<string, double>> modelData, Dictionary<int, List<Market>> periodMarkets, bool started, string saveFileName);
    }

    public class AnalyticaNcaafb : AnalyticaBase, IAnalyticaNcaafb
    {
        private readonly double _preVig;
        private readonly double _liveVig;
        private readonly int _iSw;
        private readonly int _iAdjT;
        private readonly int _iAdjS;

        public AnalyticaNcaafb()
        {
            _preVig = ToDouble(ConfigurationManager.AppSettings["preVig"]);
            _liveVig = ToDouble(ConfigurationManager.AppSettings["liveVig"]);
            _iSw = 1;
            _iAdjS = 10;
            _iAdjT = 10;
        }

        #region Public Methods

        public Dictionary<int, List<Market>> RunModel(Dictionary<string, Dictionary<string, double>> modelData, Dictionary<int, List<Market>> periodMarkets, bool started, string saveFileName)
        {
            Dictionary<int, List<Market>> result;

            if (IsTeamMode)
            {
                result = RunTeamModel(modelData, periodMarkets, started, saveFileName);
            }
            else
            {
                result = RunPlayerModel(modelData, periodMarkets, started, saveFileName);
            }

            return result;
        }

        #endregion

        #region Private Methods

        private Dictionary<int, List<Market>> RunTeamModel(Dictionary<string, Dictionary<string, double>> modelData, Dictionary<int, List<Market>> periodMarkets, bool started, string saveFileName)
        {
            if (!IsModelDataValid(modelData))
            {
                Logger.Error("RunTeamModel(): ModelData is invalid");
                //return periodMarkets;
            }

            try
            {
                Dictionary<string, string[]> tableIndicesDictionary = new Dictionary<string, string[]>
                    {
                        {NcaafbModelDataKeys.InMlf,   new[] {"iGP", "iBL", "iGL2"}},                    // Market Odds
                        {NcaafbModelDataKeys.InLMlf,  new[] { "iGP", "iBL", "iGL2"}},                   // Live Market Odds
                        {NcaafbModelDataKeys.Evs,     new[] {"iTVH", "iGP"}},                           // Game State
                        {NcaafbModelDataKeys.Egt,     new[] {"iGSI"}},                                  // Score
                        {NcaafbModelDataKeys.InTsf,   new[] {"iTVH","iQT","iSAS","iGT"}},
                        {NcaafbModelDataKeys.InSc,    new[] {"iTVH","iQT","iGT"}}
                        //{NflModelDataKeys.Xs,    new[] { "iTVH", "iASC"}}
                    };

                LoadTables(tableIndicesDictionary, modelData);

                try
                {
                    bool live = false;

                    if (!modelData.ContainsKey(NcaafbModelDataKeys.InLMlf))
                    {
                        bool arePeriodMarketsValid1 = modelData[NcaafbModelDataKeys.InLMlf]["CG,TO,T"].IsNotEqualToZero();
                        bool arePeriodMarketsValid3 = modelData[NcaafbModelDataKeys.InLMlf]["CG,ML,S1"].IsNotEqualToZero();
                        live = arePeriodMarketsValid1 || arePeriodMarketsValid3;
                    }

                    CalculateMarkets(periodMarkets, started, live);
                }
                catch (Exception exception)
                {
                    Logger.Error(exception);
                }

                SaveModel(saveFileName);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }

            return periodMarkets;
        }

        private Dictionary<int, List<Market>> RunPlayerModel(Dictionary<string, Dictionary<string, double>> modelData, Dictionary<int, List<Market>> periodMarkets, bool started, string saveFileName)
        {
            return new Dictionary<int, List<Market>>();
        }

        private static bool IsModelDataValid(IReadOnlyDictionary<string, Dictionary<string, double>> modelData)
        {
            if (!modelData.ContainsKey(NcaafbModelDataKeys.InMlf))
            {
                return false;
            }

            Dictionary<string, double> inMlf = modelData[NcaafbModelDataKeys.InMlf];

            if (inMlf.Count == 0)
            {
                return false;
            }

            bool arePeriodMarketsValid1 = inMlf["CG,TO,T"].IsNotEqualToZero();
            bool arePeriodMarketsValid2 = inMlf["CG,SP,S1"].IsNotEqualToZero();
            bool arePeriodMarketsValid3 = inMlf["CG,ML,S1"].IsNotEqualToZero();
            bool arePeriodMarketsValid = arePeriodMarketsValid1 || arePeriodMarketsValid2 || arePeriodMarketsValid3;

            return arePeriodMarketsValid;
        }

        private void CalculateMarkets(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            GetMainLinesTwoWaysResults(periodMarkets, started, live);
            GetMoneyTwoWaysResults(periodMarkets, started);

            GetMoneyThreeWaysResults(periodMarkets, started);

            GetMainLinesThreeWaysResults(periodMarkets, started);

            GetTeamTotalsTwoWaysResults(periodMarkets, started);

            GetTeamTotalsThreeWaysResults(periodMarkets, started);
        }

        private void GetMainLinesTwoWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            const string tableName = "ML2";
            string[] indexArray = { "iGP", "iALT", "iTGT", "iGL2", "iSW", "iAdjT", "iAdjS" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);
            int lowerBound1 = array.GetLowerBound(1);
            int upperBound1 = array.GetUpperBound(1);

            for (int gp = lowerBound0; gp <= upperBound0; gp++)
            {
                for (int altp = lowerBound1; altp <= upperBound1; altp++)
                {
                    string s1 = "U";
                    string s2 = "O";

                    double value1 = GetArrayValue(array, gp, altp, 2, 1, _iSw, _iAdjS, _iAdjT);
                    double value2 = GetArrayValue(array, gp, altp, 2, 2, _iSw, _iAdjS, _iAdjT);
                    double value3 = GetArrayValue(array, gp, altp, 2, 3, _iSw, _iAdjS, _iAdjT);

                    if (value3.IsNotEqualToZero())
                    {
                        //Logger.Debug("value3 is not equal to zero");
                    }

                    MarketRunner marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value2,
                        Side = s1
                    };

                    Market market = new Market
                    {
                        Tp = "TL",
                        Target = marketRunner.Total
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value3,
                        Side = s2
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    if (!periodMarkets.ContainsKey(gp))
                    {
                        List<Market> markets = new List<Market>();
                        periodMarkets.Add(gp, markets);
                    }

                    market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{gp}{market.Tp}";

                    periodMarkets[gp].Add(market);


                    // spreads
                    // get totals
                    s1 = "A";
                    s2 = "H";

                    value1 = GetArrayValue(array, gp, altp, 1, 1, _iSw, _iAdjS, _iAdjT);
                    value2 = GetArrayValue(array, gp, altp, 1, 2, _iSw, _iAdjS, _iAdjT);
                    value3 = GetArrayValue(array, gp, altp, 1, 3, _iSw, _iAdjS, _iAdjT);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value2,
                        Side = s1
                    };

                    market = new Market
                    {
                        Tp = "SP",
                        Target = marketRunner.Total
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = -value1,
                        Price = value3,
                        Side = s2
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    if (market.MarketRunnerList[0].Price > 0)
                    {
                        market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                        market.Key = $"{market.MarketRunnerList[0].Total}{gp}{market.Tp}";

                        periodMarkets[gp].Add(market);
                    }
                }
            }
        }

        private void GetMoneyTwoWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started)
        {
            const string tableName = "MNL2";
            string[] indexArray = { "iGP", "iGL2", "iSW", "iAdjT", "iAdjS" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);

            for (int gp = lowerBound0; gp <= upperBound0; gp++)
            {
                const string s1 = "A";
                const string s2 = "H";

                double value1 = GetArrayValue(array, gp, 1, _iSw, _iAdjS, _iAdjT);
                double value2 = GetArrayValue(array, gp, 2, _iSw, _iAdjS, _iAdjT);
                double value3 = GetArrayValue(array, gp, 3, _iSw, _iAdjS, _iAdjT);

                if (value3.IsNotEqualToZero())
                {
                    //Logger.Debug("value3 is not equal to zero");
                }

                MarketRunner marketRunner = new MarketRunner
                {
                    Total = value1,
                    Price = value2,
                    Side = s1
                };

                Market market = new Market
                {
                    Tp = "ML",
                    Target = marketRunner.Total
                };

                market.MarketRunnerList.Add(marketRunner);

                marketRunner = new MarketRunner
                {
                    Total = value1,
                    Price = value3,
                    Side = s2
                };

                market.MarketRunnerList.Add(marketRunner);

                if (!periodMarkets.ContainsKey(gp))
                {
                    List<Market> markets = new List<Market>();
                    periodMarkets.Add(gp, markets);
                }


                market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                market.Key = $"{market.MarketRunnerList[0].Total}{gp}{market.Tp}";

                periodMarkets[gp].Add(market);

            }
        }

        private void GetMoneyThreeWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started)
        {
            const string tableName = "MNL3";
            string[] indexArray = { "iGP", "iGL3", "iSW", "iAdjT", "iAdjS" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);

            for (int gp = lowerBound0; gp <= upperBound0; gp++)
            {
                const string s1 = "A";
                const string s2 = "H";
                const string s3 = "X";

                double value1 = GetArrayValue(array, gp, 1, _iSw, _iAdjS, _iAdjT);
                double value2 = GetArrayValue(array, gp, 2, _iSw, _iAdjS, _iAdjT);
                double value3 = GetArrayValue(array, gp, 3, _iSw, _iAdjS, _iAdjT);
                double value4 = GetArrayValue(array, gp, 4, _iSw, _iAdjS, _iAdjT);

                if (value3.IsNotEqualToZero())
                {
                    //Logger.Debug("value3 is not equal to zero");
                }

                MarketRunner marketRunner = new MarketRunner
                {
                    Total = value1,
                    Price = value2,
                    Side = s1
                };

                Market market = new Market
                {
                    Tp = "ML3",
                    Target = marketRunner.Total
                };

                market.MarketRunnerList.Add(marketRunner);

                marketRunner = new MarketRunner
                {
                    Total = value1,
                    Price = value3,
                    Side = s2
                };

                market.MarketRunnerList.Add(marketRunner);

                marketRunner = new MarketRunner
                {
                    Total = value1,
                    Price = value4,
                    Side = s3
                };

                market.MarketRunnerList.Add(marketRunner);

                if (!periodMarkets.ContainsKey(gp))
                {
                    List<Market> markets = new List<Market>();
                    periodMarkets.Add(gp, markets);
                }

                market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                market.Key = $"{market.MarketRunnerList[0].Total}{gp}{market.Tp}";

                periodMarkets[gp].Add(market);

            }
        }

        private void GetMainLinesThreeWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started)
        {
            const string tableName = "ML3";
            string[] indexArray = { "iGP", "iALT", "iTGT", "iGL3", "iSW", "iAdjT", "iAdjS" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);
            int lowerBound1 = array.GetLowerBound(1);
            int upperBound1 = array.GetUpperBound(1);

            for (int period = lowerBound0; period <= upperBound0; period++)
            {
                for (int alt = lowerBound1; alt <= upperBound1; alt++)
                {
                    string s1 = "U";
                    string s2 = "O";
                    const string s3 = "X";

                    double value1 = GetArrayValue(array, period, alt, 2, 1, _iSw, _iAdjS, _iAdjT);
                    double value2 = GetArrayValue(array, period, alt, 2, 2, _iSw, _iAdjS, _iAdjT);
                    double value3 = GetArrayValue(array, period, alt, 2, 3, _iSw, _iAdjS, _iAdjT);
                    double value4 = GetArrayValue(array, period, alt, 2, 4, _iSw, _iAdjS, _iAdjT);

                    MarketRunner marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value2,
                        Side = s1
                    };

                    Market market = new Market
                    {
                        Tp = "TL3W",
                        Target = marketRunner.Total
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value3,
                        Side = s2
                    };
                    market.Target = marketRunner.Total;
                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value4,
                        Side = s2
                    };
                    market.MarketRunnerList.Add(marketRunner);

                    if (!periodMarkets.ContainsKey(period))
                    {
                        List<Market> markets = new List<Market>();
                        periodMarkets.Add(period, markets);
                    }

                    market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";
                    periodMarkets[period].Add(market);


                    // spreads
                    // get totals

                    s1 = "A";
                    s2 = "H";

                    value1 = GetArrayValue(array, period, alt, 1, 1, _iSw, _iAdjS, _iAdjT);
                    value2 = GetArrayValue(array, period, alt, 1, 2, _iSw, _iAdjS, _iAdjT);
                    value3 = GetArrayValue(array, period, alt, 1, 3, _iSw, _iAdjS, _iAdjT);
                    value4 = GetArrayValue(array, period, alt, 1, 4, _iSw, _iAdjS, _iAdjT);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value2,
                        Side = s1
                    };

                    market = new Market
                    {
                        Tp = "SP3W",
                        Target = marketRunner.Total
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = -value1,
                        Price = value3,
                        Side = s2
                    };

                    market.Target = marketRunner.Total;
                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value4,
                        Side = s3
                    };
                    market.MarketRunnerList.Add(marketRunner);


                    market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";

                    periodMarkets[period].Add(market);

                }
            }
        }

        private void GetTeamTotalsTwoWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started)
        {
            const string tableName = "TTL2";
            string[] indexArray = { "iGP", "iALT", "iTVH", "iGL2", "iSW" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);
            int lowerBound1 = array.GetLowerBound(1);
            int upperBound1 = array.GetUpperBound(1);

            for (int period = lowerBound0; period <= upperBound0; period++)
            {
                for (int alt = lowerBound1; alt <= upperBound1; alt++)
                {
                    const string s1 = "U";
                    const string s2 = "O";

                    double value1 = GetArrayValue(array, period, alt, 1, 1, _iSw);
                    double value2 = GetArrayValue(array, period, alt, 1, 2, _iSw);
                    double value3 = GetArrayValue(array, period, alt, 1, 3, _iSw);

                    MarketRunner marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value2,
                        Side = s1
                    };

                    Market market = new Market
                    {
                        Tp = "ATTL",
                        Target = marketRunner.Total
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value3,
                        Side = s2
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    if (!periodMarkets.ContainsKey(period))
                    {
                        List<Market> markets = new List<Market>();
                        periodMarkets.Add(period, markets);
                    }


                    market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";

                    periodMarkets[period].Add(market);

                    market = new Market
                    {
                        Tp = "HTTL",
                        Target = marketRunner.Total
                    };
                    //market.Tp = "HTTL";

                    value1 = GetArrayValue(array, period, alt, 2, 1, _iSw);
                    value2 = GetArrayValue(array, period, alt, 2, 2, _iSw);
                    value3 = GetArrayValue(array, period, alt, 2, 3, _iSw);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value2,
                        Side = s1
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value3,
                        Side = s2
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    if (!periodMarkets.ContainsKey(period))
                    {
                        List<Market> markets = new List<Market>();
                        periodMarkets.Add(period, markets);
                    }


                    market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";

                    periodMarkets[period].Add(market);

                    //********************************************
                }
            }
        }

        private void GetTeamTotalsThreeWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started)
        {
            const string tableName = "TTL3";
            string[] indexArray = { "iGP", "iALT", "iTVH", "iGL3", "iSW" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);
            int lowerBound1 = array.GetLowerBound(1);
            int upperBound1 = array.GetUpperBound(1);

            for (int period = lowerBound0; period <= upperBound0; period++)
            {
                for (int alt = lowerBound1; alt <= upperBound1; alt++)
                {
                    const string s1 = "U";
                    const string s2 = "O";
                    const string s3 = "X";

                    double value1 = GetArrayValue(array, period, alt, 1, 1, _iSw);
                    double value2 = GetArrayValue(array, period, alt, 1, 2, _iSw);
                    double value4 = GetArrayValue(array, period, alt, 1, 4, _iSw);

                    MarketRunner marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value2,
                        Side = s1
                    };

                    Market market = new Market
                    {
                        Tp = "ATTL3W",
                        Target = marketRunner.Total
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value2,
                        Side = s2
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    // X
                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value4,
                        Side = s3
                    };

                    market.MarketRunnerList.Add(marketRunner);



                    if (!periodMarkets.ContainsKey(period))
                    {
                        List<Market> markets = new List<Market>();
                        periodMarkets.Add(period, markets);
                    }


                    market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";

                    periodMarkets[period].Add(market);

                    // market.Tp = "HTTL3W";

                    value1 = GetArrayValue(array, period, alt, 2, 1, _iSw);
                    value2 = GetArrayValue(array, period, alt, 2, 2, _iSw);
                    double value3 = GetArrayValue(array, period, alt, 2, 3, _iSw);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value2,
                        Side = s1
                    };

                    market = new Market
                    {
                        Tp = "HTTL3W",
                        Target = marketRunner.Total
                    };
                    //market.Target = marketRunner.Total;

                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value3,
                        Side = s2
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value2,
                        Side = s3
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    if (!periodMarkets.ContainsKey(period))
                    {
                        List<Market> markets = new List<Market>();
                        periodMarkets.Add(period, markets);
                    }


                    market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";

                    periodMarkets[period].Add(market);

                }
            }
        }

        #endregion
    }
}
