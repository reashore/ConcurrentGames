using log4net;
using Newtonsoft.Json;
using SportsIq.Models.Markets;
using SportsIq.NoSqlDataAccess;
using SportsIq.Pusher;
using SportsIq.SqlDataAccess.Wnba;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Convert;

namespace SportsIq.Distributor.Wnba
{
    public interface IDistributorWnba
    {
        void SendMarkets(Dictionary<int, List<Market>> periodMarketsDictionary, Guid gameId, string match, Dictionary<string, double> gameState);
    }

    public class DistributorWnba : DistributorBase, IDistributorWnba
    {
        //private static readonly ILog Logger;
        //private readonly IPusherUtil _pusherUtil;
        private readonly IDatastore _datastore;

        private readonly Dictionary<int, string> _marketList;
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
        //private List<int> _aTeamTotal3WLines;
        private List<int> _altATeamTotal3WLines;
        /*****************************/
        private List<int> _hTeamTotalLines;
        private List<int> _altHTeamTotalLines;
        /*****************************/
        //private List<int> _hTeamTotal3WLines;
        private List<int> _altHTeamTotal3WLines;

        private List<int> _winningMarginR;
        private List<int> _winningMarginOt;

        private List<int> _comb6;
        private List<int> _comb4;

        static DistributorWnba()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(DistributorWnba));
        }

        public DistributorWnba(IDataAccessWnba dataAccessWnba, IDatastore datastore, IPusherUtil pusherUtil)
        {
            _datastore = datastore;
            PusherUtil = pusherUtil;
            _marketList = dataAccessWnba.GetMarkets();
            InitializeLists();
        }

        private void InitializeLists()
        {
            _periods = new List<string> { "", "CG", "H1", "H2", "Q1", "Q2", "Q3", "Q4" };

            //moneylines
            _moneyLines = new List<int> { 0, 95, 105, 114, 122, 132, 142, 152 };
            _spreadLines = new List<int> { 0, 96, 107, 115, 124, 134, 144, 154 };
            _altSpreadLines = new List<int> { 0, 217, 173, 174, 175, 176, 177, 178 };
            _totalLines = new List<int> { 0, 97, 108, 116, 125, 135, 145, 155 };
            _altTotalLines = new List<int> { 0, 208, 179, 180, 181, 182, 183, 184 };
            _aTeamTotalLines = new List<int> { 0, 101, 111, 119, 128, 138, 148, 158 };
            _altATeamTotalLines = new List<int> { 0, 198, 199, 185, 186, 187, 188, 189 };
            _altATeamTotal3WLines = new List<int> { 0, 237, 238, 232, 233, 234, 235, 236 };
            _altHTeamTotal3WLines = new List<int> { 0, 244, 245, 239, 240, 241, 242, 243 };
            _hTeamTotalLines = new List<int> { 0, 102, 112, 120, 129, 139, 149, 159 };
            _altHTeamTotalLines = new List<int> { 0, 195, 196, 190, 191, 192, 193, 194 };
            _totalLines3W = new List<int> { 0, 100, 110, 118, 127, 137, 147, 157 };
            _altTotalLines3W = new List<int> { 0, 201, 202, 203, 204, 205, 206, 207 };
            _spread3WLines = new List<int> { 0, 99, 109, 117, 126, 136, 146, 156 };
            _moneyLines3W = new List<int> { 0, 98, 106, 216, 123, 133, 143, 153 };
            _altSpread3WLines = new List<int> { 0, 209, 210, 211, 212, 213, 214, 215 };
            _winningMarginR = new List<int> { 0, 218, 219, 220, 221, 222, 223, 224 };
            _winningMarginOt = new List<int> { 0, 225, 226, 227, 228, 229, 230, 231 };
            _comb6 = new List<int> { 0, 172, 165, 246, 166, 167, 168, 247 };
            _comb4 = new List<int> { 0, 163, 164, 248, 169, 170, 171, 249 };
        }

        public void SendMarkets(Dictionary<int, List<Market>> periodMarketsDictionary, Guid gameId, string match, Dictionary<string, double> gameState)
        {
            // todo divide into smaller functions?
            if (periodMarketsDictionary.Count == 0)
            {
                Logger.Info("SendMarkets(): periodMarketsDictionary.Count is zero");
                return;
            }

            if (periodMarketsDictionary[1].Count == 0)
            {
                Logger.Info("SendMarkets(): periodMarketsDictionary[1].Count is zero");
                return;
            }
            
            const string eventName = "WNBATEAM";
            List<Markets> formattedMarketList = new List<Markets>();

            foreach (KeyValuePair<int, List<Market>> keyValuePair in periodMarketsDictionary)
            {
                try
                {
                    int key = keyValuePair.Key;
                    List<Market> marketList = keyValuePair.Value;
                    string period = _periods[key];

                    //------------------------------------- 2 way spread -----------------------------------

                    int id = _altSpreadLines[key];

                    Markets handicapAlts = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectInsideLimitsWithNonZeroTarget(s, "SP"))
                    };

                    if (handicapAlts.MarketList.Count > 0)
                    {
                        Markets activeHandicapAlts = SetMarketActiveNew(handicapAlts, true, gameState);
                        SendOddsMessage(activeHandicapAlts, gameId, formattedMarketList, eventName);

                        if (handicapAlts.MarketList.Count > 0)
                        {
                            Market market = handicapAlts.MarketList.OrderBy(v => v.Weight).First();

                            if (market != null)
                            {
                                int id2 = _spreadLines[key];
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

                    //inactive
                    Markets handicapAltsInactive = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectOutsideLimitsWithNonZeroTarget(s, "SP"))
                    };

                    if (handicapAltsInactive.MarketList.Count > 0)
                    {
                        Markets inActiveHandicapAlts = SetMarketInactive(handicapAlts);
                        SendOddsMessage2(inActiveHandicapAlts, gameId, eventName);

                        if (handicapAlts.MarketList.Count == 0)
                        {
                            Market market = handicapAltsInactive.MarketList.OrderBy(v => v.Weight).First();

                            if (market != null)
                            {
                                int id2 = _spreadLines[key];
                                market.Active = false;

                                Markets handicap = new Markets
                                {
                                    Game = gameId.ToString(),
                                    Period = period,
                                    Id = id2,
                                    Name = _marketList[id2]
                                };

                                handicap.MarketList.Add(market);
                                SendOddsMessage2(handicap, gameId, eventName);
                            }
                        }
                    }

                    //--------------------------------------------- 2 way moneyline ---------------------------

                    id = _moneyLines[key];

                    Markets moneyline = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id]
                    };

                    Market market1 = marketList.Find(s => SelectInsideLimitsWithZeroTarget(s, "ML"));

                    if (market1 != null)
                    {
                        market1.Active = CheckActive(period, gameState, true);
                        market1.Tp = "ML";
                        moneyline.MarketList.Add(market1);
                        SendOddsMessage(moneyline, gameId, formattedMarketList, eventName);
                    }
                    else
                    {  //inactive
                        Market market = new Market
                        {
                            Active = false,
                            Tp = "ML"
                        };
                        moneyline.MarketList.Add(market);
                        SendOddsMessage2(moneyline, gameId, eventName);
                    }

                    //----------------------------------------- 2 way total -------------------------------

                    id = _altTotalLines[key];

                    Markets totallineAlt = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectInsideLimitsWithNonZeroTarget(s, "TL"))
                    };

                    if (totallineAlt.MarketList.Count > 0)
                    {
                        Markets activeTotallineAlt = SetMarketActiveNew(totallineAlt, true, gameState); //SetMarketActive(totallineAlt);
                        SendOddsMessage(activeTotallineAlt, gameId, formattedMarketList, eventName);
                        Market market2 = totallineAlt.MarketList.OrderBy(v => v.Weight).First();

                        if (market2 != null)
                        {
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

                    Markets totallineAltInactive = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectOutsideLimitsWithNonZeroTarget(s, "TL"))
                    };

                    if (totallineAltInactive.MarketList.Count > 0)
                    {
                        Markets activeTotallineAlt = SetMarketInactive(totallineAltInactive);
                        SendOddsMessage(activeTotallineAlt, gameId, formattedMarketList, eventName);

                        if (totallineAlt.MarketList.Count == 0)
                        {
                            Market market2 = totallineAltInactive.MarketList.OrderBy(v => v.Weight).First();

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
                                SendOddsMessage(totalline, gameId, formattedMarketList, eventName);
                            }
                        }
                    }

                    //------------------------------------------------2 way away team total ------------------------

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
                        Markets activeAwayTotallineAlt = SetMarketActiveNew(awayTotallineAlt, true, gameState); //SetMarketActive(awayTotallineAlt);
                        SendOddsMessage(activeAwayTotallineAlt, gameId, formattedMarketList, eventName);
                        Market market3 = awayTotallineAlt.MarketList.OrderBy(v => v.Weight).First();

                        if (market3 != null)
                        {
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

                    if (awayTotallineAltInactive.MarketList.Count > 0)
                    {
                        Markets activeAwayTotallineAlt = SetMarketInactive(awayTotallineAltInactive);
                        SendOddsMessage2(activeAwayTotallineAlt, gameId, eventName);

                        if (awayTotallineAlt.MarketList.Count == 0)
                        {
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
                                SendOddsMessage2(awayTotalline, gameId, eventName);
                            }
                        }
                    }

                    //--------------------------------------------2 way home team total ----------------------------

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
                        Markets activeHomeTotallineAlt = SetMarketActiveNew(homeTotallineAlt, true, gameState);
                        //SetMarketActive(homeTotallineAlt);
                        SendOddsMessage(activeHomeTotallineAlt, gameId, formattedMarketList, eventName);
                        Market market4 = homeTotallineAlt.MarketList.OrderBy(v => v.Weight).First();

                        if (market4 != null)
                        {
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

                    Markets homeTotallineAltInactive = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectOutsideLimitsWithNonZeroTarget(s, "HTTL"))
                    };

                    if (homeTotallineAltInactive.MarketList.Count > 0)
                    {
                        Markets activeHomeTotallineAlt = SetMarketInactive(homeTotallineAltInactive);
                        SendOddsMessage2(activeHomeTotallineAlt, gameId, eventName);

                        if (homeTotallineAlt.MarketList.Count == 0)
                        {
                            Market market3 = homeTotallineAltInactive.MarketList.OrderBy(v => v.Weight).First();

                            if (market3 != null)
                            {
                                int id2 = _hTeamTotalLines[key];
                                market3.Active = false;
                                Markets homeTotalline = new Markets
                                {
                                    Game = gameId.ToString(),
                                    Period = period,
                                    Id = id2,
                                    Name = _marketList[id2]
                                };

                                homeTotalline.MarketList.Add(market3);
                                SendOddsMessage2(homeTotalline, gameId, eventName);
                            }
                        }
                    }

                    //-------------------------------------three way total -----------------------------------

                    id = _totalLines3W[key];

                    Markets totalline3W = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = _totalLines3W[key],
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectInsideLimitsWithNonZeroTarget(s, "TL3W"))
                    };

                    Markets activeTotalline3W = SetMarketActiveNew(totalline3W, true, gameState);  //SetMarketActive(totalline3W);
                    SendOddsMessage(activeTotalline3W, gameId, formattedMarketList, eventName);

                    Markets totalline3WInactive = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = _totalLines3W[key],
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectOutsideLimitsWithNonZeroTarget(s, "TL3W"))
                    };

                    activeTotalline3W = SetMarketInactive(totalline3WInactive);
                    SendOddsMessage2(activeTotalline3W, gameId, eventName);

                    //-------------------------------------three way away team total -----------------------------------

                    id = _altATeamTotal3WLines[key];

                    Markets awayTotalline3W = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = _totalLines3W[key],
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectInsideLimitsWithNonZeroTarget(s, "ATTL3W"))
                    };

                    Markets activeAwayTotalline3W = SetMarketActive(awayTotalline3W);
                    SendOddsMessage(activeAwayTotalline3W, gameId, formattedMarketList, eventName);

                    Markets awayTotalline3WInactive = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = _totalLines3W[key],
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectOutsideLimitsWithNonZeroTarget(s, "ATTL3W"))
                    };

                    activeAwayTotalline3W = SetMarketInactive(awayTotalline3WInactive);
                    SendOddsMessage(activeAwayTotalline3W, gameId, formattedMarketList, eventName);

                    //-------------------------------------three way home team total -----------------------------------

                    id = _altHTeamTotal3WLines[key];

                    Markets homeTotalline3W = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = _totalLines3W[key],
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectInsideLimitsWithNonZeroTarget(s, "HTTL3W"))
                    };

                    Markets activeHomeTotalline3W = SetMarketActiveNew(homeTotalline3W, true, gameState);
                    //SetMarketActive(homeTotalline3W);
                    SendOddsMessage(activeHomeTotalline3W, gameId, formattedMarketList, eventName);

                    Markets homeTotalline3WInactive = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = _totalLines3W[key],
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectOutsideLimitsWithNonZeroTarget(s, "HTTL3W"))
                    };

                    activeHomeTotalline3W = SetMarketActiveNew(homeTotalline3WInactive, true, gameState);
                    SendOddsMessage2(activeHomeTotalline3W, gameId, eventName);

                    //--------------------------------------- three way spread ---------------------------------

                    id = _spread3WLines[key];

                    Markets handicap3W = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id],
                        // todo why is the target check missing? !s.Target.IsEqualToZero()
                        MarketList = marketList.FindAll(s =>
                            s.Tp == "SP3W" && s.MarketRunnerList[0].Probability > 0.035 &&
                                                             s.MarketRunnerList[0].Probability < 0.965)
                        //MarketList = marketList.FindAll(s => s.Tp == "SP3W" && s.MarketRunnerList[0].Probability > 0.035 &&
                        //                                     s.MarketRunnerList[0].Probability < 0.965)
                    };
                    Markets activehandicap3W = SetMarketActiveNew(handicap3W, true, gameState);
                    SendOddsMessage(activehandicap3W, gameId, formattedMarketList, eventName);

                    Markets handicap3WInactive = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectOutsideLimitsWithoutTarget(s, "SP3W"))
                    };

                    activehandicap3W = SetMarketInactive(handicap3WInactive);
                    SendOddsMessage2(activehandicap3W, gameId, eventName);

                    //--------------------------------------- three way money ---------------------------------

                    id = _moneyLines3W[key];

                    Markets moneyLine3W = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectInsideLimitsWithoutTarget(s, "ML3"))
                    };

                    Markets activeMoneyLine3W = SetMarketActiveNew(moneyLine3W, true, gameState); //SetMarketActive(moneyLine3W);
                    SendOddsMessage(activeMoneyLine3W, gameId, formattedMarketList, eventName);

                    Markets moneyLine3WInactive = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectOutsideLimitsWithoutTarget(s, "ML3"))
                    };

                    activeMoneyLine3W =  SetMarketActiveNew(moneyLine3WInactive, true, gameState);
                    SendOddsMessage2(activeMoneyLine3W, gameId, eventName);

                    //--------------------------------------------winning margin REG ----------------------------

                    id = _winningMarginR[key];

                    Markets winningMargin = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectInsideLimitsWithoutTarget(s, "WMR"))
                    };
                    Markets activeWinningMargin = SetMarketActiveNew(winningMargin, true, gameState);
                    SendOddsMessage(activeWinningMargin, gameId, formattedMarketList, eventName);

                    Markets winningMarginInactive = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectOutsideLimitsWithoutTarget(s, "WMP"))
                    };
                    activeWinningMargin = SetMarketInactive(winningMarginInactive);
                    SendOddsMessage2(activeWinningMargin, gameId, eventName);

                    //--------------------------------------------winning margin CG ----------------------------

                    id = _winningMarginOt[key];

                    Markets winningMarginCg = new Markets  // CG BECAUSE ITS C (complete) and G(game) 
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectInsideLimitsWithoutTarget(s, "WMCG"))
                    };
                    Markets activewinningMarginCg = SetMarketActiveNew(winningMarginCg, true, gameState); //SetMarketActive(winningMarginCg);
                    SendOddsMessage(activewinningMarginCg, gameId, formattedMarketList, eventName);

                    Markets winningMarginCgInactive = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectOutsideLimitsWithoutTarget(s, "WMCG"))
                    };

                    activewinningMarginCg = SetMarketInactive(winningMarginCgInactive);
                    SendOddsMessage(activewinningMarginCg, gameId, formattedMarketList, eventName);

                    //--------------------------------------------combo 6 ----------------------------

                    id = _comb6[key];

                    Markets winnerTotal = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectOutsideLimitsWithoutTarget(s, "C6W"))
                    };
                    Markets activeWinnerTotal = SetMarketActiveNew(winnerTotal, true, gameState); //SetMarketActive(winnerTotal);
                    SendOddsMessage(activeWinnerTotal, gameId, formattedMarketList, eventName);

                    Markets winnerTotalInactive = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectOutsideLimitsWithoutTarget(s, "C6W"))
                    };
                    activeWinnerTotal = SetMarketInactive(winnerTotal);
                    SendOddsMessage2(activeWinnerTotal, gameId, eventName);

                    //--------------------------------------------combo 4 ----------------------------

                    id = _comb4[key];

                    Markets spreadTotal = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectInsideLimitsWithoutTarget(s, "C4W"))
                    };
                    Markets activeSpreadTotal = SetMarketActiveNew(spreadTotal, true, gameState); //SetMarketActive(spreadTotal);
                    SendOddsMessage(activeSpreadTotal, gameId, formattedMarketList, eventName);

                    // todo why is this unused? Typo?
                    Markets spreadTotalInactive = new Markets
                    {
                        Game = gameId.ToString(),
                        Period = period,
                        Id = id,
                        Name = _marketList[id],
                        MarketList = marketList.FindAll(s => SelectInsideLimitsWithoutTarget(s, "C4W"))
                    };

                    activeSpreadTotal = SetMarketInactive(spreadTotalInactive);
                    SendOddsMessage2(activeSpreadTotal, gameId, eventName);
                }
                catch (Exception exception)
                {
                    Logger.Error(exception);
                }
            }

            try
            {
                FormattedGame formattedGame = new FormattedGame
                {
                    Id = gameId.ToString(),
                    Match = match,
                    MarketList = formattedMarketList
                };

                string formattedGameJsonString = JsonConvert.SerializeObject(formattedGame);
                _datastore.AddFinishedOdds(gameId, formattedGameJsonString);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }
        }

        private static bool CheckActive(string periodString, Dictionary<string, double> gameState, bool active)
        {
            int seconds = ToInt32(gameState["S"]);
            const int takedownTime = 120;
            const int gameSeconds = 2400;
            const int quater = gameSeconds / 4;

            switch (periodString)
            {
                case "CG":
                    active = seconds > takedownTime;
                    break;

                case "H1":
                    active = seconds > gameSeconds / 2 + takedownTime;
                    break;

                case "H2":
                    active = seconds > takedownTime;
                    break;

                case "Q1":
                    active = seconds > gameSeconds - quater + takedownTime;
                    break;

                case "Q2":
                    active = seconds > gameSeconds - gameSeconds / 2 + takedownTime;
                    break;

                case "Q3":
                    active = seconds > quater + takedownTime;
                    break;

                case "Q4":
                    active = seconds > takedownTime;
                    break;
            }

            return active;
        }

        private static Markets SetMarketActiveNew(Markets markets, bool active, Dictionary<string, double> gameState)
        {
            int seconds = ToInt32(gameState["S"]);
            const int takedownTime = 120;
            const int gameSeconds = 2400;
            const int quater = gameSeconds / 4;

            switch (markets.Period)
            {
                case "CG":
                    active = seconds > takedownTime;
                    break;

                case "H1":
                    active = seconds > gameSeconds / 2 + takedownTime;
                    break;

                case "H2":
                    active = seconds > takedownTime;
                    break;

                case "Q1":
                    active = seconds > gameSeconds - quater + takedownTime;
                    break;

                case "Q2":
                    active = seconds > gameSeconds - gameSeconds / 2 + takedownTime;
                    break;

                case "Q3":
                    active = seconds > quater + takedownTime;
                    break;

                case "Q4":
                    active = seconds > takedownTime;
                    break;
            }

            foreach (Market market in markets.MarketList)
            {
                market.Active = active;
            }

            return markets;
        }
    }
}
