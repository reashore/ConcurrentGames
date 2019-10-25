using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using Google.Cloud.PubSub.V1;
using log4net;
using Newtonsoft.Json;
using SportsIq.Analytica.Ncaabb;
using SportsIq.Distributor.Ncaabb;
using SportsIq.Models.Constants.Ncaabb;
using SportsIq.Models.Markets;
using SportsIq.Models.SportRadar.Ncaabb.GameEvents;
using SportsIq.Models.SportRadar.Ncaabb.GameInfo;
using SportsIq.NoSqlDataAccess;
using SportsIq.PubSub;
using SportsIq.Utilities;
using SportsIq.Pusher;
using SportsIq.SportsRadar.Ncaabb;
using SportsIq.SqlDataAccess.Ncaabb;
using static System.Convert;

// todo remove this eventually
using MarketDescription = SportsIq.SqlDataAccess.Ncaabb.MarketDescription;

namespace SportsIq.Games.Ncaabb
{
    public class NcaabbGame : GameBase<Team, NcaabbGameInfo>, IGame
    {
        private static readonly ILog Logger;
        private readonly IDataAccessNcaabb _dataAccessNcaabb;
        private readonly IRadarNcaabb _radarNcaabb;
        private readonly IAnalyticaNcaabb _analyticaNcaabb;
        private readonly IDatastore _datastore;
        private readonly IDistributorNcaabb _distributorNcaabb;
        private readonly IPubSubUtil _pubSubUtil;
        private NcaabbGameState NcaabbGameState { get; }
        private readonly IPusherUtil _pusherUtil;
        private readonly List<MarketDescription> _marketList;

