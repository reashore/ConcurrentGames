using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Google.Cloud.PubSub.V1;
using log4net;
using Newtonsoft.Json;
using SportsIq.Analytica.Nba;
using SportsIq.Distributor.Nba;
using SportsIq.Models.Constants.Nba;
using SportsIq.Models.Markets;
using SportsIq.Models.SportRadar.Nba.GameEvents;
using SportsIq.Models.SportRadar.Nba.GameInfo;
using SportsIq.NoSqlDataAccess;
using SportsIq.PubSub;
using SportsIq.Utilities;
using SportsIq.Pusher;
using SportsIq.SportsRadar.Nba;
using SportsIq.SqlDataAccess.Nba;
using static System.Convert;

namespace SportsIq.Games.Nba
{
    public class NbaGame : GameBase<Team, NbaGameInfo>, IGame
    {
        private static readonly ILog Logger;
        private readonly IDataAccessNba _dataAccessNba;
        private readonly IRadarNba _radarNba;
        private readonly IAnalyticaNba _analyticaNba;
        private readonly IDatastore _datastore;
        private readonly IDistributorNba _distributorNba;
        private readonly IPubSubUtil _pubSubUtil;
        private NbaGameState NbaGameState { get; }
        private readonly IPusherUtil _pusherUtil;
        private readonly List<MarketDescription> _marketList;

