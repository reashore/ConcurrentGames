using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Newtonsoft.Json;
using SportsIq.Models.Markets;
using SportsIq.NoSqlDataAccess;
using SportsIq.Pusher;
using SportsIq.SqlDataAccess.Ncaafb;
using SportsIq.Utilities;

namespace SportsIq.Distributor.Ncaafb
{
    public interface IDistributorNcaafb
    {
        void SendMarkets(Dictionary<int, List<Market>> periodMarketsDictionary, Guid gameId, string match);
    }

    public class DistributorNcaafb : IDistributorNcaafb
    {
        private static readonly ILog Logger;
        private readonly Dictionary<int, string> _marketList;
        private readonly IDatastore _datastore;
        private readonly IPusherUtil _pusherUtil;
        private List<string> _periods;

        /*****************************/
        private List<int> _moneyLines;
        private List<int> _moneyLines3W;
        /*****************************/
        private List<int> _spreadLines;
        private List<int> _altSpreadLines;
        /*****************************/
        private List<int> _spread3WLines;
        private List<int> _altSpread3WLines;
        /*****************************/
        private List<int> _totalLines;
        private List<int> _altTotalLines;
        /*****************************/
        private List<int> _totalLines3W;
        private List<int> _altTotalLines3W;
        /*****************************/
        private List<int> _aTeamTotalLines;
        private List<int> _altATeamTotalLines;
        /*****************************/
        private List<int> _aTeamTotal3WLines;
        private List<int> _altATeamTotal3WLines;
        /*****************************/
        private List<int> _hTeamTotalLines;
        private List<int> _altHTeamTotalLines;

        /*****************************/
        private List<int> _hTeamTotal3WLines;
        private List<int> _altHTeamTotal3WLines;

