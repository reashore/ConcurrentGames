using System;
using System.Collections.Generic;
using System.Configuration;
using SportsIq.Models.Constants.Nhl;
using SportsIq.Models.Markets;
using SportsIq.Utilities;
using static System.Convert;

namespace SportsIq.Analytica.Nhl
{
    public interface IAnalyticaNhl
    {
        bool IsTeamMode { get; set; }
        Dictionary<int, List<Market>> RunModel(Dictionary<string, Dictionary<string, double>> modelData, Dictionary<int, List<Market>> periodMarkets, bool started, string saveFileName);
    }

    public class AnalyticaNhl : AnalyticaBase, IAnalyticaNhl
    {
        private readonly double _preVig;
        private readonly double _liveVig;

        public AnalyticaNhl()
        {
            _preVig = ToDouble(ConfigurationManager.AppSettings["preVig"]);
            _liveVig = ToDouble(ConfigurationManager.AppSettings["liveVig"]);
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
                        {NhlModelDataKeys.InMlf,  new[] {"iGP", "iBL", "iGL2"}},            // Market Odds
                        //{NhlModelDataKeys.InLMlf, new[] {"iBL", "iGL2"}},                   // Live Market Odds
                        {NhlModelDataKeys.Evs,    new[] {"iTVH", "iGP"}},                   // Game State
                        {NhlModelDataKeys.Egt,    new[] {"iGT"}}                            // Score
                    };

              //  Dictionary<string, double> tempDictionary = Utils.CopyDictionary(modelData[NhlModelDataKeys.InMlf]);
              //  modelData[NhlModelDataKeys.InMlf] = ValidateMarketOdds(modelData[NhlModelDataKeys.InMlf]);
                LoadTables(tableIndicesDictionary, modelData);
             //   modelData[NhlModelDataKeys.InMlf] = tempDictionary;

                try
                {
                    bool live = false;
                    /*
                    if (modelData.ContainsKey(NhlModelDataKeys.InLMlf))
                    {
                        bool arePeriodMarketsValid1 = false;
                        bool arePeriodMarketsValid3 = false;

                        if (modelData[NhlModelDataKeys.InLMlf].ContainsKey("TO,T"))
                        {
                            arePeriodMarketsValid1 = modelData[NhlModelDataKeys.InLMlf]["TO,T"].IsNotEqualToZero();
                        }

                        if (modelData[NhlModelDataKeys.InLMlf].ContainsKey("ML,S1"))
                        {
                            arePeriodMarketsValid3 =
                                modelData[NhlModelDataKeys.InLMlf]["ML,S1"].IsNotEqualToZero();
                        }

                        live = arePeriodMarketsValid1 || arePeriodMarketsValid3;
                    }
                    */
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
            //return new Dictionary<int, List<Market>>();
            
                Dictionary<string, string[]> tableIndicesDictionary = new Dictionary<string, string[]>
                    {
                    //  {NhlModelDataKeys.Poi, new[] {"iTVH", "iPOI"}},  //PLAYERS ON ICE
                    //    {NhlModelDataKeys.Pib, new[] {"iTVH", "iPOI","iPSP"}},  //PLAYERS IN BOX
                        {NhlModelDataKeys.ScoreS, new[] {"iTVH", "iGP", "iSMT", "iPLY"}},  //SCORE SKATERS
                        {NhlModelDataKeys.Egt, new[] {"iGT"}},  //EVENT GAME TIME
                        {NhlModelDataKeys.InPp, new[] {"iTVH", "iAPP"}},  //POWER PLAY END TIMES
                        {NhlModelDataKeys.ScoreG, new[] {"iTVH", "iGP", "iGO", "iGMT" }},  //POWER PLAY END TIMES
                        {NhlModelDataKeys.InSgp,   new[] { "iTVH", "iPLY", "iGMPS" }},      //PLAYER GAMES PLAYED 
                        {NhlModelDataKeys.InSsl,  new[] { "iTVH", "iGP", "iSMT", "iTS", "iPLY" }},
                        {NhlModelDataKeys.InSst,  new[] { "iTVH", "iGP", "iSMT", "iTS", "iPLY" }},
                        {NhlModelDataKeys.InSsg,  new[] { "iTVH", "iGP", "iGO", "iSMT", "iTS", "iPLY" }},
                        {NhlModelDataKeys.InGgp,  new[] { "iTVH", "iGO", "iGMPG"}},
                        {NhlModelDataKeys.InGsl,  new[] { "iTVH", "iGP", "IGMT", "iTS", "iGO"}},
                        {NhlModelDataKeys.InGst,  new[] { "iTVH", "iGP", "IGMT", "iTS", "iGO"}} 
                    };
                bool _live = true;
                 LoadTables(tableIndicesDictionary, modelData);
                Dictionary<string, double> adjustment = new Dictionary<string, double>();
               CalculatePlayerMarkets(periodMarkets, started, _live,adjustment);
                SaveModel(saveFileName);
            //}
            //catch (Exception exception)
           // {
            //    Logger.Error(exception);
            //    throw;
           // }

            return periodMarkets;




        }

