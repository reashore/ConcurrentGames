using System;
using System.Collections.Generic;
using System.Configuration;
using ADE;
using SportsIq.Models.Constants.Wnba;
using SportsIq.Models.Markets;
using SportsIq.Utilities;
using static System.Convert;

namespace SportsIq.Analytica.Wnba
{
    public interface IAnalyticaWnba
    {
        bool IsTeamMode { get; set; }
        Dictionary<int, List<Market>> RunModel(Dictionary<string, Dictionary<string, double>> modelData, Dictionary<int, List<Market>> periodMarkets, bool started, string saveFileName);
    }

    public class AnalyticaWnba : AnalyticaBase, IAnalyticaWnba
    {
        private readonly double _preVig;
        private readonly double _liveVig;
        private readonly int _iSw;
        private readonly int _iAdjT;
        private readonly int _iAdjS;
        private bool _followLive;
        private bool _live;

        public AnalyticaWnba()
        {
            _preVig = ToDouble(ConfigurationManager.AppSettings["preVig"]);
            _liveVig = ToDouble(ConfigurationManager.AppSettings["liveVig"]);

            //to be set by the controls
            _iSw = 1;
            _iAdjS = 10;
            _iAdjT = 10;
            _followLive = true;
            _live = false;
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

        private Dictionary<int, List<Market>> RunPlayerModel(Dictionary<string, Dictionary<string, double>> modelData, Dictionary<int, List<Market>> periodMarkets, bool started, string saveFileName)
        {
            if (!IsModelDataValid(modelData))
            {
                Logger.Error("RunPlayerModel(): ModelData is invalid");
                return periodMarkets;
            }

            try
            {
                Dictionary<string, string[]> tableIndicesDictionary = new Dictionary<string, string[]>
                    {
                        {WnbaModelDataKeys.InMlf, new[] {"iGP", "iBL", "iGL2"}},                                // Market Odds
                        //{WnbaModelDataKeys.Evs, new[] {"iTVH", "iGP"}},                                       // Game State
                        //{WnbaModelDataKeys.Egt, new[] {"iGT"}},                                               // Score
                        {WnbaModelDataKeys.Gsc,   new[] { "iTVH", "iMTR", "iQT", "iPLY" }},                     // Game score
                        {WnbaModelDataKeys.Ttm,   new[] { "iTVH", "iGP", "iMTR" }},                             // Team Totals by Metric 
                        {WnbaModelDataKeys.Posc,  new[] { "iTVH", "iMTR", "iQT", "iGT", "iPLY", "iTS" }},       // Standard Deviation (player) vs Team by Metric
                        {WnbaModelDataKeys.Potm,  new[] { "iTVH", "iMTR", "iQT", "iGT", "iPLY", "iTS" }},       // Standard Deviation (player) vs Team by Metric
                        {WnbaModelDataKeys.Pop,   new[] { "iTVH", "iMTR", "iQT", "iGT", "iPLY", "iTS" }},       // Standard Deviation (player) vs Team by Metric
                        {WnbaModelDataKeys.Psco,  new[] { "iTVH", "iMTR", "iQT", "iGT", "iPLY", "iTS" }},       // Standard Deviation (player) vs Team by Metric
                        {WnbaModelDataKeys.Sdvtm, new[] { "iTVH", "iMTR", "iPLY" }},                            // Standard Deviation (player) vs Team by Metric
                        {WnbaModelDataKeys.Sdom,  new[] { "iTVH", "iMTR", "iPLY" }}                             // Standard Deviation (player) vs Team by Metric
                    };

                LoadTables(tableIndicesDictionary, modelData);
                CalculateMarkets(periodMarkets, started, _live);
                SaveModel(saveFileName);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }

            return periodMarkets;
        }

        private Dictionary<int, List<Market>> RunTeamModel(Dictionary<string, Dictionary<string, double>> modelData, Dictionary<int, List<Market>> periodMarkets, bool started, string saveFileName)
        {
            if (!IsModelDataValid(modelData))
            {
                return periodMarkets;
            }

            /*settings
             {"MSR":50,"SCW":50,"FOL":true}
             */
            if (!modelData[WnbaModelDataKeys.Settings].ContainsKey("MSR"))
            {
                modelData[WnbaModelDataKeys.Settings]["MSR"] = 0.0;
            }

            if (!modelData[WnbaModelDataKeys.Settings].ContainsKey("SCW"))
            {
                modelData[WnbaModelDataKeys.Settings]["SCW"] = 0.0;
            }

            if (!modelData[WnbaModelDataKeys.Settings].ContainsKey("FOL"))
            {
                modelData[WnbaModelDataKeys.Settings]["FOL"] = 1;
            }

            Dictionary<string, double> tempSettings = new Dictionary<string, double>
            {
                {"MSR", modelData[WnbaModelDataKeys.Settings]["MSR"]},
                {"SCW", modelData[WnbaModelDataKeys.Settings]["SCW"]}
            };
            UpdateSettings(tempSettings);

            _followLive = modelData[WnbaModelDataKeys.Settings]["FOL"].IsEqualTo(1);

            try
            {
                Dictionary<string, string[]> tableIndicesDictionary = new Dictionary<string, string[]>
                    {
                        {WnbaModelDataKeys.InMlf, new[] {"iGP", "iBL", "iGL2"}},            //good
                        {WnbaModelDataKeys.InLMlf, new[] {"iGP", "iBL", "iGL2"}},           //good
                        {WnbaModelDataKeys.InSc,  new[] {"iTVH", "iQT", "iGT"}},            //
                        {WnbaModelDataKeys.InLsF,  new[] {"iQT", "iGT"}},
                        {WnbaModelDataKeys.InTss, new[] {"iTVH", "iAR", "iSHS" }},
                        {WnbaModelDataKeys.InTsf, new[] {"iTVH", "iQT", "iSAS",  "iGT"}},
                        {WnbaModelDataKeys.Evs, new[] {"iTVH", "iGP"}},                     // Game State
                        {WnbaModelDataKeys.Egt, new[] {"iGS"}}
                    };

                Dictionary<string, double> tempDictionary = Utils.CopyDictionary(modelData[WnbaModelDataKeys.InMlf]);
                modelData[WnbaModelDataKeys.InMlf] = ValidateMarketOdds(modelData[WnbaModelDataKeys.InMlf]);
                LoadTables(tableIndicesDictionary, modelData);
                modelData[WnbaModelDataKeys.InMlf] = tempDictionary;

                try
                {

                    if (modelData.ContainsKey(WnbaModelDataKeys.InLMlf))
                    {
                        bool arePeriodMarketsValid2 = modelData[WnbaModelDataKeys.InLMlf]["CG,SP,S1"].IsNotEqualToZero();
                        bool arePeriodMarketsValid3 = modelData[WnbaModelDataKeys.InLMlf]["CG,ML,S1"].IsNotEqualToZero();
                        bool arePeriodMarketsValid1 = modelData[WnbaModelDataKeys.InLMlf]["CG,TO,T"].IsNotEqualToZero();

                        _live = arePeriodMarketsValid1 && arePeriodMarketsValid2 && arePeriodMarketsValid3 && _followLive;
                    }

                    //CalculateMarkets(periodMarkets, started, live);
                }
                catch (Exception exception)
                {
                    Logger.Error(exception);
                }

                //Stopwatch stopwatch = Stopwatch.StartNew();
                CalculateMarkets(periodMarkets, started, _live);
                //Logger.Info($"CalculateMarkets() runtime = {stopwatch.Elapsed.Seconds,4} sec");

                SaveModel(saveFileName);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }

            return periodMarkets;
        }

        /* set the single value objects */
        private void UpdateSettings(Dictionary<string, double> settings)
        {
            foreach (KeyValuePair<string, double> setting in settings)
            {
                SetDescription(setting.Key, setting.Value);
            }
        }

        /* make sure that the model has at least the money and total for the period */
        private static Dictionary<string, double> ValidateMarketOdds(Dictionary<string, double> marketDictionary)
        {
            // make sure that the model has at least the money and total for the period 

            Dictionary<string, double> newMarketDictionary = Utils.CopyDictionary(marketDictionary);
            List<string> periodList = new List<string> { "H1", "Q1", "CG" };

            foreach (string period in periodList)
            {
                string spreadKey = $"{period},SP,S1";
                string totalKey = $"{period},TO,S1";
                string moneyKey = $"{period},ML,S1";
                bool spreadSet = newMarketDictionary[spreadKey].IsEqualToZero();
                bool totalSet = newMarketDictionary[totalKey].IsEqualToZero();
                bool moneySet = newMarketDictionary[moneyKey].IsEqualToZero();

                bool notSet = !(totalSet == false && spreadSet == false && moneySet == false);

                //bool notSet = true;
                //if (totalSet == false && spreadSet == false && moneySet == false)
                //{
                //    notSet = false;
                //}
                //spreadSet && totalSet ? false : true;

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
            if (!modelData.ContainsKey(WnbaModelDataKeys.InMlf))
            {
                return false;
            }

            Dictionary<string, double> inMlf = modelData[WnbaModelDataKeys.InMlf];

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

        #region Calculate Markets

        private void CalculateMarkets(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            if (live)
            {
                GetMainLinesTwoWaysResultsLive(periodMarkets, started, live);
            }
            else
            {
                GetMainLinesTwoWaysResults(periodMarkets, started, live);
            }

            if (live)
            {
                GetMoneyTwoWaysResultsLive(periodMarkets, started, live);
            }
            else
            {
                GetMoneyTwoWaysResults(periodMarkets, started, live);
            }

            GetMoneyThreeWaysResults(periodMarkets, started);

            GetMainLinesThreeWaysResults(periodMarkets, started);

            GetTeamTotalsTwoWaysResults(periodMarkets, started);

            GetTeamTotalsThreeWaysResults(periodMarkets, started);

            GetTeamWmRegularTimeResults(periodMarkets, started);

            GetTeamWMCGResults(periodMarkets, started);

            GetFixedRangeResults(periodMarkets, started);
            //  if (live)
            //  {
            //     GetMoneyTotalResultsLive(periodMarkets, started);
            //  }
            //  else
            //  {


            GetMoneyTotalResults(periodMarkets, started);
            //}

            //  GetSpreadTotalResults(periodMarkets, started);
        }

        private void GetMainLinesTwoWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            string liveNotLive = live == false ? "ML2" : "SML";
            string tableName = liveNotLive;
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


        private void GetMainLinesTwoWaysResultsLive(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            //string liveNotLive = (live == false) ? "ML2" : "SML";
            const string tableName = "SML";
            string[] indexArray = { "iGP", "iALT", "iTGT", "iGL2", "iAdjT", "iAdjS" };
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

                    double value1 = GetArrayValue(array, gp, altp, 2, 1, _iAdjS, _iAdjT);
                    double value2 = GetArrayValue(array, gp, altp, 2, 2, _iAdjS, _iAdjT);
                    double value3 = GetArrayValue(array, gp, altp, 2, 3, _iAdjS, _iAdjT);

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

                    value1 = GetArrayValue(array, gp, altp, 1, 1, _iAdjS, _iAdjT);
                    value2 = GetArrayValue(array, gp, altp, 1, 2, _iAdjS, _iAdjT);
                    value3 = GetArrayValue(array, gp, altp, 1, 3, _iAdjS, _iAdjT);

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

        private void GetMoneyTwoWaysResultsLive(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            //string liveNotLive = live == false ? "MNL2" : "Shml";
            const string tableName = "Shml";
            //  const string tableName = "MNL2";
            string[] indexArray = { "iGP", "iGL2", "iAdjT", "iAdjS" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);

            for (int gp = lowerBound0; gp <= upperBound0; gp++)
            {
                const string s1 = "A";
                const string s2 = "H";

                double value1 = GetArrayValue(array, gp, 1, _iAdjS, _iAdjT);
                double value2 = GetArrayValue(array, gp, 2, _iAdjS, _iAdjT);
                double value3 = GetArrayValue(array, gp, 3, _iAdjS, _iAdjT);

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

        private void GetMoneyTwoWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            //string liveNotLive = live == false ? "MNL2" : "Shml";
            const string tableName = "MNL2";
            //  const string tableName = "MNL2";
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

        // todo cannot rename because of identifier conflict
        private void GetTeamWMCGResults(IDictionary<int, List<Market>> periodMarkets, bool started)
        {
            const string tableName = "Winning_Margin_Compl";

            string[] indexArray = { "iGP", "iWM", "iSW", "iAdjT", "iAdjS" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);

            //int[] stepsStr = { -19, -18, -15, -12, -9, -6, -3, 0, 3, 6, 9, 12, 15, 18, 19 };
            int[] steps = { -19, -18, -15, -12, -9, -6, -3, 0, 3, 6, 9, 12, 15, 18, 19 };

            for (int gp = lowerBound0; gp <= upperBound0; gp++)
            {
                int wm = 1;
                Market market = new Market
                {
                    Tp = "WMCG",
                    Target = wm
                };

                foreach (int step in steps)
                {
                    string description;

                    if (step < 0 && step > -19)
                    {
                        description = "Away BY " + Math.Abs(step);
                    }
                    else if (step == 0)
                    {
                        description = "x";
                        //run.s = "D";
                    }
                    else if (step == -19)
                    {
                        description = "Away BY ANY OTHER SCORE";
                        //run.s = "V";
                    }
                    else if (step == 19)
                    {
                        description = "Home BY ANY OTHER SCORE";
                        //run.s = "H";
                    }
                    else
                    {
                        description = "Home BY " + step;
                    }

                    double value = GetArrayValue(array, gp, wm, _iSw, _iAdjS, _iAdjT);

                    MarketRunner marketRunner = new MarketRunner
                    {
                        Total = step,
                        Price = value,
                        Side = description
                    };
                    market.MarketRunnerList.Add(marketRunner);

                    if (!periodMarkets.ContainsKey(gp))
                    {
                        List<Market> markets = new List<Market>();
                        periodMarkets.Add(gp, markets);
                    }

                    wm++;
                }

                market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                market.Key = $"{market.MarketRunnerList[0].Total}{gp}{market.Tp}";
                periodMarkets[gp].Add(market);
            }
        }

        private void GetTeamWmRegularTimeResults(IDictionary<int, List<Market>> periodMarkets, bool started)
        {
            const string tableName = "oWM48";
            string[] indexArray = { "iGP", "iWM", "iSW", "iAdjT", "iAdjS" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);

            //int[] stepsStr = { -19, -18, -15, -12, -9, -6, -3, 0, 3, 6, 9, 12, 15, 18, 19 };
            int[] steps = { -19, -18, -15, -12, -9, -6, -3, 0, 3, 6, 9, 12, 15, 18, 19 };

            for (int gp = lowerBound0; gp <= upperBound0; gp++)
            {


                int wm = 1;
                Market market = new Market
                {
                    Tp = "WMR",
                    Target = wm
                };

                foreach (int step in steps)
                {
                    string description;

                    if (step < 0 && step > -19)
                    {
                        description = "Away BY " + Math.Abs(step);
                    }
                    else if (step == 0)
                    {
                        description = "x";
                        //run.s = "D";
                    }
                    else if (step == -19)
                    {
                        description = "Away BY ANY OTHER SCORE";
                        //run.s = "V";
                    }
                    else if (step == 19)
                    {
                        description = "Home BY ANY OTHER SCORE";
                        //run.s = "H";
                    }
                    else
                    {
                        description = "Home BY " + step;
                    }

                    double value = GetArrayValue(array, gp, wm, _iSw, _iAdjS, _iAdjT);

                    MarketRunner marketRunner = new MarketRunner
                    {
                        Total = step,
                        Price = value,
                        Side = description
                    };
                    market.MarketRunnerList.Add(marketRunner);

                    if (!periodMarkets.ContainsKey(gp))
                    {
                        List<Market> markets = new List<Market>();
                        periodMarkets.Add(gp, markets);
                    }

                    wm++;
                }

                market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                market.Key = $"{market.MarketRunnerList[0].Total}{gp}{market.Tp}";
                periodMarkets[gp].Add(market);
            }
        }

        private void GetTeamWmcgResults(IDictionary<int, List<Market>> periodMarkets, bool started)
        {
            const string tableName = "Winning_Margin_Compl";
            string[] indexArray = { "iGP", "iWM", "iSW", "iAdjT", "iAdjS" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);

            //int[] stepsStr = { -19, -18, -15, -12, -9, -6, -3, 0, 3, 6, 9, 12, 15, 18, 19 };
            List<int> steps = new List<int> { -19, -18, -15, -12, -9, -6, -3, 0, 3, 6, 9, 12, 15, 18, 19 };

            for (int gp = lowerBound0; gp <= upperBound0; gp++)
            {
                int wm = 1;
                Market market = new Market
                {
                    Tp = "WMCG",
                    Target = wm
                };

                foreach (int step in steps)
                {

                    string description;

                    if (step < 0 && step > -19)
                    {
                        description = "Away BY " + Math.Abs(step);
                    }
                    else if (step == 0)
                    {
                        description = "x";
                        //run.s = "D";
                    }
                    else if (step == -19)
                    {
                        description = "Away BY ANY OTHER SCORE";
                        //run.s = "V";
                    }
                    else if (step == 19)
                    {
                        description = "Home BY ANY OTHER SCORE";
                        //run.s = "H";
                    }
                    else
                    {
                        description = "Home BY " + step;
                    }

                    double value = GetArrayValue(array, gp, wm, _iSw, _iAdjS, _iAdjT);

                    MarketRunner marketRunner = new MarketRunner
                    {
                        Total = step,
                        Price = value,
                        Side = description
                    };
                    market.MarketRunnerList.Add(marketRunner);

                    if (!periodMarkets.ContainsKey(gp))
                    {
                        List<Market> markets = new List<Market>();
                        periodMarkets.Add(gp, markets);
                    }

                    wm++;
                }

                market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                market.Key = $"{market.MarketRunnerList[0].Total}{gp}{market.Tp}";
                periodMarkets[gp].Add(market);
            }
        }

        private void GetFixedRangeResults(IDictionary<int, List<Market>> periodMarkets, bool started)
        {
            const string tableName = "ORng";

            string[] indexArray = { "iGP", "iRNG", "iTGT", "iGL3", "iSW", "iAdjT", "iAdjS" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);
            int lowerBound1 = array.GetLowerBound(1);
            int upperBound1 = array.GetUpperBound(1);

            for (int gp = lowerBound0; gp <= upperBound0; gp++)
            {
                int range = 3;
                for (int rng = lowerBound1; rng <= upperBound1; rng++)
                {
                    /*************SPREAD RANGE ******************/
                    Market market = new Market
                    {
                        Tp = "SRNG",
                        Target = range
                    };
                    double value1 = GetArrayValue(array, gp, rng, 1, 1, _iSw, _iAdjS, _iAdjT);
                    double value2 = GetArrayValue(array, gp, rng, 1, 2, _iSw, _iAdjS, _iAdjT);
                    double value3 = GetArrayValue(array, gp, rng, 1, 3, _iSw, _iAdjS, _iAdjT);


                    MarketRunner marketRunner = new MarketRunner
                    {
                        Total = range,
                        Price = value1,
                        Side = "A"
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    MarketRunner marketRunner2 = new MarketRunner
                    {
                        Total = range,
                        Price = value2,
                        Side = "H"
                    };

                    market.MarketRunnerList.Add(marketRunner2);

                    MarketRunner marketRunner3 = new MarketRunner
                    {
                        Total = range,
                        Price = value3,
                        Side = "X"
                    };

                    market.MarketRunnerList.Add(marketRunner3);

                    market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{gp}{market.Tp}";
                    periodMarkets[gp].Add(market);

                    /*************TOTAL RANGE ******************/
                    market = new Market
                    {
                        Tp = "TRNG",
                        Target = range
                    };

                    value1 = GetArrayValue(array, gp, rng, 2, 1, _iSw, _iAdjS, _iAdjT);
                    value2 = GetArrayValue(array, gp, rng, 2, 2, _iSw, _iAdjS, _iAdjT);
                    value3 = GetArrayValue(array, gp, rng, 2, 3, _iSw, _iAdjS, _iAdjT);

                    marketRunner = new MarketRunner
                    {
                        Total = range,
                        Price = value1,
                        Side = "O"
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner2 = new MarketRunner
                    {
                        Total = range,
                        Price = value2,
                        Side = "U"
                    };

                    market.MarketRunnerList.Add(marketRunner2);

                    marketRunner3 = new MarketRunner
                    {
                        Total = range,
                        Price = value3,
                        Side = "X"
                    };

                    market.MarketRunnerList.Add(marketRunner3);

                    market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                    market.Key = $"{market.MarketRunnerList[0].Total}{gp}{market.Tp}";
                    periodMarkets[gp].Add(market);

                    range += 3;
                }
            }
        }

        /*SPREAD TOTAL*/
        private void GetMoneyTotalResults(IDictionary<int, List<Market>> periodMarkets, bool started)
        {
            const string tableName = "CM6";
            string[] indexArray = { "iGP", "iCMTA", "iCM6", "iSW", "iAdjT", "iAdjS" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);
            int lowerBound1 = array.GetLowerBound(1);
            int upperBound1 = array.GetUpperBound(1);

            for (int period = lowerBound0; period <= upperBound0; period++)
            {
                for (int alt = lowerBound1; alt <= upperBound1; alt++)
                {

                    const string s1 = "ao";
                    const string s2 = "au";
                    const string s3 = "h_o";
                    const string s4 = "h_u";
                    const string s5 = "a_d"; //away & draw
                    const string s6 = "h_d";

                    double value1 = GetArrayValue(array, period, alt, 1, _iSw, _iAdjS, _iAdjT);
                    double value2 = GetArrayValue(array, period, alt, 2, _iSw, _iAdjS, _iAdjT);
                    double value3 = GetArrayValue(array, period, alt, 3, _iSw, _iAdjS, _iAdjT);
                    double value4 = GetArrayValue(array, period, alt, 4, _iSw, _iAdjS, _iAdjT);
                    double value5 = GetArrayValue(array, period, alt, 5, _iSw, _iAdjS, _iAdjT);
                    double value6 = GetArrayValue(array, period, alt, 6, _iSw, _iAdjS, _iAdjT);
                    double value7 = GetArrayValue(array, period, alt, 7, _iSw, _iAdjS, _iAdjT);

                    MarketRunner marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value2,
                        Side = s1
                    };

                    Market market = new Market
                    {
                        Tp = "C6W",
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

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value5,
                        Side = s4
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value6,
                        Side = s5
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value7,
                        Side = s6
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    if (!periodMarkets.ContainsKey(period))
                    {
                        List<Market> markets = new List<Market>();
                        periodMarkets.Add(period, markets);
                    }

                    if (market.MarketRunnerList[0].Price > 0)
                    {
                        market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                        market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";

                        periodMarkets[period].Add(market);
                    }
                }
            }
        }

        private void GetMoneyTotalResultsLive(IDictionary<int, List<Market>> periodMarkets, bool started)
        {
            const string tableName = "CM6";
            string[] indexArray = { "iGP", "iCMTA", "iCM6", "iSW", "iAdjT", "iAdjS" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);
            int lowerBound1 = array.GetLowerBound(1);
            int upperBound1 = array.GetUpperBound(1);

            for (int period = lowerBound0; period <= upperBound0; period++)
            {
                for (int alt = lowerBound1; alt <= upperBound1; alt++)
                {

                    const string s1 = "ao";
                    const string s2 = "au";
                    const string s3 = "h_o";
                    const string s4 = "h_u";
                    const string s5 = "a_d"; //away & draw
                    const string s6 = "h_d";

                    double value1 = GetArrayValue(array, period, alt, 1, _iSw, _iAdjS, _iAdjT);
                    double value2 = GetArrayValue(array, period, alt, 2, _iSw, _iAdjS, _iAdjT);
                    double value3 = GetArrayValue(array, period, alt, 3, _iSw, _iAdjS, _iAdjT);
                    double value4 = GetArrayValue(array, period, alt, 4, _iSw, _iAdjS, _iAdjT);
                    double value5 = GetArrayValue(array, period, alt, 5, _iSw, _iAdjS, _iAdjT);
                    double value6 = GetArrayValue(array, period, alt, 6, _iSw, _iAdjS, _iAdjT);
                    double value7 = GetArrayValue(array, period, alt, 7, _iSw, _iAdjS, _iAdjT);

                    MarketRunner marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value2,
                        Side = s1
                    };

                    Market market = new Market
                    {
                        Tp = "C6W",
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

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value5,
                        Side = s4
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value6,
                        Side = s5
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Price = value7,
                        Side = s6
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    if (!periodMarkets.ContainsKey(period))
                    {
                        List<Market> markets = new List<Market>();
                        periodMarkets.Add(period, markets);
                    }

                    if (market.MarketRunnerList[0].Price > 0)
                    {
                        market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                        market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";

                        periodMarkets[period].Add(market);
                    }
                }
            }
        }


        private void GetSpreadTotalResults(IDictionary<int, List<Market>> periodMarkets, bool started)
        {
            const string tableName = "CM4";
            string[] indexArray = { "iGP", "iCMTA", "iCM4", "iSW", "iAdjT", "iAdjS" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);
            int lowerBound1 = array.GetLowerBound(1);
            int upperBound1 = array.GetUpperBound(1);

            for (int period = lowerBound0; period <= upperBound0; period++)
            {
                for (int alt = lowerBound1; alt <= upperBound1; alt++)
                {
                    const string s1 = "a_o";
                    const string s2 = "a_u";
                    const string s3 = "h_o";
                    const string s4 = "h_u";

                    double value1 = GetArrayValue(array, period, alt, 1, _iSw, _iAdjS, _iAdjT);
                    double value2 = GetArrayValue(array, period, alt, 2, _iSw, _iAdjS, _iAdjT);
                    double value3 = GetArrayValue(array, period, alt, 3, _iSw, _iAdjS, _iAdjT);
                    double value4 = GetArrayValue(array, period, alt, 4, _iSw, _iAdjS, _iAdjT);
                    double value5 = GetArrayValue(array, period, alt, 5, _iSw, _iAdjS, _iAdjT);
                    double value6 = GetArrayValue(array, period, alt, 6, _iSw, _iAdjS, _iAdjT);

                    MarketRunner marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Handicap = value2,
                        Price = value3,
                        Side = s1
                    };

                    Market market = new Market
                    {
                        Tp = "C4W",
                        Target = marketRunner.Total
                    };

                    market.MarketRunnerList.Add(marketRunner);


                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Handicap = value2,
                        Price = value4,
                        Side = s2
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Handicap = value2,
                        Price = value5,
                        Side = s3
                    };

                    market.MarketRunnerList.Add(marketRunner);

                    marketRunner = new MarketRunner
                    {
                        Total = value1,
                        Handicap = value2,
                        Price = value6,
                        Side = s4
                    };

                    market.MarketRunnerList.Add(marketRunner);


                    if (!periodMarkets.ContainsKey(period))
                    {
                        List<Market> markets = new List<Market>();
                        periodMarkets.Add(period, markets);
                    }

                    if (market.MarketRunnerList[0].Price > 0)
                    {
                        market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                        market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";

                        periodMarkets[period].Add(market);
                    }

                }
            }
        }

        /* END SPREAD TOTAL*/

        #endregion

        #endregion

        #region Experimental Code

        public Dictionary<string, List<string>> GetAllTableIndices()
        {
            List<string> tableNameList = new List<string>
            {
                WnbaModelDataKeys.InMlf,
                WnbaModelDataKeys.Gsc,
                WnbaModelDataKeys.Ttm,
                WnbaModelDataKeys.Posc,
                WnbaModelDataKeys.Potm,
                WnbaModelDataKeys.Pop,
                WnbaModelDataKeys.Psco,
                WnbaModelDataKeys.Sdvtm,
                WnbaModelDataKeys.Sdom
            };

            Dictionary<string, List<string>> tableIndicesDictionary = new Dictionary<string, List<string>>();

            foreach (string tableName in tableNameList)
            {
                List<string> tableIndexList = GetTableIndices(tableName);
                tableIndicesDictionary[tableName] = tableIndexList;
            }

            return tableIndicesDictionary;
        }

        private List<string> GetTableIndices(string tableName)
        {
            CATable caTable = GetDefTable(tableName);
            List<string> tableIndexNames = new List<string>();

            foreach (object indexName in (Array)(object)caTable.IndexNames)
            {
                string indexNameString = (string)indexName;
                tableIndexNames.Add(indexNameString);
            }

            return tableIndexNames;
        }

        #endregion
    }
}