        static NbaGame()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(NbaGame));
        }

        public NbaGame(IDataAccessNba dataAccessNba, IRadarNba radarNba, IAnalyticaNba analyticaNba, IDatastore datastore, IDistributorNba distributorNba, IPubSubUtil pubSubUtil, IPusherUtil pusherUtil)
        {
            string isSimulationString = ConfigurationManager.AppSettings["isSimulation"];
            IsSimulation = ToBoolean(isSimulationString);
            GameTimeSeconds = 2880;
            PeriodList = new List<string> { "CG", "H1", "H2", "Q1", "Q2", "Q3", "Q4" };
            InitializePeriodScoring(PeriodList);

            ModelData[NbaModelDataKeys.InMlf] = new Dictionary<string, double>();
            ModelData[NbaModelDataKeys.Evs] = new Dictionary<string, double>();
            ModelData[NbaModelDataKeys.Egt] = new Dictionary<string, double>();

            if (!IsTeamMode)
            {
                ModelData[NbaModelDataKeys.EgtP] = new Dictionary<string, double>();
                //player
                ModelData[NbaModelDataKeys.Ttm] = new Dictionary<string, double>();
                ModelData[NbaModelDataKeys.Posc] = new Dictionary<string, double>();
                ModelData[NbaModelDataKeys.Potm] = new Dictionary<string, double>();
                ModelData[NbaModelDataKeys.Pop] = new Dictionary<string, double>();
                ModelData[NbaModelDataKeys.Psco] = new Dictionary<string, double>();
                ModelData[NbaModelDataKeys.Sdvtm] = new Dictionary<string, double>();
                ModelData[NbaModelDataKeys.Sdom] = new Dictionary<string, double>();
            }

            ModelData[NbaModelDataKeys.InLMlf] = new Dictionary<string, double>();
            ModelData[NbaModelDataKeys.InTsf] = new Dictionary<string, double>();
            ModelData[NbaModelDataKeys.InLsF] = new Dictionary<string, double>();
            ModelData[NbaModelDataKeys.Settings] = new Dictionary<string, double>();
            ModelData[NbaModelDataKeys.Adjust] = new Dictionary<string, double>();
            ModelData[NbaModelDataKeys.Gsc] = new Dictionary<string, double>();
            NbaGameState = new NbaGameState();

            _dataAccessNba = dataAccessNba;
            _analyticaNba = analyticaNba;
            _datastore = datastore;
            _distributorNba = distributorNba;
            _radarNba = radarNba;
            _pubSubUtil = pubSubUtil;
            _pusherUtil = pusherUtil;
            _marketList = dataAccessNba.GetMarketsDescriptions();
        }

        #region Radar Event Subscriptions and Handlers

        public void AddRadarGameEventHandler()
        {
            _radarNba.RadarGameEvent += HandleGameEvent;
        }

        public void RemoveRadarGameEventHandler()
        {
            _radarNba.RadarGameEvent -= HandleGameEvent;
        }

        public TimeSpan GetTimeSinceLastGameEventOrHeartbeat()
        {
            TimeSpan timeSinceLastGameEventOrHeartbeat = DateTime.Now - _radarNba.TimeOfLastRadarGameEventOrHeartbeat;
            return timeSinceLastGameEventOrHeartbeat;
        }

        private void HandleGameEvent(object sender, NbaGameEventEventArgs nbaGameEventEventArgs)
        {
            NbaGameEvent nbaGameEvent = nbaGameEventEventArgs.GameEvent;
            Guid gameId = nbaGameEvent.Payload.Game.Id;

            if (gameId == GameId)
            {
                string eventType = nbaGameEvent.Payload.Event.EventType;
                string operation = nbaGameEvent.Metadata.Operation;
                //string message = $"Handling Radar game event: GameId = {gameId}, EventType = {eventType}";
                //Logger.Info(message);

                switch (operation)
                {
                    case "delete":
                        return;
                }

                switch (eventType)
                {
                    // todo add specific event
                    default:
                        ProcessRadarGameEvent(nbaGameEvent);
                        ModelUpdateRequired = true;
                        break;
                }
            }
        }

        private void ProcessRadarGameEvent(NbaGameEvent nbaGameEvent)
        {
            try
            {
                if (nbaGameEvent.Metadata == null)
                {
                    return;
                }

                Dictionary<string, double> evsDictionary = ModelData[NbaModelDataKeys.Evs];

                // todo are these if statements necessary since we are writing and not reading?
                if (!evsDictionary.ContainsKey("H,Q1"))
                {
                    evsDictionary["V,Q1"] = 0;
                    evsDictionary["H,Q1"] = 0;
                }

                if (!evsDictionary.ContainsKey("H,H1"))
                {
                    evsDictionary["V,H1"] = 0;
                    evsDictionary["H,H1"] = 0;
                }

                if (!evsDictionary.ContainsKey("H,Q2"))
                {
                    evsDictionary["V,Q2"] = 0;
                    evsDictionary["H,Q2"] = 0;
                }

                if (!evsDictionary.ContainsKey("H,H2"))
                {
                    evsDictionary["V,H2"] = 0;
                    evsDictionary["H,H2"] = 0;
                }

                if (!evsDictionary.ContainsKey("H,Q3"))
                {
                    evsDictionary["V,Q3"] = 0;
                    evsDictionary["H,Q3"] = 0;
                }

                if (!evsDictionary.ContainsKey("H,Q4"))
                {
                    evsDictionary["V,Q4"] = 0;
                    evsDictionary["H,Q4"] = 0;
                }

                NbaGameState.Status = nbaGameEvent.Metadata.Status;
                Event payloadEvent = nbaGameEvent.Payload.Event;
                Game payloadGame = nbaGameEvent.Payload.Game;
                long sequence = payloadEvent.Sequence;

                if (sequence <= NbaGameState.Sequence)
                {
                    return;
                }

                NbaGameState.Sequence = sequence;
                NbaGameState.Period = $"Q{payloadEvent.Period.Sequence}";
                NbaGameState.PeriodNumber = payloadEvent.Period.Sequence;
                NbaGameState.Seconds =
                    Utils.ConvertPeriodToGameString(payloadEvent.Period.Sequence, payloadEvent.Clock, GameTimeSeconds);
                NbaGameState.Clock = payloadEvent.Clock;
                NbaGameState.AwayScore = payloadGame.Away.Points;
                NbaGameState.HomeScore = payloadGame.Home.Points;

                Dictionary<string, double> egtDictionary = ModelData[NbaModelDataKeys.Egt];
                egtDictionary["S"] = NbaGameState.Seconds;
                egtDictionary["F"] = NbaGameState.Foul;
                //egtDictionary["P"] = NbaGameState.Possession;

                int homeScore = NbaGameState.HomeScore;
                int awayScore = NbaGameState.AwayScore;
                evsDictionary["H,CG"] = homeScore;
                evsDictionary["V,CG"] = awayScore;

                switch (NbaGameState.PeriodNumber)
                {
                    case 1:
                        evsDictionary["V,Q1"] = awayScore;
                        evsDictionary["H,Q1"] = homeScore;
                        break;

                    case 2:
                        evsDictionary["V,Q2"] = awayScore - evsDictionary["V,Q1"];
                        evsDictionary["H,Q2"] = homeScore - evsDictionary["H,Q1"];
                        break;

                    case 3:
                        evsDictionary["V,Q3"] = awayScore - (evsDictionary["V,Q1"] + evsDictionary["V,Q2"]);
                        evsDictionary["H,Q3"] = homeScore - (evsDictionary["H,Q1"] + evsDictionary["H,Q2"]);
                        break;

                    case 4:
                        evsDictionary["V,Q4"] =
                            awayScore - (evsDictionary["V,Q1"] + evsDictionary["V,Q2"] + evsDictionary["V,Q3"]);
                        evsDictionary["H,Q4"] =
                            homeScore - (evsDictionary["H,Q1"] + evsDictionary["H,Q2"] + evsDictionary["H,Q3"]);
                        break;
                }

                evsDictionary["V,H1"] = evsDictionary["V,Q1"] + evsDictionary["V,Q2"];
                evsDictionary["H,H1"] = evsDictionary["H,Q1"] + evsDictionary["H,Q2"];

                evsDictionary["V,H2"] = evsDictionary["V,Q3"] + evsDictionary["V,Q4"];
                evsDictionary["H,H2"] = evsDictionary["H,Q3"] + evsDictionary["H,Q4"];

                string messageJson = $@"{{
                                        ""game"":           ""{GameId}"",
                                        ""away_score"":     {awayScore},
                                        ""home_score"":     {homeScore},
                                        ""period"":         ""{NbaGameState.Period}"",
                                        ""clock"":          ""{NbaGameState.Clock}"",
                                        ""possession"":     ""{NbaGameState.Possession}"",
                                        ""foul"":           {NbaGameState.Foul},
                                        ""pause"":           false
                                    }}";

                if (!Utils.IsValidJson(messageJson))
                {
                    throw new Exception("JSON is invalid: {jsonString}");
                }

                string messageKey = GameId + "score";
                const string eventName = "NBATEAM";
                _pusherUtil.SendScoreMessage(messageJson, eventName, messageKey);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }
        }

        #endregion

        #region PubSub Subscriptions and Handlers

        public void AddPubSubEventHandler()
        {
            _pubSubUtil.PubSubEvent += HandlePubSubEvent;
        }

        public void RemovePubSubEventHandler()
        {
            _pubSubUtil.PubSubEvent -= HandlePubSubEvent;
        }

        private void HandlePubSubEvent(object sender, PubSubEventArgs pubSubEventArgs)
        {
            // The PubSubMessage header should only contain the message type.
            // The message data should contain all information to process the message.

            PubsubMessage pubsubMessage = pubSubEventArgs.PubsubMessage;
            string pubSubMessageType = GetPubSubMessageAttribute(pubsubMessage, "type");
            // the gameId should not be stored in the message Attributes but rather in the data
            string gameIdString = GetPubSubMessageAttribute(pubsubMessage, "comp_id");
            Guid gameId = Guid.Parse(gameIdString);
            string pubsubData = pubsubMessage.Data.ToStringUtf8();

            switch (pubSubMessageType)
            {
                case "odds":
                    HandleManualOddsUpdate(pubsubData, gameId);
                    break;

                case "marketOdds":
                    HandleMarketOddsUpdate(pubsubData, gameId, "reg");
                    break;

                case "liveOdds":
                    HandleMarketOddsUpdate(pubsubData, gameId, "live");
                    break;

                case "settings":
                    HandleSettings(pubsubData, gameId);
                    break;

                case "perMarket":
                    HandleMarketAdjust(pubsubData, gameId);
                    break;

                case "players": //adjust player stats
                    if (IsTeamMode == false)
                    {
                        HandleStatsAdjust(pubsubData, gameId);
                    }
                    break;

                default:
                    Logger.Error($"Could not handle PubSub message type = '{pubSubMessageType}'");
                    return;
            }
        }

        private static string GetPubSubMessageAttribute(PubsubMessage pubsubMessage, string key)
        {
            // todo warn if the PubSubMessage Attributes contains comp_id (remove this code eventually)
            //if (pubsubMessage.Attributes.ContainsKey("comp_id"))
            //{
            //    Logger.Warn("Protocol error: PubSubMessage Attributes should not contain comp_id **********************");
            //}

            if (pubsubMessage.Attributes.ContainsKey(key))
            {
                return pubsubMessage.Attributes[key];
            }

            return "";
        }

        private void HandleMarketAdjust(string pubsubData, Guid gameId)
        {
            if (GameId == gameId)
            {
                Logger.Info(pubsubData);
                List<Adjustment> adjustments = JsonConvert.DeserializeObject<List<Adjustment>>(pubsubData);
                foreach (Adjustment adjustment in adjustments)
                {
                    MarketDescription marketDescription = _marketList.Find(s => s.MarketId == adjustment.MarketId);
                    if (marketDescription != null)
                    {
                        ModelData[NbaModelDataKeys.Adjust] = ProcesAdjustment(marketDescription, adjustment,
                            ModelData[NbaModelDataKeys.Adjust]);
                        Logger.Info($"Updated adjustment odds for {GameId} ({marketDescription.ShortName}  {adjustment.Value})");
                    }
                }
                ModelUpdateRequired = true;


            }
        }

        private void HandleStatsAdjust(string pubsubData, Guid gameId)
        {
            if (GameId == gameId)
            {

                List<Adjustment> adjustments = JsonConvert.DeserializeObject<List<Adjustment>>(pubsubData);
                foreach (Adjustment adjustment in adjustments)
                {
                    MarketDescription marketDescription = _marketList.Find(s => s.MarketId == adjustment.MarketId);
                    if (marketDescription != null)
                    {
                        ModelData[NbaModelDataKeys.Adjust] = ProcesAdjustment(marketDescription, adjustment,
                            ModelData[NbaModelDataKeys.Adjust]);
                        Logger.Info($"Updated adjustment odds for {GameId} ({marketDescription.ShortName}  {adjustment.Value})");
                    }
                }
                ModelUpdateRequired = true;


            }
        }

        private void HandleMarketOddsUpdate(string pubsubData, Guid gameId, string type)
        {
            if (GameId == gameId)
            {
                Odds odds = JsonConvert.DeserializeObject<Odds>(pubsubData);
                string key = type == "live" ? NbaModelDataKeys.InLMlf : NbaModelDataKeys.InMlf;
                ModelData[key] = ProcesOddsMessage(odds, ModelData[key]);
                Logger.Info($"{GameId} ({Description}) Updated market odds");
                ModelUpdateRequired = true;
            }
        }

        private void HandleManualOddsUpdate(string pubsubData, Guid gameId)
        {
            if (GameId == gameId)
            {
                Dictionary<string, double> odds = JsonConvert.DeserializeObject<Dictionary<string, double>>(pubsubData);
                ModelData[NbaModelDataKeys.InMlf] = odds;
                Logger.Info($"{GameId} ({Description}) Updated live odds");
                ModelUpdateRequired = true;
            }
        }

        private void HandleSettings(string pubsubData, Guid gameId)
        {
            string message = $"{GameId} ({Description}) Settings message: Body = {pubsubData}";
            Logger.Info(message);

            if (GameId == gameId)
            {
                ModelData[NbaModelDataKeys.Settings] = JsonConvert.DeserializeObject<Dictionary<string, double>>(pubsubData);
                ModelUpdateRequired = true;
            }
        }

        private static Dictionary<string, double> ProcesAdjustment(MarketDescription market, Adjustment adjustment, Dictionary<string, double> gameAdjustmentDictionary)
        {

            string oddsAbbreviaiton = market.ShortName;
            string period = Convert.ToString(market.Period);
            string key = $"{oddsAbbreviaiton},{period}";


            double val = Math.Abs(adjustment.Value) / .5;
            int aVal = 11;

            if (adjustment.Value < 0)
            {
                aVal = 11 - ToInt16(val);
            }

            if (adjustment.Value > 0)
            {
                aVal = 11 + ToInt16(val);
            }



            try
            {
                gameAdjustmentDictionary[key] = aVal;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }

            return gameAdjustmentDictionary;
        }

        private static Dictionary<string, double> ProcesOddsMessage(Odds odds, Dictionary<string, double> gameOddsDictionary)
        {
            string oddsType = odds.Type;
            string key = $"{oddsType},T";

            try
            {
                if (!gameOddsDictionary.ContainsKey(key))
                {
                    if (oddsType.Contains("TO"))
                    {
                        gameOddsDictionary[key] = Math.Abs(odds.OddsRunnerList[0].Target);   // make sure totals are positive
                    }
                    else if (oddsType.Contains("ML"))
                    {
                        gameOddsDictionary[key] = 0;                                         // make sure money is 0
                    }
                    else if (oddsType != "")
                    {
                        gameOddsDictionary[key] = odds.OddsRunnerList[0].Target;
                    }

                    gameOddsDictionary[$"{oddsType},S1"] = odds.OddsRunnerList[0].Price;
                    gameOddsDictionary[$"{oddsType},S2"] = odds.OddsRunnerList[1].Price;
                }
                else
                {
                    if (oddsType.Contains("TO"))
                    {
                        gameOddsDictionary[key] = Math.Abs(odds.OddsRunnerList[0].Target);
                    }
                    else if (oddsType.Contains("ML"))
                    {
                        gameOddsDictionary[key] = 0;
                    }
                    else if (oddsType != "")
                    {
                        gameOddsDictionary[key] = odds.OddsRunnerList[0].Target;
                    }

                    gameOddsDictionary[$"{oddsType},S1"] = odds.OddsRunnerList[0].Price;
                    gameOddsDictionary[$"{oddsType},S2"] = odds.OddsRunnerList[1].Price;
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }

            return gameOddsDictionary;
        }

        #endregion

        #region Public Game Methods

        public void LoadModelData()
        {
            if (IsTeamMode)
            {
                LoadTeamModelData();
            }
            else
            {
                LoadPlayerModelData();
            }

            SetGameDescription();
            LoadCompleteCountdownEvent.Signal();
            InitialModelDataLoadComplete = true;
        }

        public void RunModel()
        {
            if (!InitialModelDataLoadComplete)
            {
                const string message = "LoadModelData() must be called before RunModel()";
                Logger.Error(message);
                throw new Exception(message);
            }

            // clear periodMarkets each time
            PeriodMarkets.Clear();
            string saveFile = GetSaveFullFileName();
            _analyticaNba.IsTeamMode = IsTeamMode;

            Stopwatch stopwatch = Stopwatch.StartNew();
            Dictionary<int, List<Market>> result = _analyticaNba.RunModel(ModelData, PeriodMarkets, Started, saveFile);

            Logger.Info($"{GameId} ({Description}) RunModel() runtime = {stopwatch.Elapsed.Seconds,4} sec");

            // todo this code does not belong here
            stopwatch = Stopwatch.StartNew();
            if (!ModelData.ContainsKey(NbaModelDataKeys.Egt))
            {
                ModelData.Add(NbaModelDataKeys.Egt, new Dictionary<string, double>());
                ModelData[NbaModelDataKeys.Egt].Add("S", GameTimeSeconds);
            }

            if (!ModelData[NbaModelDataKeys.Egt].ContainsKey("S"))
            {
                ModelData[NbaModelDataKeys.Egt].Add("S", GameTimeSeconds);
            }

            if (IsTeamMode)
            {
                _distributorNba.SendMarkets(result, GameId, Description, ModelData[NbaModelDataKeys.Egt]);
            }
            else
            {
                /*
                //format results
                //todo move to utils
               if (result.Count > 0)
                {
                    foreach (Market market in result[1])
                    {
                        string[] playerStr = market.Player.Split('_');
                        string side = playerStr[0];
                        int number = ToInt16(playerStr[1]);
                        switch (side)
                  
            {
                                   case "away":
                                NbaPlayer playerA = AwayTeam.PlayerList.Find(x => x.Number == number);

                                if (market.MarketRunnerList[0].Price.IsNotEqualToZero() && market.MarketRunnerList[0].Price.IsNotEqualTo(1))
                                {
                                    if (playerA.Stats != null && market.Tp != null && !playerA.Stats.ContainsKey(market.Tp))
                                    {
                                        playerA.Stats.Add(market.Tp, new OddsAndStats());
                                        playerA.Side = side;
                                        playerA.comp_id = Convert.ToString(GameId);
                                        playerA.team = Convert.ToString(AwayTeam.TeamId);
                                        //playerA.player_id = Convert.ToString(playerA.PlayerId);
                                        playerA.player = playerA.FullName;
                                    }

                                 playerA.Stats[market.Tp].odds.Add(market);
                                }

                                break;

                            case "home":
                                Player playerH = HomeTeam.PlayerList.Find(x => x.Number == number);

                                if (market.MarketRunnerList[0].Price.IsNotEqualToZero() && market.MarketRunnerList[0].Price.IsNotEqualTo(1))
                                {

                                    if (playerH.Stats != null && market.Tp != null && !playerH.Stats.ContainsKey(market.Tp))
                                    {
                                        playerH.Stats.Add(market.Tp, new OddsAndStats());
                                        playerH.Side = side;
                                        playerH.comp_id = Convert.ToString(GameId);
                                        //  playerH.player_id = Convert.ToString(playerH.PlayerId);
                                        playerH.team = Convert.ToString(HomeTeam.TeamId);
                                        playerH.player = playerH.FullName;
                                    }

                                    playerH.Stats[market.Tp].odds.Add(market);
                                }

                                break;
                        }
                    }
                }

                List<Player> playersMessages = new List<Player>();
              //add stats template to fill in missing elements
                List<string> types = new List<string>{ "P", "R", "A", "S", "B", "T", "F3M", "F3A", "F2M", "F2A", "FTM", "FTA" };
            
                foreach (Player player in HomeTeam.PlayerList)
                {
                    foreach (KeyValuePair<string, OddsAndStats> playerStatMarket in player.Stats)
                    {
                        playerStatMarket.Value.MainMarket =
                         playerStatMarket.Value.odds.OrderBy(v => v.Weight).First();
                   }


                    //check for missing stats and fill them in
                    foreach (string type in types)
                    {
                       if (!player.Stats.ContainsKey(type))
                        {
                            //add new market
                            Market market = new Market
                            {
                                Tp = type,
                                Target = 0
                            };


                            market.MarketRunnerList.Add(new MarketRunner
                            {
                                Total = 0,
                                Price = 0,
                                Side = "U"
                            });

                            market.MarketRunnerList.Add(new MarketRunner
                            {
                                Total = 0,
                                Price = 0.0,
                                Side = "O"
                            });
                            OddsAndStats os = new OddsAndStats
                            {
                                MainMarket = market
                            };
                            player.Stats.Add(type, os);

                        }
                    }

                    if (player.comp_id != null && player.Stats.Count != 0)
                    {
                        playersMessages.Add(player);
                    }
                }

                foreach (Player player in AwayTeam.PlayerList)
               {
                    foreach (KeyValuePair<string, OddsAndStats> playerStatMarket in player.Stats)
                    {
                        playerStatMarket.Value.MainMarket =
                            playerStatMarket.Value.odds.OrderBy(v => v.Weight).First();
                    }


            //        //check for missing stats and fill them in
                    foreach (string type in types)
                    {
                        if (!player.Stats.ContainsKey(type))
                        {
                            //add new market
                            Market market = new Market
                            {
                                Tp = type,
                                Target = 0
                            };


                            market.MarketRunnerList.Add(new MarketRunner
                            {
                                Total = 0,
                                Price = 0,
                                Side = "U"
                            });

                            market.MarketRunnerList.Add(new MarketRunner
                            {
                                Total = 0,
                                Price = 0.0,
                                Side = "O"
                            });
                            OddsAndStats os = new OddsAndStats
                           {
                                MainMarket = market
                            };
                            player.Stats.Add(type, os);

                        }
                    }

                    if (player.comp_id != null && player.Stats.Count != 0)
                    {
                        playersMessages.Add(player);
                    }
                }

                Dictionary<string, List<Player>> playersMessage = new Dictionary<string, List<Player>>
                {
                    ["players"] = playersMessages
                };
                string messageStr = JsonConvert.SerializeObject(playersMessage);
                _distributorNba.SendPlayerMarkets(messageStr, GameId, Description);
                */
            }

            Logger.Info($"{GameId} ({Description}) SendMarkets() runtime = {stopwatch.Elapsed.Seconds,4} sec");

            ModelUpdateRequired = false;
        }

        #endregion

        #region Private Game Methods

        private void SetGameDescription()
        {
            Description = $"{AwayTeam.ShortName,-3} @ {HomeTeam.ShortName,-3}";
        }

        private void LoadTeamModelData()
        {
            GameInfo = _radarNba.GetGameInfo(GameId);
            LoadModelDataInMlf();
            //  LoadModelDataInTsf();
            //  LoadModelDataInLsf();
            //   LoadModelDataInTss();
            //  LoadModelDataInSc();

            bool isGameStarted = StartDateTime < DateTime.UtcNow;

            if (isGameStarted)
            {
                LoadCurrentScore();
            }

            InitialModelDataLoadComplete = true;
        }

        private void LoadPlayerModelData()
        {
            GameInfo = _radarNba.GetGameInfo(GameId);
            LoadModelDataPlayer();
            LoadModelDataInMlf();

            bool isGameStarted = StartDateTime < DateTime.UtcNow;

            if (isGameStarted)
            {
                LoadCurrentScore();
            }

            InitialModelDataLoadComplete = true;
        }

        private void LoadCurrentScore()
        {
            NbaGameInfo nbaGameInfo = _radarNba.GetGameSummary(GameId);

            if (nbaGameInfo.Status != IBaseGameAttributesStatus.Inprogress)
            {
                return;
            }

            if (nbaGameInfo.Clock == null)
            {
                return;
            }

            Dictionary<string, double> evsDictionary = ModelData[NbaModelDataKeys.Evs];
            evsDictionary["H,CG"] = ToDouble(nbaGameInfo.Team[0].Points);
            evsDictionary["V,CG"] = ToDouble(nbaGameInfo.Team[1].Points);
            int period = ToInt32(nbaGameInfo.Quarter);
            NbaGameState.Seconds = Utils.ConvertPeriodToGameString(period, nbaGameInfo.Clock, 2880);
            NbaGameState.Clock = nbaGameInfo.Clock;
            Collection<PeriodScoreType> homeQuarters = nbaGameInfo.Team[0].Scoring.Quarter;
            Collection<PeriodScoreType> awayQuarters = nbaGameInfo.Team[1].Scoring.Quarter;

            Dictionary<string, double> egtDictionary = ModelData[NbaModelDataKeys.Egt];
            egtDictionary["S"] = NbaGameState.Seconds;
            egtDictionary["F"] = 0; //TODO FIX

            foreach (PeriodScoreType homeQuarter in homeQuarters)
            {
                int homeQuarterNumber = ToInt32(homeQuarter.Number);
                double homeQuarterPoints = ToDouble(homeQuarter.Points);
                evsDictionary[$"H,Q{homeQuarterNumber}"] = homeQuarterPoints;

                if (homeQuarterNumber < 3)
                {
                    const string key = "H,H1";

                    if (evsDictionary.ContainsKey(key))
                    {
                        evsDictionary[key] += homeQuarterPoints;
                    }
                    else
                    {
                        evsDictionary[key] = homeQuarterPoints;
                    }
                }
                else
                {
                    const string key = "H,H2";

                    if (evsDictionary.ContainsKey(key))
                    {
                        evsDictionary[key] += homeQuarterPoints;
                    }
                    else
                    {
                        evsDictionary[key] = homeQuarterPoints;
                    }
                }
            }

            foreach (PeriodScoreType awayQuarter in awayQuarters)
            {
                int awayQuarterNumber = ToInt32(awayQuarter.Number);
                double awayQuarterPoints = ToDouble(awayQuarter.Points);
                evsDictionary[$"V,Q{awayQuarter.Number}"] = awayQuarterPoints;

                if (awayQuarterNumber < 3)
                {
                    const string key = "V,H1";

                    if (evsDictionary.ContainsKey(key))
                    {
                        evsDictionary[key] += awayQuarterPoints;
                    }
                    else
                    {
                        evsDictionary[key] = awayQuarterPoints;
                    }
                }
                else
                {
                    const string key = "V,H2";

                    if (evsDictionary.ContainsKey(key))
                    {
                        evsDictionary[key] += awayQuarterPoints;
                    }
                    else
                    {
                        evsDictionary[key] = awayQuarterPoints;
                    }
                }
            }
        }

        private void LoadModelDataPlayer()
        {
            Dictionary<string, double> homePopDictionary = new Dictionary<string, double>();
            Dictionary<string, double> homePoscDictionary = new Dictionary<string, double>();
            Dictionary<string, double> homePotmDictionary = new Dictionary<string, double>();
            Dictionary<string, double> homePscoDictionary = new Dictionary<string, double>();
            Dictionary<string, double> homeSdomDictionary = new Dictionary<string, double>();
            Dictionary<string, double> homeSdvtmDictionary = new Dictionary<string, double>();

            Dictionary<string, double> awayPopDictionary = new Dictionary<string, double>();
            Dictionary<string, double> awayPoscDictionary = new Dictionary<string, double>();
            Dictionary<string, double> awayPotmDictionary = new Dictionary<string, double>();
            Dictionary<string, double> awayPscoDictionary = new Dictionary<string, double>();
            Dictionary<string, double> awaySdomDictionary = new Dictionary<string, double>();
            Dictionary<string, double> awaySdvtmDictionary = new Dictionary<string, double>();

            Guid homeTeamId = HomeTeam.TeamId;
            var side = "H";
            Dictionary<string, double> homeTtmDictionary = _dataAccessNba.GetTtm(homeTeamId, side);

            Guid awayTeamId = AwayTeam.TeamId;
            side = "V";
            Dictionary<string, double> awayTtmDictionary = _dataAccessNba.GetTtm(awayTeamId, side);



            foreach (Player wnbaPlayer in HomeTeam.PlayerList)
            {
                side = "H";
                Guid opponentTeamId = AwayTeam.TeamId;

                homePopDictionary = _dataAccessNba.GetPop(wnbaPlayer.PlayerId, wnbaPlayer.Number, side);
                homePoscDictionary = _dataAccessNba.GetPosc(wnbaPlayer.PlayerId, wnbaPlayer.Number, side);
                homePscoDictionary = _dataAccessNba.GetPsco(wnbaPlayer.PlayerId, wnbaPlayer.Number, side);
                homeSdomDictionary = _dataAccessNba.GetSdom(wnbaPlayer.PlayerId, wnbaPlayer.Number, side);
                homePotmDictionary = _dataAccessNba.GetPotm(wnbaPlayer.PlayerId, wnbaPlayer.Number, side, opponentTeamId);
                homeSdvtmDictionary = _dataAccessNba.GetSdvtm(wnbaPlayer.PlayerId, wnbaPlayer.Number, side, opponentTeamId);
            }

            foreach (Player wnbaPlayer in AwayTeam.PlayerList)
            {
                side = "V";
                Guid opponentTeamId = HomeTeam.TeamId;

                awayPopDictionary = _dataAccessNba.GetPop(wnbaPlayer.PlayerId, wnbaPlayer.Number, side);
                awayPoscDictionary = _dataAccessNba.GetPosc(wnbaPlayer.PlayerId, wnbaPlayer.Number, side);
                awayPscoDictionary = _dataAccessNba.GetPsco(wnbaPlayer.PlayerId, wnbaPlayer.Number, side);
                awaySdomDictionary = _dataAccessNba.GetSdom(wnbaPlayer.PlayerId, wnbaPlayer.Number, side);
                awayPotmDictionary = _dataAccessNba.GetPotm(wnbaPlayer.PlayerId, wnbaPlayer.Number, side, opponentTeamId);
                awaySdvtmDictionary = _dataAccessNba.GetSdvtm(wnbaPlayer.PlayerId, wnbaPlayer.Number, side, opponentTeamId);
            }

            ModelData[NbaModelDataKeys.Pop] = Utils.MergeDictionaries(homePopDictionary, awayPopDictionary);
            ModelData[NbaModelDataKeys.Posc] = Utils.MergeDictionaries(homePoscDictionary, awayPoscDictionary);
            ModelData[NbaModelDataKeys.Psco] = Utils.MergeDictionaries(homePscoDictionary, awayPscoDictionary);
            ModelData[NbaModelDataKeys.Sdom] = Utils.MergeDictionaries(homeSdomDictionary, awaySdomDictionary);
            ModelData[NbaModelDataKeys.Potm] = Utils.MergeDictionaries(homePotmDictionary, awayPotmDictionary);
            ModelData[NbaModelDataKeys.Sdvtm] = Utils.MergeDictionaries(homeSdvtmDictionary, awaySdvtmDictionary);
            ModelData[NbaModelDataKeys.Ttm] = Utils.MergeDictionaries(homeTtmDictionary, awayTtmDictionary);
        }

        private void LoadModelDataInMlf()
        {
            //initalize the live odds object
            Dictionary<string, double> dictionaryLive = ModelData[NbaModelDataKeys.InLMlf];
            InitializeOdds(dictionaryLive);
            Dictionary<string, double> marketOddsDictionary = _datastore.GetMarketOdds(GameId);

            if (marketOddsDictionary.Count > 0)
            {
                ModelData[NbaModelDataKeys.InMlf] = marketOddsDictionary;
            }
            else
            {
                Dictionary<string, double> dictionary = ModelData[NbaModelDataKeys.InMlf];
                InitializeOdds(dictionary);
            }
        }

        private void LoadModelDataInTss()
        {
            Dictionary<string, double> dictionary1 = _dataAccessNba.GetTeamInTssFge(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary2 = _dataAccessNba.GetTeamInTssFge(HomeTeam.TeamId, "V");
            Dictionary<string, double> dictionary3 = _dataAccessNba.GetTeamInTssFge(AwayTeam.TeamId, "H");
            Dictionary<string, double> dictionary4 = _dataAccessNba.GetTeamInTssFge(AwayTeam.TeamId, "V");

            Dictionary<string, double> dictionary5 = _dataAccessNba.GetTeamInTss(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary6 = _dataAccessNba.GetTeamInTss(HomeTeam.TeamId, "V");
            Dictionary<string, double> dictionary7 = _dataAccessNba.GetTeamInTss(AwayTeam.TeamId, "H");
            Dictionary<string, double> dictionary8 = _dataAccessNba.GetTeamInTss(AwayTeam.TeamId, "V");

            Dictionary<string, double> result = new Dictionary<string, double>();

            result = Utils.MergeDictionaries(dictionary1, result);
            result = Utils.MergeDictionaries(dictionary2, result);
            result = Utils.MergeDictionaries(dictionary3, result);
            result = Utils.MergeDictionaries(dictionary4, result);

            result = Utils.MergeDictionaries(dictionary5, result);
            result = Utils.MergeDictionaries(dictionary6, result);
            result = Utils.MergeDictionaries(dictionary7, result);
            result = Utils.MergeDictionaries(dictionary8, result);

            ModelData[NbaModelDataKeys.InTss] = result;
        }

        private void LoadModelDataInSc()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            Dictionary<string, double> dictionary1 = _dataAccessNba.GetTeamInSc(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary2 = _dataAccessNba.GetTeamInSc(HomeTeam.TeamId, "V");
            Dictionary<string, double> dictionary3 = _dataAccessNba.GetTeamInSc(AwayTeam.TeamId, "H");
            Dictionary<string, double> dictionary4 = _dataAccessNba.GetTeamInSc(AwayTeam.TeamId, "V");

            result = Utils.MergeDictionaries(dictionary1, result);
            result = Utils.MergeDictionaries(dictionary2, result);
            result = Utils.MergeDictionaries(dictionary3, result);
            result = Utils.MergeDictionaries(dictionary4, result);

            ModelData[NbaModelDataKeys.InSc] = result;
        }

        private void LoadModelDataInTsf()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            Dictionary<string, double> dictionary1 = _dataAccessNba.GetTeamIntsf(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary2 = _dataAccessNba.GetTeamIntsf(HomeTeam.TeamId, "V");
            Dictionary<string, double> dictionary3 = _dataAccessNba.GetTeamIntsf(AwayTeam.TeamId, "H");
            Dictionary<string, double> dictionary4 = _dataAccessNba.GetTeamIntsf(AwayTeam.TeamId, "V");

            result = Utils.MergeDictionaries(dictionary1, result);
            result = Utils.MergeDictionaries(dictionary2, result);
            result = Utils.MergeDictionaries(dictionary3, result);
            result = Utils.MergeDictionaries(dictionary4, result);

            ModelData[NbaModelDataKeys.InTsf] = result;
        }

        private void LoadModelDataInLsf()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            Dictionary<string, double> dictionary1 = _dataAccessNba.GetTeamInLsf("Q1");
            Dictionary<string, double> dictionary2 = _dataAccessNba.GetTeamInLsf("Q2");
            Dictionary<string, double> dictionary3 = _dataAccessNba.GetTeamInLsf("Q3");
            Dictionary<string, double> dictionary4 = _dataAccessNba.GetTeamInLsf("Q4");

            result = Utils.MergeDictionaries(dictionary1, result);
            result = Utils.MergeDictionaries(dictionary2, result);
            result = Utils.MergeDictionaries(dictionary3, result);
            result = Utils.MergeDictionaries(dictionary4, result);

            ModelData[NbaModelDataKeys.InLsF] = result;
        }

        private void InitializePeriodScoring(IEnumerable<string> periodList)
        {
            foreach (string period in periodList)
            {
                PeriodScore periodScore = new PeriodScore
                {
                    HomeScore = 0,
                    AwayScore = 0
                };

                PeriodScoreDictionary[period] = periodScore;
            }
        }

        private static void InitializeOdds(IDictionary<string, double> dictionary)
        {
            List<string> lineTypeList = new List<string> { "TO", "SP", "ML" };
            List<string> periodList = new List<string> { "CG", "H1", "H2", "Q1", "Q2", "Q3", "Q4" };
            List<string> sideList = new List<string> { "T", "S1", "S2" };

            foreach (string period in periodList)
            {
                foreach (string lineType in lineTypeList)
                {
                    foreach (string side in sideList)
                    {
                        string key = $"{period},{lineType},{side}";
                        dictionary[key] = 0.0d;
                    }
                }
            }
        }

        private string GetSaveFullFileName()
        {
            const string dateFormat = "yyyymmd";
            string dateTimeString = DateTime.Now.ToString(dateFormat);
            string saveFileName = $"{Description}_{GameId}_{dateTimeString}.ana";

            string baseDirectory = Utils.GetBaseDirectory();
            string analyticaSavedModelsDirectory = Path.Combine(baseDirectory, "AnalyticaSavedModels");

            if (!Directory.Exists(analyticaSavedModelsDirectory))
            {
                Directory.CreateDirectory(analyticaSavedModelsDirectory);
            }

            string saveFullFileName = Path.Combine(analyticaSavedModelsDirectory, saveFileName);
            return saveFullFileName;
        }

        #endregion
    }

    public class NbaPlayer : Player
    {
        public string comp_id { get; set; }

        [JsonIgnore]
            public string player { get; set; }
            public string team { get; set; }
            [JsonProperty("side")]
            public string Side { get; set; }
            public Dictionary<string, OddsAndStats> Stats { get; set; }

            public NbaPlayer()
            {
                Stats = new Dictionary<string, OddsAndStats>();
            }
    }

    public class NbaGameState
    {
        public string Status { get; set; }
        public string Period { get; set; }
        public int PeriodNumber { get; set; }
        public int Seconds { get; set; }
        public string Clock { get; set; }
        public string Possession { get; set; }
        public int Foul { get; set; }
        public long Sequence { get; set; }
        public int AwayScore { get; set; }
        public int HomeScore { get; set; }

        public Dictionary<int, NbaInningScore> InningScores { get; }

        public NbaGameState()
        {
            InningScores = new Dictionary<int, NbaInningScore>();
            Sequence = 0;
        }
    }



    public class Adjustment
    {
        public int MarketId { get; set; }
        public double Value { get; set; }
    }

    public class NbaInningScore
    {
        public int Away { get; set; }
        public int Home { get; set; }
    }


}
