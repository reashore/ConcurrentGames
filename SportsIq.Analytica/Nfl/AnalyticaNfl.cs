using System;
using System.Collections.Generic;
using System.Configuration;
using SportsIq.Models.Constants.Nfl;
using SportsIq.Models.Markets;
using SportsIq.Utilities;
using static System.Convert;

namespace SportsIq.Analytica.Nfl
{
    public interface IAnalyticaNfl
    {
        bool IsTeamMode { get; set; }
        Dictionary<int, List<Market>> RunModel(Dictionary<string, Dictionary<string, double>> modelData, Dictionary<int, List<Market>> periodMarkets, bool started, string saveFileName);
    }

    public class AnalyticaNfl : AnalyticaBase, IAnalyticaNfl
    {
        private readonly double _preVig;
        private readonly double _liveVig;
        private readonly int _iSw;
        private int _iAdjT;
        private int _iAdjS;
        private bool _followLive;
        private bool _live;

        public AnalyticaNfl()
        {
            _preVig = ToDouble(ConfigurationManager.AppSettings["preVig"]);
            _liveVig = ToDouble(ConfigurationManager.AppSettings["liveVig"]);
            _iSw = 1;
            _iAdjS = 11;
            _iAdjT = 11;
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

        private Dictionary<int, List<Market>> RunTeamModel(Dictionary<string, Dictionary<string, double>> modelData, Dictionary<int, List<Market>> periodMarkets, bool started, string saveFileName)
        {
            if (!IsModelDataValid(modelData))
            {
                Logger.Error("RunTeamModel(): ModelData is invalid");
                return periodMarkets;
            }


            //settings
            if (!modelData[NflModelDataKeys.Settings].ContainsKey("MSR"))
            {
                modelData[NflModelDataKeys.Settings]["MSR"] = 0.0;
            }

            if (!modelData[NflModelDataKeys.Settings].ContainsKey("SCW"))
            {
                modelData[NflModelDataKeys.Settings]["SCW"] = 0.0;
            }

            if (!modelData[NflModelDataKeys.Settings].ContainsKey("FOL"))
            {
                modelData[NflModelDataKeys.Settings]["FOL"] = 1;
            }

            Dictionary<string, double> tempSettings = new Dictionary<string, double>
            {
                {"MSR", modelData[NflModelDataKeys.Settings]["MSR"]}, 
                {"SCW", modelData[NflModelDataKeys.Settings]["SCW"]}
            };
            UpdateSettings(tempSettings);

            _followLive = modelData[NflModelDataKeys.Settings]["FOL"].IsEqualTo(1);

            try
            {
                Dictionary<string, string[]> tableIndicesDictionary = new Dictionary<string, string[]>
                    {
                        {NflModelDataKeys.InMlf,    new[] {"iGP", "iBL", "iGL2"}},                    // Market Odds
                        {NflModelDataKeys.InLMlf,   new[] { "iGP", "iBL", "iGL2"}},                   // Live Market Odds
                        {NflModelDataKeys.Evs,      new[] {"iTVH", "iGP"}},                           // Game State
                        {NflModelDataKeys.Egt,      new[] {"iGSI"}},                                  // Score
                        {NflModelDataKeys.InTsf ,   new[] {"iTVH","iQT","iSAS","iGT"}},
                        {NflModelDataKeys.InSc ,    new[] {"iTVH","iQT","iGT"}},
                        { NflModelDataKeys.Xs ,    new[] { "iTVH", "iACS"}}
                    };

                //LoadTables(tableIndicesDictionary, modelData);
                Dictionary<string, double> tempDictionary = Utils.CopyDictionary(modelData[NflModelDataKeys.InMlf]);
                modelData[NflModelDataKeys.InMlf] = ValidateMarketOdds(modelData[NflModelDataKeys.InMlf]);
                LoadTables(tableIndicesDictionary, modelData);
                modelData[NflModelDataKeys.InMlf] = tempDictionary;

                try
                {
                   
                    if (!modelData.ContainsKey(NflModelDataKeys.InLMlf))
                    {
                        bool arePeriodMarketsValid1 = modelData[NflModelDataKeys.InLMlf]["CG,TO,T"].IsNotEqualToZero();
                        bool arePeriodMarketsValid2 = modelData[NflModelDataKeys.InLMlf]["CG,SP,T"].IsNotEqualToZero();
                        bool arePeriodMarketsValid3 = modelData[NflModelDataKeys.InLMlf]["CG,ML,S1"].IsNotEqualToZero();
                        _live = arePeriodMarketsValid1 && arePeriodMarketsValid2 && arePeriodMarketsValid3 && _followLive;
                    }

                    Dictionary<string, double> tempLiveDictionary = Utils.CopyDictionary(modelData[NflModelDataKeys.InLMlf]);
                    modelData[NflModelDataKeys.InLMlf] = ValidateMarketOdds(modelData[NflModelDataKeys.InLMlf]);

                    //live = false;
                    CalculateMarkets(periodMarkets, started, _live, modelData[NflModelDataKeys.Adjust]);
                    modelData[NflModelDataKeys.InLMlf] = tempLiveDictionary;
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

        /* set the single value objects */
        private void UpdateSettings(Dictionary<string, double> settings)
        {
            foreach (KeyValuePair<string, double> setting in settings)
            {
                SetDescription(setting.Key, setting.Value);
            }
        }

        private static Dictionary<string, double> ValidateMarketOdds(Dictionary<string, double> marketDictionary)
        {
            // make sure that the model has at least the money and total for the period 

            Dictionary<string, double> newMarketDictionary = Utils.CopyDictionary(marketDictionary);
            List<string> periodList = new List<string> { "H1", "H2", "Q1", "Q2", "Q3", "Q4", "CG" };

            foreach (string period in periodList)
            {
                string spreadKey = $"{period},SP,S1";
                string totalKey = $"{period},TO,S1";
                string moneyKey = $"{period},ML,S1";

                bool spreadSet = false;
                if (newMarketDictionary.ContainsKey(spreadKey))
                {
                    spreadSet = newMarketDictionary[spreadKey].IsEqualToZero();
                }

                bool totalSet = false;
                if (newMarketDictionary.ContainsKey(totalKey))
                {
                    totalSet = newMarketDictionary[totalKey].IsEqualToZero();
                }

                bool moneySet = false;
                if (newMarketDictionary.ContainsKey(moneyKey))
                {
                    moneySet = newMarketDictionary[moneyKey].IsEqualToZero();
                }
                  

                bool notSet = !(totalSet == false && spreadSet == false && moneySet == false);
                
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
            if (!modelData.ContainsKey(NflModelDataKeys.InMlf))
            {
                return false;
            }

            Dictionary<string, double> inMlf = modelData[NflModelDataKeys.InMlf];

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

        private void CalculateMarkets(IDictionary<int, List<Market>> periodMarkets, bool started, bool live, Dictionary<string,double> adjustment)
        {
            live = false;

            if (live)
            {
                GetMainLinesTwoWaysResultsLive(periodMarkets, started, live);
            }
            else
            {
                GetMainLinesTwoWaysResults(periodMarkets, started, live, adjustment);
            }
            
            if (live)
            {
                GetMoneyTwoWaysResultsLive(periodMarkets, started);
            }
            else
            {
                GetMoneyTwoWaysResults(periodMarkets, started, adjustment);
            }
            
            GetMoneyThreeWaysResults(periodMarkets, started);
            
            GetMainLinesThreeWaysResults(periodMarkets, started);

            GetTeamTotalsTwoWaysResults(periodMarkets, started);

            GetTeamTotalsThreeWaysResults(periodMarkets, started);
            
        }

        private void GetMainLinesTwoWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started, bool live, Dictionary<string,double> adjust)
        {
            //const string tableName = "ML2";
            string liveNotLive = live == false ? "ML2" : "SML";
            string tableName = liveNotLive;
            string[] indexArray = { "iGP", "iALT", "iTGT", "iGL2", "iSW", "iAdjS", "iAdjT" };
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
                    string type = "TL";
                    
                    //CHECK ADJUSTMENTS
                    string key = $"{type},{gp}";
                    if (adjust.ContainsKey(key))
                    {
                        _iAdjT = ToInt16(adjust[key]);
                    }
                    else
                    {
                        _iAdjT = 11;
                    }

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
                        Tp = type,
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
                    s1 = "A";
                    s2 = "H";
                    type = "SP";

                    //CHECK ADJUSTMENTS
                    key = $"{type},{gp}";
                    if (adjust.ContainsKey(key))
                    {
                        _iAdjS = ToInt16(adjust[key]);
                    }
                    else
                    {
                        _iAdjS = 11;
                    }

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
                        Tp = type,
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

               //     if (market.MarketRunnerList[0].Price > 0)
               //     {
                        market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                        market.Key = $"{market.MarketRunnerList[0].Total}{gp}{market.Tp}";

                        periodMarkets[gp].Add(market);
                 //   }
                }
            }
        }

        private void GetMainLinesTwoWaysResultsLive(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            const string liveNotLive = "SML";
            const string tableName = liveNotLive;
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

                    double value1 = GetArrayValue(array, gp, altp, 2, 1,  _iAdjS, _iAdjT);
                    double value2 = GetArrayValue(array, gp, altp, 2, 2,  _iAdjS, _iAdjT);
                    double value3 = GetArrayValue(array, gp, altp, 2, 3,  _iAdjS, _iAdjT);

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

                    value1 = GetArrayValue(array, gp, altp, 1, 1,  _iAdjS, _iAdjT);
                    value2 = GetArrayValue(array, gp, altp, 1, 2,  _iAdjS, _iAdjT);
                    value3 = GetArrayValue(array, gp, altp, 1, 3,  _iAdjS, _iAdjT);

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

        private void GetMoneyTwoWaysResultsLive(IDictionary<int, List<Market>> periodMarkets, bool started)
        {
            // const string tableName = "MNL2";
            const string liveNotLive = "Shml";
            const string tableName = liveNotLive;
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


        private void GetMoneyTwoWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started, Dictionary<string, double> adjust)
        {
            // const string tableName = "MNL2";
            const string liveNotLive = "MNL2";
            const string tableName = liveNotLive;
            string[] indexArray = { "iGP", "iGL2", "iSW", "iAdjS", "iAdjT" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);

            for (int gp = lowerBound0; gp <= upperBound0; gp++)
            {
                const string s1 = "A";
                const string s2 = "H";
                const string type = "SP";

                //CHECK ADJUSTMENTS
                string key = $"{type},{gp}";
                if (adjust.ContainsKey(key))
                {
                    _iAdjS = ToInt16(adjust[key]);
                }
                else
                {
                    _iAdjS = 11;
                }

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