        static DistributorNcaafb()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(DistributorNcaafb));
        }

        public DistributorNcaafb(IDataAccessNcaafb dataAccessNcaafb, IDatastore datastore, IPusherUtil pusherUtil)
        {
            _datastore = datastore;
            _pusherUtil = pusherUtil;
            _marketList = dataAccessNcaafb.GetMarkets();
            InitializeLists();
        }

        private void InitializeLists()
        {
            _periods = new List<string> { "", "CG", "H1", "H2", "Q1", "Q2", "Q3", "Q4" };

            _moneyLines = new List<int> { 0, 95, 100, 162, 105, 110, 115, 120 };
            _spreadLines = new List<int> { 0, 96, 101, 165, 106, 111, 116, 121 };
            _altSpreadLines = new List<int> { 0, 125, 127, 127, 129, 131, 133, 135 };  //todo add 2nd half alt spread
            _totalLines = new List<int> { 0, 97, 102, 164, 107, 112, 117, 122 };

            _altTotalLines = new List<int> { 0, 126, 128, 128, 130, 132, 134, 136 };   //todo add 2nd half alt totals
            _aTeamTotalLines = new List<int> { 0, 98, 103, 166, 108, 113, 118, 123 };
            _altATeamTotalLines = new List<int> { 0, 175, 176, 181, 177, 178, 179, 180 };  //alt
            _altATeamTotal3WLines = new List<int> { 0, 196, 197, 202, 198, 199, 200, 201 };
            _aTeamTotal3WLines = new List<int> { 0, 189, 190, 195, 191, 192, 193, 194 };
            _hTeamTotal3WLines = new List<int> { 0, 203, 204, 209, 205, 206, 207, 208 };
            _altHTeamTotal3WLines = new List<int> { 0, 210, 211, 216, 212, 213, 214, 215 };
            _hTeamTotalLines = new List<int> { 0, 99, 104, 167, 109, 114, 119, 124 };
            _altHTeamTotalLines = new List<int> { 0, 182, 183, 188, 184, 185, 186, 187 };
            _totalLines3W = new List<int> { 0, 217, 218, 225, 219, 220, 221, 222 };
            _altTotalLines3W = new List<int> { 0, 226, 227, 234, 228, 229, 230, 231 };
            _spread3WLines = new List<int> { 0, 242, 243, 248, 243, 245, 246, 247 };
            _altSpread3WLines = new List<int> { 0, 235, 236, 241, 237, 238, 239, 240 };
            _moneyLines3W = new List<int> { 0, 249, 250, 255, 251, 252, 253, 254 };
        }

        public void SendMarkets(Dictionary<int, List<Market>> periodMarketsDictionary, Guid gameId, string match)
        {
            if (periodMarketsDictionary.Count == 0)
            {
                return;
            }

            if (periodMarketsDictionary[1].Count == 0)
            {
                return;
            }

            List<Markets> formattedMarketList = new List<Markets>();

            foreach (KeyValuePair<int, List<Market>> keyValuePair in periodMarketsDictionary)
            {
                int key = keyValuePair.Key;
                List<Market> marketList = keyValuePair.Value;
                string period = _periods[key];

                //------------------------------------------------------------------------

                int id = _altSpreadLines[key];

                Markets handicapAlts = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "SP" && !s.Target.IsEqualToZero() && s.MarketRunnerList[0].Probability > 0.035 &&
                        s.MarketRunnerList[0].Probability < 0.965)
                };

                if (handicapAlts.MarketList.Count > 0)
                {
                    SendOddsMessage(SetMarketActive(handicapAlts), gameId, formattedMarketList);

                    // center line fix to create 1.5 run line standard for full game


                    Market market = handicapAlts.MarketList.OrderBy(v => v.Weight).First();

                    if (market != null)
                    {
                        int id2 = _spreadLines[key];
                        market.Active = true;

                        Markets handicap = new Markets
                        {
                            Game = gameId.ToString(),
                            Period = period,
                            Id = id2,
                            Name = _marketList[id2]
                        };

                        handicap.MarketList.Add(market);
                        SendOddsMessage(handicap, gameId, formattedMarketList);
                    }
                }

                Markets handicapAltsInactive = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "SP" && !s.Target.IsEqualToZero() && (s.MarketRunnerList[0].Probability < 0.035 ||
                        s.MarketRunnerList[0].Probability > 0.965))
                };

                if (handicapAltsInactive.MarketList.Count > 0)
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

                        int id2 = _spreadLines[key];

                        Markets handicap = new Markets
                        {
                            Game = gameId.ToString(),
                            Period = period,
                            Id = id2,
                            Name = _marketList[id2]
                        };

                        handicap.MarketList.Add(market);
                        SendOddsMessage2(handicap, gameId);
                    }
                }

                //------------------------------------------------------------------------

                id = _moneyLines[key];

                Markets moneyline = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id]
                };

                Market market1 = marketList.Find(s =>
                    s.Tp == "ML" && s.Target.IsEqualToZero() && s.MarketRunnerList[0].Probability > 0.035 &&
                    s.MarketRunnerList[0].Probability < 0.965);

                if (market1 != null)
                {
                    market1.Active = true;
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

                //------------------------------------------------------------------------

                id = _altTotalLines[key];

                Markets totallineAlt = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "TL" && !s.Target.IsEqualToZero() && s.MarketRunnerList[0].Probability > 0.035 &&
                        s.MarketRunnerList[0].Probability < 0.965)
                };

                Markets totallineAltInactive = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "TL" && !s.Target.IsEqualToZero() && (s.MarketRunnerList[0].Probability < 0.035 ||
                        s.MarketRunnerList[0].Probability > 0.965))
                };



                if (totallineAlt.MarketList.Count > 0)
                {
                    SendOddsMessage(SetMarketActive(totallineAlt), gameId, formattedMarketList);
                    Market market2 = totallineAlt.MarketList.OrderBy(v => v.Weight).First();

                    if (market2 != null)
                    {
                        int id2 = _totalLines[key];
                        market2.Active = true;
                        Markets totalline = new Markets
                        {
                            Game = gameId.ToString(),
                            Period = period,
                            Id = id2,
                            Name = _marketList[id2]
                        };

                        totalline.MarketList.Add(market2);
                        SendOddsMessage(totalline, gameId, formattedMarketList);
                    }
                }

                if (totallineAltInactive.MarketList.Count > 0)
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
                            Name = _marketList[id2]
                        };

                        totalline.MarketList.Add(market2);
                        SendOddsMessage2(totalline, gameId);
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
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "ATTL" && !s.Target.IsEqualToZero() && s.MarketRunnerList[0].Probability > 0.035 && s.MarketRunnerList[0].Probability < 0.965)
                };

                if (awayTotallineAlt.MarketList.Count > 0)
                {
                    SendOddsMessage(SetMarketActive(awayTotallineAlt), gameId, formattedMarketList);
                    Market market3 = awayTotallineAlt.MarketList.OrderBy(v => v.Weight).First();

                    if (market3 != null)
                    {
                        int id2 = _aTeamTotalLines[key];
                        market3.Active = true;
                        Markets awayTotalline = new Markets
                        {
                            Game = gameId.ToString(),
                            Period = period,
                            Id = id2,
                            Name = _marketList[id2]
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
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "ATTL" && !s.Target.IsEqualToZero() && (s.MarketRunnerList[0].Probability < 0.035 ||
                        s.MarketRunnerList[0].Probability > 0.965))
                };

                if (awayTotallineAltInactive.MarketList.Count > 0)
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
                            Name = _marketList[id2]
                        };

                        awayTotalline.MarketList.Add(market3);
                        SendOddsMessage2(awayTotalline, gameId);
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
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "HTTL" && !s.Target.IsEqualToZero() && s.MarketRunnerList[0].Probability > 0.035 && s.MarketRunnerList[0].Probability < 0.965)
                };

                if (homeTotallineAlt.MarketList.Count > 0)
                {
                    SendOddsMessage(SetMarketActive(homeTotallineAlt), gameId, formattedMarketList);
                    Market market4 = homeTotallineAlt.MarketList.OrderBy(v => v.Weight).First();

                    if (market4 != null)
                    {
                        int id2 = _hTeamTotalLines[key];
                        market4.Active = true;
                        Markets homeTotalline = new Markets
                        {
                            Game = gameId.ToString(),
                            Period = period,
                            Id = id2,
                            Name = _marketList[id2]
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
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "HTTL" && !s.Target.IsEqualToZero() && (s.MarketRunnerList[0].Probability < 0.035 ||
                        s.MarketRunnerList[0].Probability > 0.965))
                };

                if (homeTotallineAltInactive.MarketList.Count > 0)
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
                            Name = _marketList[id2]
                        };

                        homeTotalline.MarketList.Add(market4);
                        SendOddsMessage2(homeTotalline, gameId);
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
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "TL3W" && !s.Target.IsEqualToZero() && s.MarketRunnerList[0].Probability > 0.035 &&
                        s.MarketRunnerList[0].Probability < 0.965)
                };

                if (totalline3W.MarketList.Count > 0)
                {
                    SendOddsMessage(SetMarketActive(totalline3W), gameId, formattedMarketList);
                }


                Markets totalline3WInactive = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = _totalLines3W[key],
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "TL3W" && !s.Target.IsEqualToZero() && (s.MarketRunnerList[0].Probability < 0.035 ||
                        s.MarketRunnerList[0].Probability > 0.965))
                };

                if (totalline3WInactive.MarketList.Count > 0)
                {
                    SendOddsMessage2(SetMarketInactive(totalline3WInactive), gameId);
                }

                //------------------------------------------------------------------------

                id = _spread3WLines[key];

                Markets handicap3W = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s => s.Tp == "SP3W" && !s.Target.IsEqualToZero() && s.MarketRunnerList[0].Probability > 0.035 &&
                                                         s.MarketRunnerList[0].Probability < 0.965)
                };

                if (handicap3W.MarketList.Count > 0)
                {
                    SendOddsMessage(handicap3W, gameId, formattedMarketList);
                }

                Markets handicap3WInactive = new Markets
                {
                    Game = gameId.ToString(),
                    Period = period,
                    Id = id,
                    Name = _marketList[id],
                    MarketList = marketList.FindAll(s =>
                        s.Tp == "SP3W" && !s.Target.IsEqualToZero() && (s.MarketRunnerList[0].Probability < 0.035 ||
                        s.MarketRunnerList[0].Probability > 0.965))
                };

                if (handicap3WInactive.MarketList.Count > 0)
                {
                    SendOddsMessage2(handicap3WInactive, gameId);
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

        // todo move these these functions to DistributorBase
        // todo add eventName as parameter
        private void SendOddsMessage(Markets markets, Guid gameId, List<Markets> formattedMarketList)
        {
            if (markets.MarketList.Count == 0)
            {
                return;
            }

            const string eventName = "NFL";
            string message = JsonConvert.SerializeObject(markets);
            string key = $"{markets.Period}_{markets.Id}_{gameId}";

            _pusherUtil.SendOddsMessage(message, eventName, key);
            formattedMarketList.Add(markets);
        }

        private void SendOddsMessage2(Markets markets, Guid gameId)
        {
            //send odds message without updating the formatted list

            if (markets.MarketList.Count == 0)
            {
                return;
            }

            const string eventName = "NFL";
            string message = JsonConvert.SerializeObject(markets);
            string key = $"{markets.Period}{markets.Id}{gameId}";

            try
            {
                _pusherUtil.SendOddsMessage(message, eventName, key);
            }
            catch (Exception exception)
            {
                Logger.Error($"Pusher threw exception = {exception} **********************");
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
    }
}