        static NcaabbGame()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(NcaabbGame));
        }

        public NcaabbGame(IDataAccessNcaabb dataAccessNcaabb, IRadarNcaabb radarNcaabb, IAnalyticaNcaabb analyticaNcaabb, IDatastore datastore, IDistributorNcaabb distributorNcaabb, IPubSubUtil pubSubUtil, IPusherUtil pusherUtil)
        {
            PeriodList = new List<string> { "CG", "H1", "H2", "Q1", "Q2", "Q3", "Q4" };
            InitializePeriodScoring(PeriodList);
            GameTimeSeconds = 2400;
            ModelData[NcaabbModelDataKeys.InMlf] = new Dictionary<string, double>();
            ModelData[NcaabbModelDataKeys.InLMlf] = new Dictionary<string, double>();
            ModelData[NcaabbModelDataKeys.Evs] = new Dictionary<string, double>();
            ModelData[NcaabbModelDataKeys.InTsf] = new Dictionary<string, double>();
            ModelData[NcaabbModelDataKeys.Egt] = new Dictionary<string, double>();
            ModelData[NcaabbModelDataKeys.InLsF] = new Dictionary<string, double>();
            ModelData[NcaabbModelDataKeys.Settings] = new Dictionary<string, double>();

            NcaabbGameState = new NcaabbGameState();

            _dataAccessNcaabb = dataAccessNcaabb;
            _analyticaNcaabb = analyticaNcaabb;
            _datastore = datastore;
            _distributorNcaabb = distributorNcaabb;
            _radarNcaabb = radarNcaabb;
            _pubSubUtil = pubSubUtil;
            _pusherUtil = pusherUtil;
            _marketList = dataAccessNcaabb.GetMarketsDescriptions();
        }

        #region Radar Event Subscriptions and Handlers

        public void AddRadarGameEventHandler()
        {
            _radarNcaabb.RadarGameEvent += HandleGameEvent;
        }

        public void RemoveRadarGameEventHandler()
        {
            _radarNcaabb.RadarGameEvent -= HandleGameEvent;
        }

        public TimeSpan GetTimeSinceLastGameEventOrHeartbeat()
        {
            TimeSpan timeSinceLastGameEventOrHeartbeat = DateTime.Now - _radarNcaabb.TimeOfLastRadarGameEventOrHeartbeat;
            return timeSinceLastGameEventOrHeartbeat;
        }

        private void HandleGameEvent(object sender, NcaabbGameEventEventArgs ncaabbGameEventEventArgs)
        {
            NcaabbGameEvent ncaabbGameEvent = ncaabbGameEventEventArgs.GameEvent;
            Guid gameId = ncaabbGameEvent.Payload.Game.Id;

            if (gameId == GameId)
            {
                string eventType = ncaabbGameEvent.Payload.Event.EventType;
                string message = $"Handling Radar game event: GameId = {gameId}, EventType = {eventType}";
                Logger.Info(message);

                switch (eventType)
                {
                    // todo add specific event
                    default:
                        ProcessRadarGameEvent(ncaabbGameEvent);
                        ModelUpdateRequired = true;
                        break;
                }
            }
        }

        private void ProcessRadarGameEvent(NcaabbGameEvent ncaabbGameEvent)
        {
            try
            {
                if (ncaabbGameEvent.Metadata == null)
                {
                    return;
                }

                Dictionary<string, double> evsDictionary = ModelData[NcaabbModelDataKeys.Evs];

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

                NcaabbGameState.Status = ncaabbGameEvent.Metadata.Status;
                Event payloadEvent = ncaabbGameEvent.Payload.Event;
                Game payloadGame = ncaabbGameEvent.Payload.Game;
                long sequence = payloadEvent.Sequence;

                if (sequence <= NcaabbGameState.Sequence)
                {
                    return;
                }

                NcaabbGameState.Sequence = sequence;
                NcaabbGameState.Period = $"Q{payloadEvent.Period.Sequence}";
                NcaabbGameState.PeriodNumber = payloadEvent.Period.Sequence;
                NcaabbGameState.Seconds =
                    Utils.ConvertPeriodToGameString(payloadEvent.Period.Sequence, payloadEvent.Clock,2400);
                NcaabbGameState.Clock = payloadEvent.Clock;
                NcaabbGameState.AwayScore = payloadGame.Away.Points;
                NcaabbGameState.HomeScore = payloadGame.Home.Points;

                Dictionary<string, double> egtDictionary = ModelData[NcaabbModelDataKeys.Egt];
                egtDictionary["S"] = NcaabbGameState.Seconds;
                egtDictionary["F"] = NcaabbGameState.Foul;
                //egtDictionary["P"] = NcaabbGameState.Possession;

                int homeScore = NcaabbGameState.HomeScore;
                int awayScore = NcaabbGameState.AwayScore;
                evsDictionary["H,CG"] = homeScore;
                evsDictionary["V,CG"] = awayScore;

                switch (NcaabbGameState.PeriodNumber)
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
                                        ""period"":         ""{NcaabbGameState.Period}"",
                                        ""clock"":          ""{NcaabbGameState.Clock}"",
                                        ""possession"":     ""{NcaabbGameState.Possession}"",
                                        ""foul"":           {NcaabbGameState.Foul},
                                        ""pause"":           false
                                    }}";

                if (!Utils.IsValidJson(messageJson))
                {
                    throw new Exception("JSON is invalid: {jsonString}");
                }

                string messageKey = GameId + "score";
                const string eventName = "WNBATEAM";
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
                        ModelData[NcaabbModelDataKeys.Adjust] = ProcesAdjustment(marketDescription, adjustment,
                            ModelData[NcaabbModelDataKeys.Adjust]);
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
                        ModelData[NcaabbModelDataKeys.Adjust] = ProcesAdjustment(marketDescription, adjustment,
                            ModelData[NcaabbModelDataKeys.Adjust]);
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
                string key = type == "live" ? NcaabbModelDataKeys.InLMlf : NcaabbModelDataKeys.InMlf;
                ModelData[key] = ProcesOddsMessage(odds, ModelData[key]);
                Logger.Info($"{GameId} ({Description}) Updated market odds");
            }
        }

        private void HandleManualOddsUpdate(string pubsubData, Guid gameId)
        {
            if (GameId == gameId)
            {
                Dictionary<string, double> odds = JsonConvert.DeserializeObject<Dictionary<string, double>>(pubsubData);
                ModelData[NcaabbModelDataKeys.InMlf] = odds;
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
                ModelData[NcaabbModelDataKeys.Settings] = JsonConvert.DeserializeObject<Dictionary<string, double>>(pubsubData);
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
            _analyticaNcaabb.IsTeamMode = IsTeamMode;

            Stopwatch stopwatch = Stopwatch.StartNew();
            Dictionary<int, List<Market>> result = _analyticaNcaabb.RunModel(ModelData, PeriodMarkets, Started, saveFile);
            Logger.Info($"{GameId} ({Description}) RunModel() runtime = {stopwatch.Elapsed.Seconds,4} sec");

            stopwatch = Stopwatch.StartNew();
            if (!ModelData.ContainsKey(NcaabbModelDataKeys.Egt))
            {
                ModelData.Add(NcaabbModelDataKeys.Egt, new Dictionary<string, double>());
                ModelData[NcaabbModelDataKeys.Egt].Add("S", 2400);
            }

            if (!ModelData[NcaabbModelDataKeys.Egt].ContainsKey("S"))
            {
                ModelData[NcaabbModelDataKeys.Egt].Add("S", 2400);
            }

            _distributorNcaabb.SendMarkets(result, GameId, Description, ModelData[NcaabbModelDataKeys.Egt]);
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
            GameInfo = _radarNcaabb.GetGameInfo(GameId);
            LoadModelDataInMlf();
            LoadModelDataInTsf();
            LoadModelDataInLsf();
            LoadModelDataInTss();
            LoadModelDataInSc();

            bool isGameStarted = StartDateTime < DateTime.UtcNow;

            if (isGameStarted)
            {
                LoadCurrentScore();
            }

            InitialModelDataLoadComplete = true;
        }

        private void LoadPlayerModelData()
        {
            GameInfo = _radarNcaabb.GetGameInfo(GameId);
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
            NcaabbGameInfo ncaabbGameInfo = _radarNcaabb.GetGameSummary(GameId);

            if (ncaabbGameInfo.Status != IBaseGameAttributesStatus.Inprogress)
            {
                return;
            }

            if (ncaabbGameInfo.Clock == null)
            {
                return;
            }

            Dictionary<string, double> evsDictionary = ModelData[NcaabbModelDataKeys.Evs];
            evsDictionary["H,CG"] = ToDouble(ncaabbGameInfo.Team[0].Points);
            evsDictionary["V,CG"] = ToDouble(ncaabbGameInfo.Team[1].Points);
            int period = ToInt32(ncaabbGameInfo.Quarter);
            NcaabbGameState.Seconds = Utils.ConvertPeriodToGameString(period, ncaabbGameInfo.Clock,2400);
            NcaabbGameState.Clock = ncaabbGameInfo.Clock;
            Collection<PeriodScoreType> homeQuarters = ncaabbGameInfo.Team[0].Scoring.Quarter;
            Collection<PeriodScoreType> awayQuarters = ncaabbGameInfo.Team[1].Scoring.Quarter;

            Dictionary<string, double> egtDictionary = ModelData[NcaabbModelDataKeys.Egt];
            egtDictionary["S"] = NcaabbGameState.Seconds;
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
            Dictionary<string, double> homeTtmDictionary = _dataAccessNcaabb.GetTtm(homeTeamId, side);

            Guid awayTeamId = AwayTeam.TeamId;
            side = "V";
            Dictionary<string, double> awayTtmDictionary = _dataAccessNcaabb.GetTtm(awayTeamId, side);

            foreach (Player ncaabbPlayer in HomeTeam.PlayerList)
            {
                side = "H";
                Guid opponentTeamId = AwayTeam.TeamId;
                Guid playerId = ncaabbPlayer.PlayerId;

                homePopDictionary = _dataAccessNcaabb.GetPop(playerId, ncaabbPlayer.Number, side);
                homePoscDictionary = _dataAccessNcaabb.GetPosc(playerId, ncaabbPlayer.Number, side);
                homePscoDictionary = _dataAccessNcaabb.GetPsco(playerId, ncaabbPlayer.Number, side);
                homeSdomDictionary = _dataAccessNcaabb.GetSdom(playerId, ncaabbPlayer.Number, side);
                homePotmDictionary = _dataAccessNcaabb.GetPotm(playerId, ncaabbPlayer.Number, side, opponentTeamId);
                homeSdvtmDictionary = _dataAccessNcaabb.GetSdvtm(playerId, ncaabbPlayer.Number, side, opponentTeamId);
            }

            foreach (Player ncaabbPlayer in AwayTeam.PlayerList)
            {
                side = "V";
                Guid opponentTeamId = HomeTeam.TeamId;
                Guid playerId = ncaabbPlayer.PlayerId;

                awayPopDictionary = _dataAccessNcaabb.GetPop(playerId, ncaabbPlayer.Number, side);
                awayPoscDictionary = _dataAccessNcaabb.GetPosc(playerId, ncaabbPlayer.Number, side);
                awayPscoDictionary = _dataAccessNcaabb.GetPsco(playerId, ncaabbPlayer.Number, side);
                awaySdomDictionary = _dataAccessNcaabb.GetSdom(playerId, ncaabbPlayer.Number, side);
                awayPotmDictionary = _dataAccessNcaabb.GetPotm(playerId, ncaabbPlayer.Number, side, opponentTeamId);
                awaySdvtmDictionary = _dataAccessNcaabb.GetSdvtm(playerId, ncaabbPlayer.Number, side, opponentTeamId);
            }

            ModelData[NcaabbModelDataKeys.Pop] = Utils.MergeDictionaries(homePopDictionary, awayPopDictionary);
            ModelData[NcaabbModelDataKeys.Posc] = Utils.MergeDictionaries(homePoscDictionary, awayPoscDictionary);
            ModelData[NcaabbModelDataKeys.Psco] = Utils.MergeDictionaries(homePscoDictionary, awayPscoDictionary);
            ModelData[NcaabbModelDataKeys.Sdom] = Utils.MergeDictionaries(homeSdomDictionary, awaySdomDictionary);
            ModelData[NcaabbModelDataKeys.Potm] = Utils.MergeDictionaries(homePotmDictionary, awayPotmDictionary);
            ModelData[NcaabbModelDataKeys.Sdvtm] = Utils.MergeDictionaries(homeSdvtmDictionary, awaySdvtmDictionary);
            ModelData[NcaabbModelDataKeys.Ttm] = Utils.MergeDictionaries(homeTtmDictionary, awayTtmDictionary);
        }

        private void LoadModelDataInMlf()
        {
            //initalize the live odds object
            Dictionary<string, double> dictionaryLive = ModelData[NcaabbModelDataKeys.InLMlf];
            InitializeOdds(dictionaryLive);
            Dictionary<string, double> marketOddsDictionary = _datastore.GetMarketOdds(GameId);

            if (marketOddsDictionary.Count > 0)
            {
                ModelData[NcaabbModelDataKeys.InMlf] = marketOddsDictionary;
            }
            else
            {
                Dictionary<string, double> dictionary = ModelData[NcaabbModelDataKeys.InMlf];
                InitializeOdds(dictionary);
            }
        }

        private void LoadModelDataInTss()
        {
            Dictionary<string, double> dictionary1 = _dataAccessNcaabb.GetTeamInTssFge(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary2 = _dataAccessNcaabb.GetTeamInTssFge(HomeTeam.TeamId, "V");
            Dictionary<string, double> dictionary3 = _dataAccessNcaabb.GetTeamInTssFge(AwayTeam.TeamId, "H");
            Dictionary<string, double> dictionary4 = _dataAccessNcaabb.GetTeamInTssFge(AwayTeam.TeamId, "V");

            Dictionary<string, double> dictionary5 = _dataAccessNcaabb.GetTeamInTss(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary6 = _dataAccessNcaabb.GetTeamInTss(HomeTeam.TeamId, "V");
            Dictionary<string, double> dictionary7 = _dataAccessNcaabb.GetTeamInTss(AwayTeam.TeamId, "H");
            Dictionary<string, double> dictionary8 = _dataAccessNcaabb.GetTeamInTss(AwayTeam.TeamId, "V");

            Dictionary<string, double> result = new Dictionary<string, double>();

            result = Utils.MergeDictionaries(dictionary1, result);
            result = Utils.MergeDictionaries(dictionary2, result);
            result = Utils.MergeDictionaries(dictionary3, result);
            result = Utils.MergeDictionaries(dictionary4, result);

            result = Utils.MergeDictionaries(dictionary5, result);
            result = Utils.MergeDictionaries(dictionary6, result);
            result = Utils.MergeDictionaries(dictionary7, result);
            result = Utils.MergeDictionaries(dictionary8, result);

            ModelData[NcaabbModelDataKeys.InTss] = result;
        }

        private void LoadModelDataInSc()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            Dictionary<string, double> dictionary1 = _dataAccessNcaabb.GetTeamInSc(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary2 = _dataAccessNcaabb.GetTeamInSc(HomeTeam.TeamId, "V");
            Dictionary<string, double> dictionary3 = _dataAccessNcaabb.GetTeamInSc(AwayTeam.TeamId, "H");
            Dictionary<string, double> dictionary4 = _dataAccessNcaabb.GetTeamInSc(AwayTeam.TeamId, "V");

            result = Utils.MergeDictionaries(dictionary1, result);
            result = Utils.MergeDictionaries(dictionary2, result);
            result = Utils.MergeDictionaries(dictionary3, result);
            result = Utils.MergeDictionaries(dictionary4, result);

            ModelData[NcaabbModelDataKeys.InSc] = result;
        }

        private void LoadModelDataInTsf()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            Dictionary<string, double> dictionary1 = _dataAccessNcaabb.GetTeamIntsf(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary2 = _dataAccessNcaabb.GetTeamIntsf(HomeTeam.TeamId, "V");
            Dictionary<string, double> dictionary3 = _dataAccessNcaabb.GetTeamIntsf(AwayTeam.TeamId, "H");
            Dictionary<string, double> dictionary4 = _dataAccessNcaabb.GetTeamIntsf(AwayTeam.TeamId, "V");

            result = Utils.MergeDictionaries(dictionary1, result);
            result = Utils.MergeDictionaries(dictionary2, result);
            result = Utils.MergeDictionaries(dictionary3, result);
            result = Utils.MergeDictionaries(dictionary4, result);

            ModelData[NcaabbModelDataKeys.InTsf] = result;
        }

        private void LoadModelDataInLsf()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            Dictionary<string, double> dictionary1 = _dataAccessNcaabb.GetTeamInLsf("Q1");
            Dictionary<string, double> dictionary2 = _dataAccessNcaabb.GetTeamInLsf("Q2");
            Dictionary<string, double> dictionary3 = _dataAccessNcaabb.GetTeamInLsf("Q3");
            Dictionary<string, double> dictionary4 = _dataAccessNcaabb.GetTeamInLsf("Q4");

            result = Utils.MergeDictionaries(dictionary1, result);
            result = Utils.MergeDictionaries(dictionary2, result);
            result = Utils.MergeDictionaries(dictionary3, result);
            result = Utils.MergeDictionaries(dictionary4, result);

            ModelData[NcaabbModelDataKeys.InLsF] = result;
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

    public class NcaabbGameState
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

        public Dictionary<int, NcaabbInningScore> InningScores { get; }

        public NcaabbGameState()
        {
            InningScores = new Dictionary<int, NcaabbInningScore>();
            Sequence = 0;
        }
    }

    public class NcaabbInningScore
    {
        public int Away { get; set; }
        public int Home { get; set; }
    }

    public class Adjustment
    {
        public int MarketId { get; set; }
        public double Value { get; set; }
    }
}
