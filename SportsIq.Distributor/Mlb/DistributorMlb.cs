using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Newtonsoft.Json;
using SportsIq.Models.Markets;
using SportsIq.NoSqlDataAccess;
using SportsIq.Pusher;
using SportsIq.SqlDataAccess.Mlb;
using SportsIq.Utilities;
using static System.Convert;

namespace SportsIq.Distributor.Mlb
{
    public interface IDistributorMlb
    {
        void SendMarkets(Dictionary<int, List<Market>> periodMarketsDictionary, Guid gameId, string match, Dictionary<string, double> gameState);
    }

    public class DistributorMlb : DistributorBase, IDistributorMlb
    {
        private readonly IDatastore _datastore;
        private readonly Dictionary<int, string> _marketList;
        private List<string> _periods;
        private List<int> _periodMoneyLines;
        private List<int> _periodRunLines;
        private List<int> _altRunLines;
        private List<int> _totalLines;
        private List<int> _altTotalLines;
        private List<int> _aTeamTotalLines;
        private List<int> _altATeamTotalLines;
        private List<int> _altHTeamTotalLines;
        private List<int> _totalLines3W;
        private List<int> _periodMoneyLines3W;
        private List<int> _hTeamTotalLines;
        private List<int> _altTotalLines3W;
        private List<int> _runLines3W;
        private List<int> _altRunLines3W;

