using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Newtonsoft.Json;
using SportsIq.Models.Markets;
using SportsIq.NoSqlDataAccess;
using SportsIq.Pusher;
using SportsIq.SqlDataAccess.Nhl;
using SportsIq.Utilities;
using static System.Convert;

namespace SportsIq.Distributor.Nhl
{
    public interface IDistributorNhl
    {
        void SendMarkets(Dictionary<int, List<Market>> periodMarketsDictionary, Guid gameId, string match, Dictionary<string, double> gameState);
        void SendPlayerMarkets(string playerMarkets, Guid gameId, string match);
    }

    public class DistributorNhl : IDistributorNhl
    {
        private static readonly ILog Logger;
        private readonly Dictionary<int, NhlMarket> _marketList;
        private readonly IDatastore _datastore;
        private readonly IPusherUtil _pusherUtil;
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

        static DistributorNhl()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(DistributorNhl));
        }

        public DistributorNhl(IDataAccessNhl dataAccessNhl, IDatastore datastore, IPusherUtil pusherUtil)
        {
            _datastore = datastore;
            _pusherUtil = pusherUtil;
            _marketList = dataAccessNhl.GetMarkets();
            InitializeLists();
        }

        private void InitializeLists()
        {
            _periods = new List<string> { "", "CG", "P1", "P2", "P3", "OT" };

            //moneylines
            _periodMoneyLines = new List<int> { 0, 95, 105, 114, 122, 122 };
            _periodRunLines = new List<int> { 0, 96, 107, 115, 124, 124 };
            _altRunLines = new List<int> { 0, 217, 173, 174, 175, 175 };
            _totalLines = new List<int> { 0, 97, 108, 116, 125, 125};
            _altTotalLines = new List<int> { 0, 208, 179, 180, 181, 181 };
            _aTeamTotalLines = new List<int> { 0, 101, 111, 119, 128, 128 };
            _altATeamTotalLines = new List<int> { 0, 198, 199, 185, 186, 186 };
            _hTeamTotalLines = new List<int> { 0, 102, 112, 120, 129, 129 };
            _altHTeamTotalLines = new List<int> { 0, 195, 196, 190, 191, 191 };
            _totalLines3W = new List<int> { 0, 100, 110, 118, 127, 137, 147, 157 };
            _altTotalLines3W = new List<int> { 0, 201, 202, 203, 204, 205, 206, 207 };
            _runLines3W = new List<int> { 0, 99, 109, 117, 126, 136, 146, 156 };
            _periodMoneyLines3W = new List<int> { 0, 98, 106, 216, 123, 133, 143, 153 };
            _altRunLines3W = new List<int> { 0, 209, 210, 211, 212, 213, 214, 215 };
        }

        public void SendPlayerMarkets(string playerMarkets, Guid gameId, string match)
        {
            // Logger.Info(playerMarkets);
            //const string eventName = "NBA";
            //List<Markets> formattedMarketList = new List<Markets>();
            _datastore.AddFinishedProps(gameId, playerMarkets);
        }

        public void SendMarkets(Dictionary<int, List<Market>> periodMarketsDictionary, Guid gameId, string match, Dictionary<string, double> gameState)
        {
            if (periodMarketsDictionary.Count == 0)
            {
                return;
            }

            if (periodMarketsDictionary[1].Count == 0)
            {
                return;
            }

            /*  testing
            gameState = new Dictionary<string, double>();
            gameState["I"] = 5;
            gameState["T"] = 0;
            */

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
                try
                {
                    Markets handicapAlts = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id].Name,
                        MarketList = marketList.FindAll(s =>
                            s.Tp == "SP" && !s.Target.IsEqualToZero() && s.MarketRunnerList[0].Probability > 0.035 &&
                            s.MarketRunnerList[0].Probability < 0.965)
                    };

                    if (handicapAlts.MarketList.Count > 0)
                    {
                        Markets handicapAltsSet = SetMarketActiveNew(handicapAlts, true, gameState);
                        SendOddsMessage(handicapAltsSet, gameId, formattedMarketList);

                        // center line fix to create 1.5 run line standard for full game

                        List<Market> marketListRunLine =
                            handicapAlts.MarketList.FindAll(v => Math.Abs(v.Target).IsEqualTo(1.5));

                        if (marketListRunLine.Count > 0 && key == 4 )
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
                                    Name = _marketList[id2].Name
                                };

                                handicap.MarketList.Add(market);
                                SendOddsMessage(handicap, gameId, formattedMarketList);
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
                                    Name = _marketList[id2].Name
                                };

                                handicap.MarketList.Add(market);
                                SendOddsMessage(handicap, gameId, formattedMarketList);
                            }
                        }
                    }

                    Markets handicapAltsInactive = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id].Name,
                        MarketList = marketList.FindAll(s =>
                            s.Tp == "SP" && !s.Target.IsEqualToZero() && (s.MarketRunnerList[0].Probability < 0.035 ||
                                                                          s.MarketRunnerList[0].Probability > 0.965))
                    };

                    if (handicapAltsInactive.MarketList.Count > 0 && handicapAlts.MarketList.Count == 0)
                    {
                        SendOddsMessage2(SetMarketInactive(handicapAltsInactive), gameId);

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
                                Name = _marketList[id2].Name
                            };

                            handicap.MarketList.Add(market);
                            if (!cHandicap)
                            {
                                SendOddsMessage2(handicap, gameId);
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
                        Name = _marketList[id].Name
                    };

                    Market market1 = marketList.Find(s =>
                        s.Tp == "SP" && s.Target.IsEqualToZero() && s.MarketRunnerList[0].Probability > 0.035 &&
                        s.MarketRunnerList[0].Probability < 0.965);

                    if (market1 != null)
                    {
                        market1.Active = CheckActive(period, gameState, true);
                        market1.Tp = "ML";
                        moneyline.MarketList.Add(market1);
                        SendOddsMessage(moneyline, gameId, formattedMarketList);
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
                        SendOddsMessage2(moneyline, gameId);
                    }
                }
                catch(Exception e)
                {
                    Logger.Info(e);
                }

                
                 
                id = _altTotalLines[key];

                Markets totallineAlt = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id].Name,
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "TL" && !s.Target.IsEqualToZero() && s.MarketRunnerList[0].Probability > 0.035 &&
                        s.MarketRunnerList[0].Probability < 0.965)
                };

                Markets totallineAltInactive = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id].Name,
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "TL" && !s.Target.IsEqualToZero() && (s.MarketRunnerList[0].Probability < 0.035 ||
                        s.MarketRunnerList[0].Probability > 0.965))
                };

                if (totallineAlt.MarketList.Count > 0)
                {
                    Markets activeMarkets = SetMarketActiveNew(totallineAlt, true, gameState);
                    SendOddsMessage(activeMarkets, gameId, formattedMarketList);
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
                            Name = _marketList[id2].Name
                        };

                        totalline.MarketList.Add(market2);
                        SendOddsMessage(totalline, gameId, formattedMarketList);
                    }
                }

                if (totallineAltInactive.MarketList.Count > 0 && totallineAlt.MarketList.Count == 0)
                {
                    SendOddsMessage2(SetMarketInactive(totallineAltInactive), gameId);
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
                            Name = _marketList[id2].Name
                        };

                        totalline.MarketList.Add(market2);
                        if (!cTotal)
                        {
                            SendOddsMessage2(totalline, gameId);
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
                    Name = _marketList[id].Name,
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "ATTL" && !s.Target.IsEqualToZero() && s.MarketRunnerList[0].Probability > 0.035 && s.MarketRunnerList[0].Probability < 0.965)
                };

                if (awayTotallineAlt.MarketList.Count > 0)
                {
                    Markets activeMarkets = SetMarketActiveNew(awayTotallineAlt, true, gameState);
                    SendOddsMessage(activeMarkets, gameId, formattedMarketList);
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
                            Name = _marketList[id2].Name
                        };

                        awayTotalline.MarketList.Add(market3);
                        SendOddsMessage(awayTotalline, gameId, formattedMarketList);
                    }
                }

                Markets awayTotallineAltInactive = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id].Name,
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "ATTL" && !s.Target.IsEqualToZero() && (s.MarketRunnerList[0].Probability < 0.035 ||
                        s.MarketRunnerList[0].Probability > 0.965))
                };

                if (awayTotallineAltInactive.MarketList.Count > 0 && awayTotallineAlt.MarketList.Count == 0)
                {
                    SendOddsMessage2(SetMarketInactive(awayTotallineAltInactive), gameId);
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
                            Name = _marketList[id2].Name
                        };

                        awayTotalline.MarketList.Add(market3);
                        if (!cAwayTotal)
                        {
                            SendOddsMessage2(awayTotalline, gameId);
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
                    Name = _marketList[id].Name,
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "HTTL" && !s.Target.IsEqualToZero() && s.MarketRunnerList[0].Probability > 0.035 && s.MarketRunnerList[0].Probability < 0.965)
                };

                if (homeTotallineAlt.MarketList.Count > 0)
                {
                    Markets activeMarkets = SetMarketActiveNew(homeTotallineAlt, true, gameState);
                    SendOddsMessage(activeMarkets, gameId, formattedMarketList);
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
                            Name = _marketList[id2].Name
                        };

                        homeTotalline.MarketList.Add(market4);
                        SendOddsMessage(homeTotalline, gameId, formattedMarketList);
                    }
                }

                //inactive
                Markets homeTotallineAltInactive = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id].Name,
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "HTTL" && !s.Target.IsEqualToZero() && (s.MarketRunnerList[0].Probability < 0.035 ||
                        s.MarketRunnerList[0].Probability > 0.965))
                };

                if (homeTotallineAltInactive.MarketList.Count > 0 && homeTotallineAlt.MarketList.Count == 0)
                {
                    SendOddsMessage2(SetMarketInactive(homeTotallineAltInactive), gameId);
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
                            Name = _marketList[id2].Name
                        };

                        homeTotalline.MarketList.Add(market4);
                        if (!cHomeTotal)
                        {
                            SendOddsMessage2(homeTotalline, gameId);
                        }
                    }
                }
                /*
                //------------------------to do send center line-----------------------------------------------

                id = _totalLines3W[key];

                Markets totalline3W = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = _totalLines3W[key],
                    Name = _marketList[id].Name,
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "TL3W" && !s.Target.IsEqualToZero() && s.MarketRunnerList[0].Probability > 0.035 &&
                        s.MarketRunnerList[0].Probability < 0.965)
                };

                if (totalline3W.MarketList.Count > 0)
                {
                    Markets activeMarkets = SetMarketActiveNew(totalline3W, true, gameState);
                    SendOddsMessage(activeMarkets, gameId, formattedMarketList);
                }


                Markets totalline3WInactive = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = _totalLines3W[key],
                    Name = _marketList[id].Name,
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "TL3W" && !s.Target.IsEqualToZero() && (s.MarketRunnerList[0].Probability < 0.035 ||
                        s.MarketRunnerList[0].Probability > 0.965))
                };

                if (totalline3WInactive.MarketList.Count > 0)
                {
                    SendOddsMessage2(SetMarketInactive(totalline3WInactive), gameId);
                }

                //------------------------------------------------------------------------

                id = _altRunLines3W[key];

                Markets handicap3W = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id].Name,
                    MarketList = marketList.FindAll(s => s.Tp == "SP3W" && !s.Target.IsEqualToZero() && s.MarketRunnerList[0].Probability > 0.035 &&
                                                         s.MarketRunnerList[0].Probability < 0.965)
                };

                if (handicap3W.MarketList.Count > 0)
                {
                    Markets activeMarkets = SetMarketActiveNew(handicap3W, true, gameState);
                    SendOddsMessage(activeMarkets, gameId, formattedMarketList);
                }

                Markets handicap3WInactive = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id].Name,
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "SP3W" && !s.Target.IsEqualToZero() && (s.MarketRunnerList[0].Probability < 0.035 ||
                        s.MarketRunnerList[0].Probability > 0.965))
                };

                if (handicap3WInactive.MarketList.Count > 0 && handicap3W.MarketList.Count == 0)
                {
                    SendOddsMessage2(SetMarketInactive(handicap3WInactive), gameId);
                }
                */
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

        private void SendOddsMessage(Markets markets, Guid gameId, List<Markets> formattedMarketList)
        {
            if (markets.MarketList.Count == 0)
            {
                return;
            }

            const string eventName = "NHL";
            string message = JsonConvert.SerializeObject(markets);
            string key = $"{markets.Period}{markets.Id}{gameId}";

            try
            {
                _pusherUtil.SendOddsMessage(message, eventName, key);
                formattedMarketList.Add(markets);

            }
            catch (Exception exception)
            {
                Logger.Error($"SendOddsMessage() exception = {exception}");
                throw;
            }
        }

        private void SendOddsMessage2(Markets markets, Guid gameId)
        {
            //send odds message without updating the formatted list

            if (markets.MarketList.Count == 0)
            {
                return;
            }

            const string eventName = "NHL";
            string message = JsonConvert.SerializeObject(markets);
            string key = $"{markets.Period}{markets.Id}{gameId}_susp";

            try
            {
                _pusherUtil.SendOddsMessage(message, eventName, key);
            }
            catch (Exception exception)
            {
                Logger.Error($"SendOddsMessage() exception = {exception} **********************");
            }
        }

        private static Markets SetMarketActive(Markets markets)
        {
            foreach (Market market in markets.MarketList)
            {
                market.Active = true;
            }

            return markets;
        }

        private static Markets SetMarketInactive(Markets markets)
        {
            foreach (Market market in markets.MarketList)
            {
                market.Active = false;
            }

            return markets;
        }

        private static bool CheckActive(string periodStr, Dictionary<string, double> gameState, bool active)
        {
            return true;
        }

        private static Markets SetMarketActiveNew(Markets markets, bool active, Dictionary<string, double> gameState)
        {
         
            return markets;
        }
    }
}