        /* make sure that the model has at least the money and total for the period */
        private static Dictionary<string, double> ValidateMarketOdds(Dictionary<string, double> marketDictionary)
        {
            Dictionary<string, double> newMarketDictionary = Utils.CopyDictionary(marketDictionary);
            List<string> periodList =  new List<string> { "P0", "P2", "P1", "P3" };

            foreach (string period in periodList)
            {
                string spreadKey = $"{period},ML,S1";
                string totalKey = $"{period},TO,S1";
                bool spreadSet = newMarketDictionary[spreadKey].IsEqualToZero();
                bool totalSet = newMarketDictionary[totalKey].IsEqualToZero();

                // todo is this expression correct?
                bool notSet = !(spreadSet && totalSet);

                //SEE IF THEY ARE SET IF NOT MAKE ALL ZERO
                if (notSet)
                {
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
            if (!modelData.ContainsKey(NhlModelDataKeys.InMlf))
            {
                return false;
            }

            Dictionary<string, double> inMlf = modelData[NhlModelDataKeys.InMlf];

            if (inMlf.Count == 0)
            {
                return false;
            }

            // todo key does not exist in dictionary
            bool arePeriodMarketsValid1 = inMlf["P0,TO,T"].IsNotEqualToZero();
            bool arePeriodMarketsValid2 = inMlf["P0,SP,T"].IsNotEqualToZero();
            bool arePeriodMarketsValid3 = inMlf["P0,ML,S1"].IsNotEqualToZero();
            bool arePeriodMarketsValid = arePeriodMarketsValid1 || arePeriodMarketsValid2 || arePeriodMarketsValid3;

            return arePeriodMarketsValid;
        }

        private void CalculatePlayerMarkets(IDictionary<int, List<Market>> periodMarkets, bool started, bool live, Dictionary<string, double> adjustment)
        {
            try
            {
                GetPlayerProps(periodMarkets, started, live, adjustment);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }
        }

        private void GetPlayerProps(IDictionary<int, List<Market>> periodMarkets, bool started, bool live, Dictionary<string, double> adjust)
        {
            const string liveNotLive = "SPP";
            const string tableName = liveNotLive;
            string[] indexArray = { "iSMT", "iTVH", "iPLY", "iGO", "iGL2" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);
            int lowerBound1 = array.GetLowerBound(1);
            int upperBound1 = array.GetUpperBound(1);
            int lowerBound2 = array.GetLowerBound(2);
            int upperBound2 = array.GetUpperBound(2);
            int lowerBound3 = array.GetLowerBound(3);
            int upperBound3 = array.GetUpperBound(3);
            const string s1 = "U";
            const string s2 = "O";

            string[] types = { "P",  "A", "G", "B"};

            for (int metric = lowerBound0; metric <= upperBound0; metric++) //metric type
            {
                string metricStr = types[metric - 1]; //get type string

                for (int side = lowerBound1; side <= upperBound1; side++) //side
                {
                    string sideStr = side == 1 ? "away" : "home";

                    for (int player = lowerBound2; player <= upperBound2; player++) //side
                    {
                        
                        //Player playerEl = gm.roster.players.Find(player => player.side == sideStr & player.num == pNum);
                        //create a new player
                        for (int alts = lowerBound3; alts <= upperBound3; alts++)
                        {

                            double value1 = GetArrayValue(array, metric, side, player, alts, 1);
                            double value2 = GetArrayValue(array, metric, side, player, alts, 2);
                            double value3 = GetArrayValue(array, metric, side, player, alts, 3);

                            Market market = new Market
                            {
                                Tp = metricStr,
                                Target = value1,
                                Player = $"{sideStr}_{player}"
                            };


                            market.MarketRunnerList.Add(new MarketRunner
                            {
                                Total = value1,
                                Price = value2,
                                Side = s1
                            });

                            market.MarketRunnerList.Add(new MarketRunner
                            {
                                Total = value1,
                                Price = value3,
                                Side = s2
                            });

                            market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                            market.Key = $"{market.MarketRunnerList[0].Total}1{market.Tp}";

                            if (!periodMarkets.ContainsKey(1))
                            {
                                periodMarkets.Add(1, new List<Market>());
                            }

                            periodMarkets[1].Add(market);
                        }

                  
                    }
                }
            }
        }

        private void GetGoalieProps(IDictionary<int, List<Market>> periodMarkets, bool started, bool live, Dictionary<string, double> adjust)
        {
            const string liveNotLive = "GGAP";
            const string tableName = liveNotLive;
            string[] indexArray = {"iTVH", "iGO", "ST","iGL2" };
            Array array = GetTableArray(tableName, indexArray);
            int lowerBound0 = array.GetLowerBound(0);
            int upperBound0 = array.GetUpperBound(0);
            int lowerBound1 = array.GetLowerBound(1);
            int upperBound1 = array.GetUpperBound(1);
            int lowerBound2 = array.GetLowerBound(2);
            int upperBound2 = array.GetUpperBound(2);
            int lowerBound3 = array.GetLowerBound(3);
            int upperBound3 = array.GetUpperBound(3);
            const string s1 = "U";
            const string s2 = "O";

            //string[] types = { "GA" };


            string metricStr = "GA"; //get type string

                for (int side = lowerBound0; side <= upperBound0; side++) //side
                {
                    string sideStr = side == 1 ? "away" : "home";

                    for (int player = lowerBound1; player <= upperBound1; player++) //side
                    {
                        //Player playerEl = gm.roster.players.Find(player => player.side == sideStr & player.num == pNum);
                        //create a new player

                        for (int alts = lowerBound2; alts <= upperBound2; alts++)
                        {

                            double value1 = GetArrayValue(array, side, player, alts, 1);
                            double value2 = GetArrayValue(array, side, player, alts, 2);
                            double value3 = GetArrayValue(array, side, player, alts, 3);

                            Market market = new Market
                            {
                                Tp = metricStr,
                                Target = value1,
                                Player = $"{sideStr}_{player}"
                            };


                            market.MarketRunnerList.Add(new MarketRunner
                            {
                                Total = value1,
                                Price = value2,
                                Side = s1
                            });

                            market.MarketRunnerList.Add(new MarketRunner
                            {
                                Total = value1,
                                Price = value3,
                                Side = s2
                            });

                            market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                            market.Key = $"{market.MarketRunnerList[0].Total}1{market.Tp}";

                            if (!periodMarkets.ContainsKey(1))
                            {
                                periodMarkets.Add(1, new List<Market>());
                            }

                            periodMarkets[1].Add(market);
                        }


                    }
                }
            
        }



        private void CalculateMarkets(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            //clear again?
            // periodMarkets.Clear();

            GetMainLinesTwoWaysResults(periodMarkets, started, live);
           
            GetMainLinesThreeWaysResults(periodMarkets, started, live);

            GetTeamTotalsTwoWaysResults(periodMarkets, started, live);


//            GetInningTeamTotalsTwoWaysResults(periodMarkets, started, live);

 //           GetTeamTotalsThreeWaysResults(periodMarkets, started, live);
 //           GetInningTeamTotalsThreeWaysResults(periodMarkets, started, live);
        }

        private void GetMainLinesTwoWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            string tableName = live ? "Mgml2w" : "gml2w";
            string[] indexArray = { "iGP", "altp", "iTGT", "iGL2" };
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

        private void GetMainLinesThreeWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            string tableName = live ? "MGML3W" : "GML3W";
            string[] indexArray = { "iGP", "altp", "iTGT", "iGL3" };
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
                        market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
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
                        market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                        market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";
                        periodMarkets[period].Add(market);
                    }
                }
            }
        }

 
        private void GetTeamTotalsTwoWaysResults(IDictionary<int, List<Market>> periodMarkets, bool started, bool live)
        {
            string tableName = live ? "Mgtt2w" : "gtt2w";
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

                    if (value4.IsNotEqualToZero())
                    {
                        market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                        market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";
                        periodMarkets[period].Add(market);
                    }

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

                    if (value4.IsNotEqualToZero())
                    {
                        market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                        market.Key = $"{market.MarketRunnerList[0].Total}{period}{market.Tp}";

                        periodMarkets[period].Add(market);
                    }
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

                    if (value4.IsNotEqualToZero())
                    {
                        market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                        market.Key = $"{market.MarketRunnerList[0].Total}{init}{market.Tp}";

                        periodMarkets[init].Add(market);
                    }


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

                    if (value4.IsNotEqualToZero())
                    {
                        market = OddsConverter.FinishedOdds(market, started, _preVig, _liveVig);
                        market.Key = $"{market.MarketRunnerList[0].Total}{init}{market.Tp}";

                        periodMarkets[init].Add(market);
                    }
                }
            }
        }

        #endregion
    }
}
