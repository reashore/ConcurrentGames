using System;
using System.Collections.Generic;
using System.Configuration;
using SportsIq.Models.Constants.Mlb;
using SportsIq.Models.Markets;
using SportsIq.Utilities;
using static System.Convert;

namespace SportsIq.Analytica.Mlb
{
    public interface IAnalyticaMlb
    {
        bool IsTeamMode { get; set; }
        Dictionary<int, List<Market>> RunModel(Dictionary<string, Dictionary<string, double>> modelData, Dictionary<int, List<Market>> periodMarkets, bool started, string saveFileName);
    }

    public class AnalyticaMlb : AnalyticaBase, IAnalyticaMlb
    {
        private readonly double _preVig;
        private readonly double _liveVig;
        private readonly double _threeWayVig;

        public AnalyticaMlb()
        {
            _preVig = ToDouble(ConfigurationManager.AppSettings["preVig"]);
            _liveVig = ToDouble(ConfigurationManager.AppSettings["liveVig"]);
            _threeWayVig = ToDouble(ConfigurationManager.AppSettings["threeWayVig"]);
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
                return periodMarkets;
            }

            try
            {
                Dictionary<string, string[]> tableIndicesDictionary = new Dictionary<string, string[]>
                    {
                        {MlbModelDataKeys.InMlf,  new[] {"iGP", "iBL", "iGL2"}},            // Market Odds
                        {MlbModelDataKeys.InLMlf, new[] {"iBL", "iGL2"}},                   // Live Market Odds
                        {MlbModelDataKeys.Evs,    new[] {"iTVH", "iGP"}},                   // Game State
                        {MlbModelDataKeys.Egt,    new[] {"iGT"}}
                        //{MlbModelDataKeys.InTsf,  new[] { "iGP","iTVH" }}
                    };

                Dictionary<string, double> evsTemp = new Dictionary<string, double>();

                foreach (KeyValuePair<string, double> sccores in modelData[MlbModelDataKeys.Evs])
                {
                    if (!sccores.Key.Contains("I10") && !sccores.Key.Contains("I11") && !sccores.Key.Contains("I12") && !sccores.Key.Contains("I13"))
                    {
                        evsTemp.Add(sccores.Key, sccores.Value);
                    }
                }

                modelData[MlbModelDataKeys.Evs] = evsTemp;
                Dictionary<string, double> tempDictionary = new Dictionary<string, double>();

                foreach (KeyValuePair<string, double> mrkt in modelData[MlbModelDataKeys.InMlf])
                {
                    tempDictionary.Add(mrkt.Key, mrkt.Value);
                }

                modelData[MlbModelDataKeys.InMlf] = ValidateMarketOdds(modelData[MlbModelDataKeys.InMlf]);
                LoadTables(tableIndicesDictionary, modelData);
                modelData[MlbModelDataKeys.InMlf] = tempDictionary;

                try
                {
                    bool live = false;

                    if (modelData.ContainsKey(MlbModelDataKeys.InLMlf))
                    {
                        bool arePeriodMarketsValid1 = false;
                        bool arePeriodMarketsValid3 = false;

                        if (modelData[MlbModelDataKeys.InLMlf].ContainsKey("TO,T"))
                        {
                            arePeriodMarketsValid1 = modelData[MlbModelDataKeys.InLMlf]["TO,T"].IsNotEqualToZero();
                        }

                        if (modelData[MlbModelDataKeys.InLMlf].ContainsKey("ML,S1"))
                        {
                            arePeriodMarketsValid3 =
                                modelData[MlbModelDataKeys.InLMlf]["ML,S1"].IsNotEqualToZero();
                        }

                        live = arePeriodMarketsValid1 || arePeriodMarketsValid3;
                    }
                    //todo fix started to be triggered on first score message
                    CalculateMarkets(periodMarkets, live, live);
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

        private static Dictionary<string, double> ValidateMarketOdds(Dictionary<string, double> marketDictionary)
        {
            // make sure that the model has at least the money and total for the period

            Dictionary<string, double> newMarketDictionary = Utils.CopyDictionary(marketDictionary);
            List<string> periodList = new List<string> { "I3", "I5", "I9" };

            foreach (string period in periodList)
            {
                string spreadKey = $"{period},ML,S1";
                string totalKey = $"{period},TO,S1";
                bool spreadSet = newMarketDictionary[spreadKey].IsEqualToZero();
                bool totalSet = newMarketDictionary[totalKey].IsEqualToZero();
                bool notSet = !(totalSet == false && spreadSet == false);

                //SEE IF THEY ARE SET IF NOT MAKE ALL ZERO
                if (notSet)
                {
                    newMarketDictionary[$"{period},ML,T"] = 0.0;
                    newMarketDictionary[$"{period},ML,S1"] = 0.0;
                    newMarketDictionary[$"{period},ML,S2"] = 0.0;
                    newMarketDictionary[$"{period},TO,T"] = 0.0;
                    newMarketDictionary[$"{period},TO,S1"] = 0.0;
                    newMarketDictionary[$"{period},TO,S2"] = 0.0;
                    newMarketDictionary[$"{period},SP,T"] = 0.0;
                    newMarketDictionary[$"{period},SP,S1"] = 0.0;
                    newMarketDictionary[$"{period},SP,S2"] = 0.0;
                }
            }

            return newMarketDictionary;
        }

        private static bool IsModelDataValid(IReadOnlyDictionary<string, Dictionary<string, double>> modelData)
        {
            if (!modelData.ContainsKey(MlbModelDataKeys.InMlf))
            {
                return false;
            }

            Dictionary<string, double> inMlf = modelData[MlbModelDataKeys.InMlf];

            if (inMlf.Count == 0)
            {
                return false;
            }

            // todo these conditions return false, so validation fails
            bool arePeriodMarketsValid1 = inMlf["I9,TO,T"].IsNotEqualToZero();
            bool arePeriodMarketsValid2 = inMlf["I9,SP,T"].IsNotEqualToZero();
            bool arePeriodMarketsValid3 = inMlf["I9,ML,S1"].IsNotEqualToZero();
            bool arePeriodMarketsValid = arePeriodMarketsValid1 || arePeriodMarketsValid2 || arePeriodMarketsValid3;

            return arePeriodMarketsValid;
        }

        private void CalculateMarkets(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            //clear again?
            // periodMarkets.Clear();
            GetMainLinesTwoWaysResults(periodMarkets, started, live);
            GetInningMainLinesTwoWaysResults(periodMarkets, started, live);

            GetMainLinesThreeWaysResults(periodMarkets, started, live);
            GetInningsMainLinesThreeWaysResults(periodMarkets, started, live);

            GetTeamTotalsTwoWaysResults(periodMarkets, started, live);
            GetInningTeamTotalsTwoWaysResults(periodMarkets, started, live);

            GetTeamTotalsThreeWaysResults(periodMarkets, started, live);
            GetInningTeamTotalsThreeWaysResults(periodMarkets, started, live);
        }

        private void GetMainLinesTwoWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            string tableName = live ? "Mgml2w" : "gml2w";
            string[] indexArray = { "GP", "altp", "iTGT", "iGL2" };
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

                    double value1 = GetArrayValue(array, gp, altp, 2, 1);
                    double value2 = GetArrayValue(array, gp, altp, 2, 2);
                    double value3 = GetArrayValue(array, gp, altp, 2, 3);

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
                        List<Market> marketSide = new List<Market>();
                        periodMarkets.Add(gp, marketSide);
                    }

                    market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{gp}{market.Tp}";
                    periodMarkets[gp].Add(market);

                    //------------------spread------------------------------------------
                    s1 = "A";
                    s2 = "H";

                    value1 = GetArrayValue(array, gp, altp, 1, 1);
                    value2 = GetArrayValue(array, gp, altp, 1, 2);
                    value3 = GetArrayValue(array, gp, altp, 1, 3);

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
                    market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{gp}{market.Tp}";

                    periodMarkets[gp].Add(market);
                }
            }
        }

        private void GetInningMainLinesTwoWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            const int inning = 4;
            string tableName = live ? "Miml2w" : "iml2w";
            string[] indexArray = { "iGP", "alti", "iTGT", "iGL2" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);
            int lowerBound1 = array.GetLowerBound(1);
            int upperBound1 = array.GetUpperBound(1);

            for (int period = lowerBound0; period <= upperBound0; period++)
            {
                for (int alt = lowerBound1; alt <= upperBound1; alt++)
                {
                    try
                    {
                        string s1 = "U";
                        string s2 = "O";

                        double value1 = GetArrayValue(array, period, alt, 2, 1);
                        double value2 = GetArrayValue(array, period, alt, 2, 2);
                        double value3 = GetArrayValue(array, period, alt, 2, 3);

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
                        int init = inning + period;

                        if (!periodMarkets.ContainsKey(init))
                        {
                            List<Market> marketSide = new List<Market>();
                            periodMarkets.Add(init, marketSide);
                        }

                        market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                        market.Key = $"{market.MarketRunnerList[0].Total}{init}{market.Tp}";

                        periodMarkets[init].Add(market);

                        s1 = "A";
                        s2 = "H";

                        value1 = GetArrayValue(array, period, alt, 1, 1);
                        value2 = GetArrayValue(array, period, alt, 1, 2);
                        value3 = GetArrayValue(array, period, alt, 1, 3);

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

                        decimal test = ToDecimal(market.MarketRunnerList[0].Total);
                        int count = BitConverter.GetBytes(decimal.GetBits(test)[3])[2];

                        market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                        market.Key = $"{market.MarketRunnerList[0].Total}{init}{market.Tp}";
                        periodMarkets[init].Add(market);
                    }
                    catch (Exception exception)
                    {
                        Logger.Error(exception);
                    }
                }
            }
        }

        private void GetMainLinesThreeWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            string tableName = live ? "MGML3W" : "GML3W";
            string[] indexArray = { "GP", "altp", "iTGT", "iGL3" };
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

                    double value1 = GetArrayValue(array, period, alt, 2, 1);
                    double value2 = GetArrayValue(array, period, alt, 2, 2);
                    double value3 = GetArrayValue(array, period, alt, 2, 3);
                    double value4 = GetArrayValue(array, period, alt, 2, 4);

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
                        Side = s3
                    };
                    market.MarketRunnerList.Add(marketRunner);

                    if (!periodMarkets.ContainsKey(period))
                    {
                        List<Market> marketSide = new List<Market>();
                        periodMarkets.Add(period, marketSide);
                    }

                    if (value4.IsNotEqualToZero())
                    {
                        market = OddsConverter.FinishedOdds(market, started, _threeWayVig, _threeWayVig);
                        market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";

                        periodMarkets[period].Add(market);
                    }

                    // spreads

                    s1 = "A";
                    s2 = "H";

                    value1 = GetArrayValue(array, period, alt, 1, 1);
                    value2 = GetArrayValue(array, period, alt, 1, 2);
                    value3 = GetArrayValue(array, period, alt, 1, 3);
                    value4 = GetArrayValue(array, period, alt, 1, 4);

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

                    if (value4.IsNotEqualToZero())
                    {
                        market = OddsConverter.FinishedOdds(market, started, _threeWayVig, _threeWayVig);
                        market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";
                        periodMarkets[period].Add(market);
                    }
                }
            }
        }

        private void GetInningsMainLinesThreeWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            const int inning = 4;
            string tableName = live ? "MIML3W" : "IML3W";
            string[] indexArray = { "iGP", "alti", "iTGT", "iGL3" };
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

                    double value1 = GetArrayValue(array, period, alt, 2, 1);
                    double value2 = GetArrayValue(array, period, alt, 2, 2);
                    double value3 = GetArrayValue(array, period, alt, 2, 3);
                    double value4 = GetArrayValue(array, period, alt, 2, 4);

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

                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = ToDouble(value1),
                        Price = ToDouble(value4),
                        Side = s2
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    int init = inning + period;

                    if (!periodMarkets.ContainsKey(init))
                    {
                        List<Market> marketSide = new List<Market>();
                        periodMarkets.Add(init, marketSide);
                    }

                    if (value4.IsNotEqualToZero())
                    {
                        market = OddsConverter.FinishedOdds(market, started, _threeWayVig, _threeWayVig);
                        market.Key = $"{market.MarketRunnerList[0].Total}{init}{market.Tp}";
                        periodMarkets[init].Add(market);
                    }

                    s1 = "A";
                    s2 = "H";

                    value1 = GetArrayValue(array, period, alt, 1, 1);
                    value2 = GetArrayValue(array, period, alt, 1, 2);
                    value3 = GetArrayValue(array, period, alt, 1, 3);
                    value4 = GetArrayValue(array, period, alt, 1, 4);

                    marketRunner = new MarketRunner
                    {
                        Total = ToDouble(value1),
                        Price = ToDouble(value2),
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
                        Total = -ToDouble(value1),
                        Price = ToDouble(value3),
                        Side = s2
                    };
                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = ToDouble(value1),
                        Price = ToDouble(value4),
                        Side = s3
                    };
                    market.MarketRunnerList.Add(marketRunner);

                    if (value4.IsNotEqualToZero())
                    {
                        market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                        market.Key = $"{market.MarketRunnerList[0].Total}{init}{market.Tp}";
                        periodMarkets[init].Add(market);
                    }
                }
            }
        }

        private void GetTeamTotalsTwoWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            string tableName = live ? "Mgtt2w" : "gtt2w";
            string[] indexArray = { "GP", "TTRI", "iTVH", "iGL2" };
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

                    double value1 = GetArrayValue(array, period, alt, 1, 1);
                    double value2 = GetArrayValue(array, period, alt, 1, 2);
                    double value3 = GetArrayValue(array, period, alt, 1, 3);

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
                        List<Market> marketSide = new List<Market>();
                        periodMarkets.Add(period, marketSide);
                    }

                    market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";
                    periodMarkets[period].Add(market);

                    value1 = GetArrayValue(array, period, alt, 2, 1);
                    value2 = GetArrayValue(array, period, alt, 2, 2);
                    value3 = GetArrayValue(array, period, alt, 2, 3);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value2,
                        Side = s1
                    };

                    market = new Market
                    {
                        Tp = "HTTL",
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
                        List<Market> marketSide = new List<Market>();
                        periodMarkets.Add(period, marketSide);
                    }

                    market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";
                    periodMarkets[period].Add(market);
                }
            }
        }

        private void GetInningTeamTotalsTwoWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            const int inning = 4;
            string tableName = live ? "Mitt2w" : "itt2w";
            string[] indexArray = { "iGP", "TTRI", "iTVH", "iGL2" };
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

                    double value1 = GetArrayValue(array, period, alt, 1, 1);
                    double value2 = GetArrayValue(array, period, alt, 1, 2);
                    double value3 = GetArrayValue(array, period, alt, 1, 3);

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
                    int init = inning + period;

                    if (!periodMarkets.ContainsKey(init))
                    {
                        List<Market> marketSide = new List<Market>();
                        periodMarkets.Add(init, marketSide);
                    }
                    market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{init}{market.Tp}";

                    periodMarkets[init].Add(market);

                    value1 = GetArrayValue(array, period, alt, 2, 1);
                    value2 = GetArrayValue(array, period, alt, 2, 2);
                    value3 = GetArrayValue(array, period, alt, 2, 3);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value2,
                        Side = s1
                    };

                    market = new Market
                    {
                        Tp = "HTTL",
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

                    init = inning + period;
                    if (!periodMarkets.ContainsKey(init))
                    {
                        List<Market> marketSide = new List<Market>();
                        periodMarkets.Add(init, marketSide);
                    }

                    //  if (market.MarketRunnerList[0].P > 0)
                    //   {
                    market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{init}{market.Tp}";

                    periodMarkets[init].Add(market);
                    //   }
                }
            }
        }

        private void GetTeamTotalsThreeWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            string tableName = live ? "MGTT3W" : "GTT3W";
            string[] indexArray = { "GP", "TTRI", "iTVH", "iGL3" };
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

                    double value1 = GetArrayValue(array, period, alt, 1, 1);
                    double value2 = GetArrayValue(array, period, alt, 1, 2);
                    double value4 = GetArrayValue(array, period, alt, 1, 4);

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
                        List<Market> marketSide = new List<Market>();
                        periodMarkets.Add(period, marketSide);
                    }

                    //if (!market.MarketRunnerList[2].Price.IsEqualToZero())

                    // if (value4.IsNotEqualToZero())
                    // {
                    market = OddsConverter.FinishedOdds(market, started, _threeWayVig, _threeWayVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";
                    periodMarkets[period].Add(market);
                    //}

                    market = new Market { Tp = "HTTL3W" };

                    value1 = GetArrayValue(array, period, alt, 2, 1);
                    value2 = GetArrayValue(array, period, alt, 2, 2);
                    double value3 = GetArrayValue(array, period, alt, 2, 3);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value2,
                        Side = s1
                    };

                    market.Target = marketRunner.Total;
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
                        List<Market> marketSide = new List<Market>();
                        periodMarkets.Add(period, marketSide);
                    }

                    // if (value4.IsNotEqualToZero())
                    // {
                    market = OddsConverter.FinishedOdds(market, started, _threeWayVig, _threeWayVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";

                    periodMarkets[period].Add(market);
                    //}
                }
            }
        }

        private void GetInningTeamTotalsThreeWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            const int inning = 4;
            string tableName = live ? "MITT3W" : "ITT3W";
            string[] indexArray = { "iGP", "TTRI", "iTVH", "iGL3" };
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

                    double value1 = GetArrayValue(array, period, alt, 1, 1);
                    double value2 = GetArrayValue(array, period, alt, 1, 2);
                    double value3 = GetArrayValue(array, period, alt, 1, 3);
                    double value4 = GetArrayValue(array, period, alt, 1, 4);

                    // O
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

                    // U
                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value3,
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

                    int init = inning + period;

                    if (!periodMarkets.ContainsKey(init))
                    {
                        List<Market> marketSide = new List<Market>();
                        periodMarkets.Add(init, marketSide);
                    }

                    //   if (value4.IsNotEqualToZero())
                    //   {
                    market = OddsConverter.FinishedOdds(market, started, _threeWayVig, _threeWayVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{init}{market.Tp}";

                    periodMarkets[init].Add(market);
                    //   }


                    market = new Market { Tp = "HTTL3W" };

                    value1 = GetArrayValue(array, period, alt, 2, 1);
                    value2 = GetArrayValue(array, period, alt, 2, 2);
                    value3 = GetArrayValue(array, period, alt, 2, 3);
                    value4 = GetArrayValue(array, period, alt, 2, 4);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value2,
                        Side = s1
                    };

                    market.Target = marketRunner.Total;
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

                    init = inning + period;

                    if (!periodMarkets.ContainsKey(init))
                    {
                        List<Market> marketSide = new List<Market>();
                        periodMarkets.Add(init, marketSide);
                    }

                    //    if (value4.IsNotEqualToZero())
                    //    {
                    market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{init}{market.Tp}";

                    periodMarkets[init].Add(market);
                    //   }
                }
            }
        }

        #endregion
    }
}