        static DistributorMlb()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(DistributorMlb));
        }

        public DistributorMlb(IDataAccessMlb dataAccessMlb, IDatastore datastore, IPusherUtil pusherUtil) 
        {
            _datastore = datastore;
            PusherUtil = pusherUtil;
            // todo convert to dictionary?
            _marketList = dataAccessMlb.GetMarkets();
            InitializeLists();
        }

        private void InitializeLists()
        {
            _periods = new List<string> { "", "F3", "F5", "F7", "CG", "I1", "I2", "I3", "I4", "I5", "I6", "I7", "I8", "I9" };

            // moneylines
            _periodMoneyLines = new List<int> { 0, 18, 29, 40, 1, 51, 56, 61, 66, 71, 76, 81, 183, 184 };
            _periodRunLines = new List<int> { 0, 19, 30, 41, 2, 52, 57, 62, 67, 72, 77, 82, 185, 186 };
            _altRunLines = new List<int> { 0, 27, 38, 49, 16, 95, 96, 97, 98, 99, 100, 101, 181, 182 };
            _totalLines = new List<int> { 0, 20, 31, 42, 3, 53, 58, 63, 68, 73, 78, 83, 187, 188 };
            _altTotalLines = new List<int> { 0, 28, 39, 50, 17, 102, 103, 104, 105, 106, 107, 108, 179, 180 };
            _aTeamTotalLines = new List<int> { 0, 21, 32, 43, 4, 138, 139, 140, 141, 142, 143, 144, 191, 192 };
            _altATeamTotalLines = new List<int> { 0, 125, 127, 129, 123, 109, 110, 111, 112, 113, 114, 115, 189, 190 };
            _hTeamTotalLines = new List<int> { 0, 22, 33, 44, 5, 131, 132, 133, 134, 135, 136, 137, 193, 194 };
            _altHTeamTotalLines = new List<int> { 0, 126, 128, 130, 124, 116, 117, 118, 119, 120, 121, 122, 195, 196 };
            _totalLines3W = new List<int> { 0, 23, 34, 45, 6, 54, 59, 64, 69, 74, 79, 84, 197, 198 };
            _altTotalLines3W = new List<int> { 0, 146, 147, 148, 145, 149, 150, 151, 152, 153, 154, 155, 199, 200 };
            _runLines3W = new List<int> { 0, 169, 170, 171, 168, 172, 173, 174, 175, 176, 177, 178, 203, 204 };
            _periodMoneyLines3W = new List<int> { 0, 24, 35, 46, 7, 55, 60, 65, 70, 75, 80, 85, 201, 202 };
            _altRunLines3W = new List<int> { 0, 158, 159, 160, 156, 161, 162, 163, 164, 165, 166, 167, 208, 209 };
        }

        public void SendMarkets(Dictionary<int, List<Market>> periodMarketsDictionary, Guid gameId, string match, Dictionary<string, double> gameState)
        {
            // todo divide into smaller functions
            if (periodMarketsDictionary.Count == 0)
            {
                return;
            }

            if (periodMarketsDictionary[1].Count == 0)
            {
                return;
            }

            const string eventName = "MLB";
            List<Markets> formattedMarketList = new List<Markets>();

            foreach (KeyValuePair<int, List<Market>> keyValuePair in periodMarketsDictionary)
            {
                int key = keyValuePair.Key;
                List<Market> marketList = keyValuePair.Value;
                string period = _periods[key];

                //------------------------------------------------------------------------

                //center handicap exists if false send takedown
                bool cHandicap = false;
                bool cTotal = false;
                bool cHomeTotal = false;
                bool cAwayTotal = false;

                int id = _altRunLines[key];

                Markets handicapAlts = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(m => SelectInsideLimitsWithNonZeroTarget(m, "SP"))
                };

                if (handicapAlts.MarketList.Count > 0)
                {
                    Markets handicapAltsSet = SetMarketActiveNew(handicapAlts, true, gameState);
                    SendOddsMessage(handicapAltsSet, gameId, formattedMarketList, eventName);

                    // center line fix to create 1.5 run line standard for full game

                    List<Market> marketListRunLine =
                        handicapAlts.MarketList.FindAll(v => Math.Abs(v.Target).IsEqualTo(1.5));

                    if (marketListRunLine.Count > 0 && key == 4 && gameState["I"] < 2)
                    {
                        Market market = marketListRunLine.OrderBy(v => v.Weight).First();

                        if (market != null)
                        {
                            cHandicap = true;
                            int id2 = _periodRunLines[key];
                            market.Active = CheckActive(period, gameState, true);

                            Markets handicap = new Markets
                            {
                                Game = gameId.ToString(),
                                Period = period,
                                Id = id2,
                                Name = _marketList[id2]
                            };

                            handicap.MarketList.Add(market);
                            SendOddsMessage(handicap, gameId, formattedMarketList, eventName);
                        }
                    }
                    else
                    {
                        Market market = handicapAlts.MarketList.OrderBy(v => v.Weight).First();

                        if (market != null)
                        {
                            cHandicap = true;
                            int id2 = _periodRunLines[key];
                            market.Active = CheckActive(period, gameState, true);

                            Markets handicap = new Markets
                            {
                                Game = gameId.ToString(),
                                Period = period,
                                Id = id2,
                                Name = _marketList[id2]
                            };

                            handicap.MarketList.Add(market);
                            SendOddsMessage(handicap, gameId, formattedMarketList, eventName);
                        }
                    }
                }

                Markets handicapAltsInactive = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(m => SelectOutsideLimitsWithNonZeroTarget(m, "SP"))
                };

                if (handicapAltsInactive.MarketList.Count > 0 && handicapAlts.MarketList.Count == 0)
                {
                    SendOddsMessage2(SetMarketInactive(handicapAltsInactive), gameId, eventName);

                    if (handicapAltsInactive.MarketList.Count == 0)
                    {
                        Market market = new Market
                        {
                            Active = false,
                            Tp = "SP",
                            Target = 0
                        };

                        int id2 = _periodRunLines[key];

                        Markets handicap = new Markets
                        {
                            Game = gameId.ToString(),
                            Period = period,
                            Id = id2,
                            Name = _marketList[id2]
                        };

                        handicap.MarketList.Add(market);
                        if (!cHandicap)
                        {
                            SendOddsMessage2(handicap, gameId, eventName);
                        }
                    }
                }

                //------------------------------------------------------------------------

                id = _periodMoneyLines[key];

                Markets moneyline = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id]
                };

                Market market1 = marketList.Find(s => SelectInsideLimitsWithZeroTarget(s, "SP"));

                if (market1 != null)
                {
                    market1.Active = CheckActive(period, gameState, true);
                    market1.Tp = "ML";
                    moneyline.MarketList.Add(market1);
                    SendOddsMessage(moneyline, gameId, formattedMarketList, eventName);
                }
                else
                {
                    Market market = new Market
                    {
                        Active = false,
                        Tp = "ML",
                        Target = 0
                    };
                    moneyline.MarketList.Add(market);
                    SendOddsMessage2(moneyline, gameId, eventName);
                }

                //------------------------------------------------------------------------

                id = _altTotalLines[key];

                Markets totallineAlt = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s => SelectInsideLimitsWithNonZeroTarget(s, "TL"))
                };

                Markets totallineAltInactive = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s => SelectOutsideLimitsWithNonZeroTarget(s, "TL"))
                };

                if (totallineAlt.MarketList.Count > 0)
                {
                    Markets activeMarkets = SetMarketActiveNew(totallineAlt, true, gameState);
                    SendOddsMessage(activeMarkets, gameId, formattedMarketList, eventName);
                    Market market2 = totallineAlt.MarketList.OrderBy(v => v.Weight).First();

                    if (market2 != null)
                    {
                        cTotal = true;
                        int id2 = _totalLines[key];
                        market2.Active = CheckActive(period, gameState, true);
                        Markets totalline = new Markets
                        {
                            Game = gameId.ToString(),
                            Period = period,
                            Id = id2,
                            Name = _marketList[id2]
                        };

                        totalline.MarketList.Add(market2);
                        SendOddsMessage(totalline, gameId, formattedMarketList, eventName);
                    }
                }

                if (totallineAltInactive.MarketList.Count > 0 && totallineAlt.MarketList.Count == 0)
                {
                    SendOddsMessage2(SetMarketInactive(totallineAltInactive), gameId, eventName);
                    Market market2 = totallineAltInactive.MarketList.OrderBy(v => v.Weight).First();

                    if (market2 != null)
                    {
                        int id2 = _totalLines[key];
                        market2.Active = false;
                        Markets totalline = new Markets
                        {
                            Game = gameId.ToString(),
                            Period = period,
                            Id = id2,
                            Name = _marketList[id2]
                        };

                        totalline.MarketList.Add(market2);
                        if (!cTotal)
                        {
                            SendOddsMessage2(totalline, gameId, eventName);
                        }
                    }
                }

                //------------------------------------------------------------------------

                id = _altATeamTotalLines[key];

                Markets awayTotallineAlt = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s => SelectInsideLimitsWithNonZeroTarget(s, "ATTL"))
                };

                if (awayTotallineAlt.MarketList.Count > 0)
                {
                    Markets activeMarkets = SetMarketActiveNew(awayTotallineAlt, true, gameState);
                    SendOddsMessage(activeMarkets, gameId, formattedMarketList, eventName);
                    Market market3 = awayTotallineAlt.MarketList.OrderBy(v => v.Weight).First();

                    if (market3 != null)
                    {
                        cAwayTotal = true;
                        int id2 = _aTeamTotalLines[key];
                        market3.Active = CheckActive(period, gameState, true);
                        Markets awayTotalline = new Markets
                        {
                            Game = gameId.ToString(),
                            Period = period,
                            Id = id2,
                            Name = _marketList[id2]
                        };

                        awayTotalline.MarketList.Add(market3);
                        SendOddsMessage(awayTotalline, gameId, formattedMarketList, eventName);
                    }
                }

                Markets awayTotallineAltInactive = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s => SelectOutsideLimitsWithNonZeroTarget(s, "ATTL"))
                };

                if (awayTotallineAltInactive.MarketList.Count > 0 && awayTotallineAlt.MarketList.Count == 0)
                {
                    SendOddsMessage2(SetMarketInactive(awayTotallineAltInactive), gameId, eventName);
                    Market market3 = awayTotallineAltInactive.MarketList.OrderBy(v => v.Weight).First();

                    if (market3 != null)
                    {
                        int id2 = _aTeamTotalLines[key];
                        market3.Active = false;
                        Markets awayTotalline = new Markets
                        {
                            Game = gameId.ToString(),
                            Period = period,
                            Id = id2,
                            Name = _marketList[id2]
                        };

                        awayTotalline.MarketList.Add(market3);
                        if (!cAwayTotal)
                        {
                            SendOddsMessage2(awayTotalline, gameId, eventName);
                        }
                    }
                }

                //------------------------------------------------------------------------

                id = _altHTeamTotalLines[key];

                Markets homeTotallineAlt = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s => SelectInsideLimitsWithNonZeroTarget(s, "HTTL"))
                };

                if (homeTotallineAlt.MarketList.Count > 0)
                {
                    Markets activeMarkets = SetMarketActiveNew(homeTotallineAlt, true, gameState);
                    SendOddsMessage(activeMarkets, gameId, formattedMarketList, eventName);
                    Market market4 = homeTotallineAlt.MarketList.OrderBy(v => v.Weight).First();

                    if (market4 != null)
                    {
                        cHomeTotal = true;
                        int id2 = _hTeamTotalLines[key];
                        market4.Active = CheckActive(period, gameState, true);
                        Markets homeTotalline = new Markets
                        {
                            Game = gameId.ToString(),
                            Period = period,
                            Id = id2,
                            Name = _marketList[id2]
                        };

                        homeTotalline.MarketList.Add(market4);
                        SendOddsMessage(homeTotalline, gameId, formattedMarketList, eventName);
                    }
                }

                //inactive
                Markets homeTotallineAltInactive = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s => SelectOutsideLimitsWithNonZeroTarget(s, "HTTL"))
                };

                if (homeTotallineAltInactive.MarketList.Count > 0 && homeTotallineAlt.MarketList.Count == 0)
                {
                    SendOddsMessage2(SetMarketInactive(homeTotallineAltInactive), gameId, eventName);
                    Market market4 = homeTotallineAltInactive.MarketList.OrderBy(v => v.Weight).First();

                    if (market4 != null)
                    {
                        int id2 = _hTeamTotalLines[key];
                        market4.Active = false;
                        Markets homeTotalline = new Markets
                        {
                            Game = gameId.ToString(),
                            Period = period,
                            Id = id2,
                            Name = _marketList[id2]
                        };

                        homeTotalline.MarketList.Add(market4);
                        if (!cHomeTotal)
                        {
                            SendOddsMessage2(homeTotalline, gameId, eventName);
                        }
                    }
                }

                //------------------------to do send center line-----------------------------------------------

                id = _totalLines3W[key];

                Markets totalline3W = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = _totalLines3W[key],
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s => SelectInsideLimitsWithNonZeroTarget(s, "TL3W"))
                };

                if (totalline3W.MarketList.Count > 0)
                {
                    Markets activeMarkets = SetMarketActiveNew(totalline3W, true, gameState);
                    SendOddsMessage(activeMarkets, gameId, formattedMarketList, eventName);
                }


                Markets totalline3WInactive = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = _totalLines3W[key],
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s => SelectOutsideLimitsWithNonZeroTarget(s, "TL3W"))
                };

                if (totalline3WInactive.MarketList.Count > 0)
                {
                    SendOddsMessage2(SetMarketInactive(totalline3WInactive), gameId, eventName);
                }

                //------------------------------------------------------------------------

                id = _altRunLines3W[key];

                Markets handicap3W = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s => SelectInsideLimitsWithoutTarget(s,"SP3W"))
                };

                if (handicap3W.MarketList.Count > 0)
                {
                    Markets activeMarkets = SetMarketActiveNew(handicap3W, true, gameState);
                    SendOddsMessage(activeMarkets, gameId, formattedMarketList, eventName);
                }

                Markets handicap3WInactive = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s => SelectOutsideLimitsWithoutTarget(s, "SP3W"))
                };

                if (handicap3WInactive.MarketList.Count > 0 && handicap3W.MarketList.Count == 0)
                {
                    SendOddsMessage2(SetMarketInactive(handicap3WInactive), gameId, eventName);
                }
            }

            FormattedGame formattedGame = new FormattedGame
            {
                Id = gameId.ToString(),
                Match = match,
                MarketList = formattedMarketList
            };

            string formattedGameJsonString = JsonConvert.SerializeObject(formattedGame);
            _datastore.AddFinishedOdds(gameId, formattedGameJsonString);
        }

        private static bool CheckActive(string periodString, Dictionary<string, double> gameState, bool active)
        {
            int i = ToInt32(gameState["I"]);
            int t = ToInt32(gameState["T"]);

            switch (periodString)
            {
                case "I1":
                    if (i >= 1)
                    {
                        if (i == 1 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 1)
                        {
                            active = false;
                        }
                    }

                    break;

                case "I2":
                    if (i >= 2)
                    {
                        if (i == 2 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 2)
                        {
                            active = false;
                        }
                    }

                    break;

                case "I3":
                    if (i >= 3)
                    {
                        if (i == 3 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 3)
                        {
                            active = false;
                        }
                    }

                    break;


                case "I4":
                    if (i >= 4)
                    {
                        if (i == 4 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 4)
                        {
                            active = false;
                        }
                    }

                    break;

                case "I5":
                    if (i >= 5)
                    {
                        if (i == 5 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 5)
                        {
                            active = false;
                        }
                    }

                    break;

                case "I6":
                    if (i >= 6)
                    {
                        if (i == 6 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 6)
                        {
                            active = false;
                        }
                    }

                    break;

                case "I7":
                    if (i >= 7)
                    {
                        if (i == 7 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 7)
                        {
                            active = false;
                        }
                    }

                    break;

                case "I8":
                    if (i >= 8)
                    {
                        if (i == 8 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 8)
                        {
                            active = false;
                        }
                    }

                    break;

                case "I9":

                    if (i >= 9)
                    {
                        if (i == 9 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 9)
                        {
                            active = false;
                        }
                    }

                    break;
            }

            if (periodString == "F3" && i >= 3)
            {
                active = false;
            }

            if (periodString == "F5" && i >= 5)
            {
                active = false;
            }

            if (periodString == "F7" && i >= 7)
            {
                active = false;
            }

            return active;
        }

        private static Markets SetMarketActiveNew(Markets markets, bool active, Dictionary<string, double> gameState)
        {
            int i = ToInt32(gameState["I"]);
            int t = ToInt32(gameState["T"]);

            switch (markets.Period)
            {
                case "I1":
                    if (i >= 1)
                    {
                        if (i == 1 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 1)
                        {
                            active = false;
                        }
                    }
                    break;

                case "I2":
                    if (i >= 2)
                    {
                        if (i == 2 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 2)
                        {
                            active = false;
                        }
                    }

                    break;

                case "I3":
                    if (i >= 3)
                    {
                        if (i == 3 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 3)
                        {
                            active = false;
                        }
                    }

                    break;

                case "I4":
                    if (i >= 4)
                    {
                        if (i == 4 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 4)
                        {
                            active = false;
                        }
                    }

                    break;

                case "I5":
                    if (i >= 5)
                    {
                        if (i == 5 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 5)
                        {
                            active = false;
                        }
                    }

                    break;

                case "I6":
                    if (i >= 6)
                    {
                        if (i == 6 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 6)
                        {
                            active = false;
                        }
                    }

                    break;

                case "I7":
                    if (i >= 7)
                    {
                        if (i == 7 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 7)
                        {
                            active = false;
                        }
                    }

                    break;

                case "I8":
                    if (i >= 8)
                    {
                        if (i == 8 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 8)
                        {
                            active = false;
                        }
                    }

                    break;

                case "I9":
                    if (i >= 9)
                    {
                        if (i == 9 && t == 0)
                        {
                            active = false;
                        }

                        if (i > 9)
                        {
                            active = false;
                        }
                    }

                    break;
            }

            if (markets.Period == "F3" && i >= 3)
            {
                active = false;
            }

            if (markets.Period == "F5" && i >= 5)
            {
                active = false;
            }

            if (markets.Period == "F7" && i >= 7)
            {
                active = false;
            }

            foreach (Market market in markets.MarketList)
            {
                market.Active = active;
            }

            return markets;
        }
    }
}
