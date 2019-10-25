using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Google.Cloud.PubSub.V1;
using log4net;
using Newtonsoft.Json;
using SportsIq.Analytica.Mlb;
using SportsIq.Distributor.Mlb;
using SportsIq.Models.Constants.Mlb;
using SportsIq.Models.Markets;
using SportsIq.Models.SportRadar.Mlb.GameEvents;
using SportsIq.Models.SportRadar.Mlb.GameInfo;
using SportsIq.NoSqlDataAccess;
using SportsIq.PubSub;
using SportsIq.Pusher;
using SportsIq.SportsRadar.Mlb;
using SportsIq.SqlDataAccess.Mlb;
using SportsIq.Utilities;
using static System.Convert;

namespace SportsIq.Games.Mlb
{
    public class MlbGame : GameBase<Team, MlbGameInfo>, IGame
    {
        private static readonly ILog Logger;
        private readonly IAnalyticaMlb _analyticaMlb;
        private readonly IDataAccessMlb _dataAccessMlb;
        private readonly IRadarMlb _radarMlb;
        private readonly IDatastore _datastore;
        private readonly IDistributorMlb _distributorMlb;
        private readonly IPubSubUtil _pubSubUtil;
        private readonly IPusherUtil _pusherUtil;
        private MlbGameState MlbGameState { get; }

        static MlbGame()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(MlbGame));
        }

        public MlbGame(IDataAccessMlb dataAccessMlb, IRadarMlb radarMlb, IAnalyticaMlb analyticaMlb, IDatastore datastore, IDistributorMlb distributorMlb, IPubSubUtil pubSubUtil, IPusherUtil pusherUtil)
        {
            string isSimulationString = ConfigurationManager.AppSettings["isSimulation"];
            IsSimulation = ToBoolean(isSimulationString);

            PeriodList = new List<string> { "F3", "F5", "F7", "CG", "I1", "I2", "I3", "I4", "I5", "I6", "I7", "I8", "I9" };
            InitializePeriodScoring(PeriodList);

            ModelData[MlbModelDataKeys.InMlf] = new Dictionary<string, double>();
            ModelData[MlbModelDataKeys.InLMlf] = new Dictionary<string, double>();
            ModelData[MlbModelDataKeys.Evs] = new Dictionary<string, double>();
            ModelData[MlbModelDataKeys.InTsf] = new Dictionary<string, double>();
            ModelData[MlbModelDataKeys.Egt] = new Dictionary<string, double>();

            MlbGameState = new MlbGameState();

            _dataAccessMlb = dataAccessMlb;
            _radarMlb = radarMlb;
            _analyticaMlb = analyticaMlb;
            _datastore = datastore;
            _distributorMlb = distributorMlb;
            _pubSubUtil = pubSubUtil;
            _pusherUtil = pusherUtil;
        }

        public Player HomePitcher { get; set; }
        public Player AwayPitcher { get; set; }

        #region Radar Event Subscriptions and Handlers

        public TimeSpan GetTimeSinceLastGameEventOrHeartbeat()
        {
            // todo need to ensure TimeOfLastRadarGameEventOrHeartbeat is not undefined
            TimeSpan timeSinceLastGameEventOrHeartbeat = DateTime.Now - _radarMlb.TimeOfLastRadarGameEventOrHeartbeat;
            return timeSinceLastGameEventOrHeartbeat;
        }

        public void AddRadarGameEventHandler()
        {
            _radarMlb.RadarGameEvent += HandleRadarGameEvent;
        }

        public void RemoveRadarGameEventHandler()
        {
            _radarMlb.RadarGameEvent -= HandleRadarGameEvent;
        }

        private void HandleRadarGameEvent(object sender, MlbGameEventEventArgs mlbGameEventEventArgs)
        {
            MlbGameEvent mlbGameEvent = mlbGameEventEventArgs.GameEvent;
            Guid gameId = mlbGameEvent.Payload.Game.Id;

            if (gameId == GameId)
            {
                string eventType = mlbGameEvent.Payload.Event.Type;

                switch (eventType)
                {
                    case "event_over":
                        GameOver = true;
                        Logger.Info($"{GameId} ({Description}) Game over");
                        break;

                    default:
                        Logger.Info($"{GameId} ({Description}) Radar Event type = {eventType}");
                        break;
                }

                // what radar events trigger these calls
                ProcessRadarGameEvent1(mlbGameEvent);
                ProcessRadarGameEvent2(mlbGameEvent);
                ModelUpdateRequired = true; //move into the handler to choose when to update
            }
        }

        private void ProcessRadarGameEvent1(MlbGameEvent mlbGameEvent)
        {
            try
            {
                if (mlbGameEvent.Metadata == null)
                {
                    return;
                }

                MlbGameState.Status = mlbGameEvent.Metadata.Status;
                MlbGameState.Inning = mlbGameEvent.Metadata.Inning;
                MlbGameState.InningHalf = mlbGameEvent.Metadata.InningHalf;

                Event payloadEvent = mlbGameEvent.Payload.Event;

                // check to make sure its moving forward
                int sequence = payloadEvent.SequenceNumber;

                if (sequence <= MlbGameState.Sequence)
                {
                    return;
                }

                MlbGameState.Sequence = sequence;
                bool isHit = false;

                if (mlbGameEvent.Payload.Event.Flags != null)
                {
                    isHit = mlbGameEvent.Payload.Event.Flags.IsHit;
                }

                if (isHit && MlbGameState.InningHalf == "T")
                {
                    MlbGameState.AwayHits += 1;
                }

                if (isHit && MlbGameState.InningHalf == "B")
                {
                    MlbGameState.HomeHits += 1;
                }

                List<Runner> payloadEventRunners = payloadEvent.Runners;
                MlbGameState.BaseSit = GetBasesSituation(payloadEventRunners);

                if (payloadEvent.Count != null)
                {
                    MlbGameState.Balls = payloadEvent.Count.Balls;
                    MlbGameState.Strikes = payloadEvent.Count.Strikes;
                    MlbGameState.Outs = payloadEvent.Count.Outs;

                    if (MlbGameState.Outs >= 3)
                    {
                        //move ahead
                        MlbGameState.Balls = 0;
                        MlbGameState.Strikes = 0;
                        MlbGameState.Outs = 0;
                        MlbGameState.BaseSit = 0;

                        if (MlbGameState.InningHalf == "T")
                        {
                            MlbGameState.InningHalf = "B";
                        }else if (MlbGameState.InningHalf == "B")
                        {
                            MlbGameState.InningHalf = "T";
                            MlbGameState.Inning += 1;
                        }
                    }
                }

                Event gameEvent = mlbGameEvent.Payload.Event;

                if (gameEvent.Type == "atbat")
                {
                    MlbGameState.Balls = 0;
                    MlbGameState.Strikes = 0;
                }

                Dictionary<string, double> egtDictionary = ModelData[MlbModelDataKeys.Egt];
                egtDictionary["I"] = MlbGameState.Inning;
                egtDictionary["T"] = MlbGameState.InningHalf == "T" ? 1 : 0;
                egtDictionary["O"] = MlbGameState.Outs;
                egtDictionary["B"] = MlbGameState.Balls;
                egtDictionary["S"] = MlbGameState.Strikes;
                egtDictionary["RS"] = MlbGameState.BaseSit;

                // update evs
                int totalAway = mlbGameEvent.Payload.Game.Away.Runs;
                int totalHome = mlbGameEvent.Payload.Game.Home.Runs;
                MlbGameState.Away = totalAway;
                MlbGameState.Home = totalHome;
                double awayscore = 0;
                double homescore = 0;
                Dictionary<string, double> evsDictionary = ModelData[MlbModelDataKeys.Evs];

                if (totalAway != 0)
                {
                    foreach (KeyValuePair<string, double> scoreItem in evsDictionary)
                    {
                        if (scoreItem.Key.Contains("V"))
                        {
                            if (!scoreItem.Key.Contains($"I{MlbGameState.Inning}"))
                            {
                                awayscore += scoreItem.Value;
                            }
                        }
                    }
                }

                if (Utils.AreNotEqual(awayscore, totalAway))
                {
                    string key = $"V,I{MlbGameState.Inning}";
                    evsDictionary[key] = totalAway - awayscore;
                }

                if (mlbGameEvent.Payload.Game.Home.Runs != 0)
                {
                    foreach (KeyValuePair<string, double> scoreItem in evsDictionary)
                    {
                        if (scoreItem.Key.Contains("H"))
                        {
                            if (!scoreItem.Key.Contains($"I{MlbGameState.Inning}"))
                            {
                                homescore += scoreItem.Value;
                            }
                        }
                    }
                }

                if (Utils.AreNotEqual(homescore, totalHome))
                {
                    string key = $"H,I{MlbGameState.Inning}";
                    evsDictionary[key] = totalHome - homescore;
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }
        }

        private void ProcessRadarGameEvent2(MlbGameEvent mlbGameEvent)
        {
            try
            {
                Event gameEvent = mlbGameEvent.Payload.Event;

                string pitcher = "";
                string batter = "";

                if (gameEvent.Pitcher != null)
                {
                    pitcher = $"{gameEvent.Pitcher.FirstName} {gameEvent.Pitcher.LastName}";
                }

                if (gameEvent.Hitter != null)
                {
                    batter = $"{gameEvent.Hitter.FirstName} {gameEvent.Hitter.LastName}";
                }

                IReadOnlyCollection<Runner> runnerList = new List<Runner>();

                if (gameEvent.Runners != null)
                {
                    runnerList = gameEvent.Runners;
                }

                string hkey = $"H,I{MlbGameState.Inning}";
                string akey = $"V,I{MlbGameState.Inning}";
                double home = 0;
                if (MlbGameState.Inning <= 9)
                {
                    if (ModelData[MlbModelDataKeys.Evs].ContainsKey(hkey))
                    {
                        home = ModelData[MlbModelDataKeys.Evs][hkey];
                    }
                }

                double away = 0;
                if (MlbGameState.Inning <= 9)
                {
                    if (ModelData[MlbModelDataKeys.Evs].ContainsKey(akey))
                    {
                        away = ModelData[MlbModelDataKeys.Evs][akey];
                    }
                }

                string messageJson = $@"{{
                                        ""game"":         ""{GameId}"",
                                        ""away_runs"":    {MlbGameState.Away},
                                        ""away_score"":   {away},
                                        ""away_hits"":    {MlbGameState.AwayHits},
                                        ""home_score"":   {home},
                                        ""home_runs"":    {MlbGameState.Home},
                                        ""home_hits"":    {MlbGameState.HomeHits},
                                        ""inning"":       {MlbGameState.Inning},
                                        ""top_btm"":      ""{MlbGameState.InningHalf}"",
                                        ""outs"":         {MlbGameState.Outs},
                                        ""balls"":        {MlbGameState.Balls},
                                        ""strikes"":      {MlbGameState.Strikes},
                                        ""bases"":        {GetBasesSituation(runnerList)},
                                        ""pitcher"":      ""{pitcher}"",
                                        ""batter"":       ""{batter}""
                                    }}";

                if (!Utils.IsValidJson(messageJson))
                {
                    throw new Exception($"JSON is invalid: {messageJson}");
                }

                const string eventName = "MLB";
                _pusherUtil.SendScoreMessage(messageJson, eventName, GameId.ToString());
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }
        }

        private static int GetBasesSituation(IReadOnlyCollection<Runner> runnerList)
        {
            if (runnerList == null)
            {
                return 0;
            }

            bool base1 = false;
            bool base2 = false;
            bool base3 = false;
            int basesSituation = 0;

            foreach (Runner runner in runnerList)
            {
                switch (runner.EndingBase)
                {
                    case 1:
                        base1 = true;
                        break;

                    case 2:
                        base2 = true;
                        break;

                    case 3:
                        base3 = true;
                        break;
                }
            }

            if (!base1 && !base2 && !base3)
            {
                basesSituation = 0;
            }

            if (base1 && !base2 && !base3)
            {
                basesSituation = 1;
            }

            if (!base1 && base2 && !base3)
            {
                basesSituation = 2;
            }

            if (!base1 && !base2 && base3)
            {
                basesSituation = 3;
            }

            if (base1 && base2 && !base3)
            {
                basesSituation = 4;
            }

            if (base1 && !base2 && base3)
            {
                basesSituation = 5;
            }

            if (!base1 && base2 && base3)
            {
                basesSituation = 6;
            }

            if (base1 && base2 && base3)
            {
                basesSituation = 7;
            }

            return basesSituation;
        }

        #endregion

        #region PubSub Event Subscriptions and Handlers

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
            // The PubSubMessage Attributes should only contain the message type.
            // The PubSubMessage Data should contain all information to process the message.

            PubsubMessage pubsubMessage = pubSubEventArgs.PubsubMessage;
            string pubSubMessageType = GetPubSubMessageAttribute(pubsubMessage, "type");
            // the gameId should not be stored in the message Attributes but rather in the data
            // todo this code is for backward compatibility and should eventually be deleted
            string gameIdString = GetPubSubMessageAttribute(pubsubMessage, "comp_id");
            Guid gameId = ParseGuid(gameIdString);
            string pubsubData = pubsubMessage.Data.ToStringUtf8();

            //Logger.Info($"{GameId} ({Description}) PubSub Event of type = {pubSubMessageType}");

            switch (pubSubMessageType)
            {
                case "pitcher_update":
                    HandlePitcherUpdate(pubsubData);
                    break;

                case "marketOdds":
                    HandleMarketOddsUpdate(pubsubData, gameId);
                    break;

                case "odds":  // from puncher
                    HandleOddsUpdate(pubsubData, gameId);
                    break;

                case "liveOdds":
                    HandleLiveMarketOddsUpdate(pubsubData, gameId);
                    break;

                case "test":
                    HandleTest(pubsubData);
                    break;

                default:
                    Logger.Error($"Could not handle PubSub message type = '{pubSubMessageType}'");
                    break;
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

        private static Guid ParseGuid(string guidString)
        {
            // todo this function should be removed once comp_id is no longer in the header
            bool success = Guid.TryParse(guidString, out Guid guid);

            if (success)
            {
                return guid;
            }

            return Guid.Empty;
        }

        private void HandlePitcherUpdate(string pubsubData)
        {
            PitcherUpdate pitcherUpdate = PitcherUpdate.FromJson(pubsubData);

            foreach (Guid gameId in pitcherUpdate.GameList)
            {
                if (GameId == gameId)
                {
                    Logger.Info($"{gameId} ({Description}) Pitcher update ");

                    MlbGamePitchers mlbGamePitchers = _dataAccessMlb.GetGamePitchers(GameId);

                    HomePitcher = GameConverterBase.CreatePlayerFromPlayerDto(mlbGamePitchers.HomePitcherDto);
                    AwayPitcher = GameConverterBase.CreatePlayerFromPlayerDto(mlbGamePitchers.AwayPitcherDto);

                    ModelUpdateRequired = true;
                }
            }
        }

        private void HandleMarketOddsUpdate(string pubsubData, Guid gameId)
        {
            if (GameId == gameId)
            {
                Odds odds = JsonConvert.DeserializeObject<Odds>(pubsubData);
                ModelData[MlbModelDataKeys.InMlf] = ProcesOddsMessage(odds, ModelData[MlbModelDataKeys.InMlf]);
                ModelUpdateRequired = true;
                Logger.Info($"{GameId} ({Description}) Updated market odds");
            }
        }

        private void HandleOddsUpdate(string pubsubData, Guid gameId)
        {
            if (GameId == gameId)
            {
                Dictionary<string, double> gameOdds = DeserializeData<Dictionary<string, double>>(pubsubData);
                ModelData[MlbModelDataKeys.InMlf] = gameOdds;
                ModelUpdateRequired = true;
                Logger.Info($"{GameId} ({Description}) Updated manual odds");
            }
        }

        private void HandleLiveMarketOddsUpdate(string pubsubData, Guid gameId)
        {
            if (GameId == gameId)
            {
                Odds odds = JsonConvert.DeserializeObject<Odds>(pubsubData);
                ModelData[MlbModelDataKeys.InLMlf] = ProcesOddsMessageLive(odds, ModelData[MlbModelDataKeys.InLMlf]);
                ModelUpdateRequired = true;
                Logger.Info($"{GameId} ({Description}) Updated live odds");
            }
        }

        private static void HandleTest(string pubsubData)
        {
            string message = $"Test message: Body = {pubsubData}";
            Logger.Info(message);
        }

        private static T DeserializeData<T>(string data)
        {
            T deserializedData = JsonConvert.DeserializeObject<T>(data);
            return deserializedData;
        }

        private static Dictionary<string, double> ProcesOddsMessageLive(Odds odds, Dictionary<string, double> gameOddsDictionary)
        {
            string oddsType = odds.Type;

            if (!oddsType.Contains("I9"))
            {
                return gameOddsDictionary;
            }

            oddsType = oddsType.Replace("I9,", "");

            string key = $"{oddsType},T";

            try
            {
                if (!gameOddsDictionary.ContainsKey(key))
                {
                    if (oddsType.Contains("TO"))
                    {
                        gameOddsDictionary.Add(key, Math.Abs(odds.OddsRunnerList[0].Target));   // make sure totals are positive
                    }
                    else if (oddsType.Contains("ML"))
                    {
                        gameOddsDictionary.Add(key, 0);                                         // make sure money is 0
                    }
                    else if (oddsType != "")
                    {
                        gameOddsDictionary.Add(key, odds.OddsRunnerList[0].Target);
                    }

                    gameOddsDictionary.Add($"{oddsType},S1", odds.OddsRunnerList[0].Price);
                    gameOddsDictionary.Add($"{oddsType},S2", odds.OddsRunnerList[1].Price);
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
                        gameOddsDictionary.Add(key, Math.Abs(odds.OddsRunnerList[0].Target));   // make sure totals are positive
                    }
                    else if (oddsType.Contains("ML"))
                    {
                        gameOddsDictionary.Add(key, 0);                                         // make sure money is 0
                    }
                    else if (oddsType != "")
                    {
                        gameOddsDictionary.Add(key, odds.OddsRunnerList[0].Target);
                    }

                    gameOddsDictionary.Add($"{oddsType},S1", odds.OddsRunnerList[0].Price);
                    gameOddsDictionary.Add($"{oddsType},S2", odds.OddsRunnerList[1].Price);
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

        #region Public Methods

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
            _analyticaMlb.IsTeamMode = IsTeamMode;
            Dictionary<int, List<Market>> result = _analyticaMlb.RunModel(ModelData, PeriodMarkets, Started, saveFile);
            _distributorMlb.SendMarkets(result, GameId, Description, ModelData[MlbModelDataKeys.Egt]);
            ModelUpdateRequired = false;
        }

        #endregion

        #region Private Methods

        private void SetGameDescription()
        {
            Description = $"{AwayTeam.ShortName,-3} @ {HomeTeam.ShortName,-3}";
        }

        private void LoadTeamModelData()
        {
            GameInfo = _radarMlb.GetGameInfo(GameId);
            LoadModelDataInMlf();
            LoadModelDataEvs();
            LoadModelDataInTsf();

            bool isGameStarted = StartDateTime < DateTime.UtcNow;

            if (isGameStarted)
            {
                LoadCurrentScore();
            }

            InitialModelDataLoadComplete = true;
        }

        private static void LoadPlayerModelData()
        {
            // MLB currently does not use player mode
        }

        private void LoadCurrentScore()
        {
            MlbGameInfo mlbGameInfo = _radarMlb.GetGameSummary(GameId);

            if (mlbGameInfo.Status != IBaseGameAttributesStatus.Inprogress)
            //if (mlbGameInfo.Status != Status1Enum.Inprogress)
            {
                return;
            }

            if (mlbGameInfo.Outcome == null)
            {
                Logger.Error("mlbGameInfo.Outcome is null");
                return;
            }

            //OutcomeType.CountElm outcomeCount = mlbGameInfo.Outcome.Count;
            OutcomeTypeCount outcomeCount = mlbGameInfo.Outcome.Count;

            if (outcomeCount != null)
            {
                MlbGameState.Balls = ToInt32(outcomeCount.Balls);
                MlbGameState.Strikes = ToInt32(outcomeCount.Strikes);
                MlbGameState.Outs = ToInt32(outcomeCount.Outs);
                MlbGameState.Inning = ToInt32(outcomeCount.Inning);
                MlbGameState.InningHalf = outcomeCount.Inning_Half;
                MlbGameState.AwayHits = ToInt32(mlbGameInfo.Away[0].Hits);
                MlbGameState.HomeHits = ToInt32(mlbGameInfo.Home[0].Hits);
            }

            Dictionary<string, double> egtDictionary = ModelData[MlbModelDataKeys.Egt];
            egtDictionary["I"] = MlbGameState.Inning;
            egtDictionary["T"] = MlbGameState.InningHalf == "T" ? 1 : 0;
            egtDictionary["O"] = MlbGameState.Outs;
            egtDictionary["B"] = MlbGameState.Balls;
            egtDictionary["S"] = MlbGameState.Strikes;
            egtDictionary["RS"] = MlbGameState.BaseSit;

            //TeamTypeCt.ScoringElm homeScoring = mlbGameInfo.Home[0].Scoring;
            //TeamTypeCt.ScoringElm awayScoring = mlbGameInfo.Away[0].Scoring;
            Collection<InningScoreType> homeScoring = mlbGameInfo.Home[0].Scoring;
            Collection<InningScoreType> awayScoring = mlbGameInfo.Away[0].Scoring;
            Dictionary<string, double> evsDictionary = ModelData[MlbModelDataKeys.Evs];

            //for (int inning = 1; inning <= homeScoring.Inning.Count; inning++)
            //{
            //    string runsString = homeScoring.Inning[inning - 1].Runs;
            //    runsString = runsString == "X" ? "0" : runsString;
            //    int runs = ToInt32(runsString);
            //    evsDictionary[$"H,I{inning}"] = runs;
            //    MlbGameState.Home += runs;
            //}

            for (int inning = 1; inning <= homeScoring.Count; inning++)
            {
                string runsString = homeScoring[inning - 1].Runs;
                runsString = runsString == "X" ? "0" : runsString;
                int runs = ToInt32(runsString);
                evsDictionary[$"H,I{inning}"] = runs;
                MlbGameState.Home += runs;
            }

            //for (int inning = 1; inning <= awayScoring.Inning.Count; inning++)
            //{
            //    string runsString = awayScoring.Inning[inning - 1].Runs;
            //    runsString = runsString == "X" ? "0" : runsString;
            //    int runs = ToInt32(runsString);
            //    evsDictionary[$"V,I{inning}"] = runs;
            //    MlbGameState.Away += runs;
            //}

            for (int inning = 1; inning <= awayScoring.Count; inning++)
            {
                string runsString = awayScoring[inning - 1].Runs;
                runsString = runsString == "X" ? "0" : runsString;
                int runs = ToInt32(runsString);
                evsDictionary[$"V,I{inning}"] = runs;
                MlbGameState.Away += runs;
            }
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

        private void LoadModelDataInMlf()
        {
            Dictionary<string, double> marketOddsDictionary = _datastore.GetMarketOdds(GameId);

            if (marketOddsDictionary.Count > 0)
            {
                ModelData[MlbModelDataKeys.InMlf] = marketOddsDictionary;
            }
            else
            {
                Dictionary<string, double> dictionary = ModelData[MlbModelDataKeys.InMlf];
                InitializeOdds(dictionary);
            }
        }

        private static void InitializeOdds(IDictionary<string, double> dictionary)
        {
            List<string> lineTypeList = new List<string> { "TO", "SP", "ML" };
            List<string> periodList = new List<string> { "I3", "I5", "I7", "I9" };
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

        private void LoadModelDataEvs()
        {
            Dictionary<string, double> gameScoreDictionary = new Dictionary<string, double>();
            Dictionary<string, double> dictionaryEgt = new Dictionary<string, double>();

            if (GameInfo.Status != IBaseGameAttributesStatus.Inprogress)
            {
                for (int inning = 1; inning <= 9; inning++)
                {
                    string key = $"V,I{inning}";
                    gameScoreDictionary[key] = 0d;

                    key = $"H,I{inning}";
                    gameScoreDictionary[key] = 0d;
                }

                ModelData[MlbModelDataKeys.Evs] = gameScoreDictionary;

                dictionaryEgt["I"] = 1;
                dictionaryEgt["T"] = 1;
                dictionaryEgt["O"] = 0;
                dictionaryEgt["B"] = 0;
                dictionaryEgt["S"] = 0;
                dictionaryEgt["RS"] = 0;
                ModelData[MlbModelDataKeys.Egt] = dictionaryEgt;

                return;
            }

            //List<InningScoreTypeCt> homeScoreList = GetScoring(GameInfo.Home);
            //List<InningScoreTypeCt> awayScoreList = GetScoring(GameInfo.Away);
            List<InningScoreType> homeScoreList = GetScoring(GameInfo.Home);
            List<InningScoreType> awayScoreList = GetScoring(GameInfo.Away);

            // todo handle innings beyond 9 ************************************************
            if (homeScoreList.Count > 9)
            {
                string message = $"homeScoringList is too long, truncating scoring list: Count = {homeScoreList.Count} ***********************************";
                Logger.Error(message);
                homeScoreList = homeScoreList.Take(9).ToList();
            }

            // todo handle innings beyond 9 ************************************************
            if (awayScoreList.Count > 9)
            {
                string message = $"awayScoringList is too long, truncating scoring list: Count = {awayScoreList.Count} ***********************************";
                Logger.Error(message);
                awayScoreList = awayScoreList.Take(9).ToList();
            }

            string homeOrVisitor = "H";
            LoadGameScoreDictionary(homeScoreList, gameScoreDictionary, homeOrVisitor);

            homeOrVisitor = "V";
            LoadGameScoreDictionary(awayScoreList, gameScoreDictionary, homeOrVisitor);

            ModelData[MlbModelDataKeys.Evs] = gameScoreDictionary;

            //EGT
            //update with game info details
            dictionaryEgt["I"] = 0;
            dictionaryEgt["T"] = 0;
            dictionaryEgt["O"] = 0;
            dictionaryEgt["B"] = 0;
            dictionaryEgt["S"] = 0;
            dictionaryEgt["RS"] = 0;
            ModelData[MlbModelDataKeys.Egt] = dictionaryEgt;
        }

        private static void LoadGameScoreDictionary(IEnumerable<InningScoreType> scoringList, IDictionary<string, double> gameScoreDictionary, string homeOrVistor)
        {
            foreach (InningScoreType homeScoring in scoringList)
            {
                string inningNumber = homeScoring.Number;
                int runs = ParseRuns(homeScoring.Runs);

                string key = $"{homeOrVistor},I{inningNumber}";
                gameScoreDictionary[key] = runs;
            }
        }

        //private static void LoadGameScoreDictionary(List<InningScoreTypeCt> scoringList, IDictionary<string, double> gameScoreDictionary, string homeOrVistor)
        //{
        //    foreach (InningScoreType homeScoring in scoringList)
        //    {
        //        string inningNumber = homeScoring.Number;
        //        int runs = ParseRuns(homeScoring.Runs);

        //        string key = $"{homeOrVistor},I{inningNumber}";
        //        gameScoreDictionary[key] = runs;
        //    }
        //}

        //private static List<InningScoreTypeCt> GetScoring(List<TeamTypeCt> teamList)
        //{
        //    TeamTypeCt team = teamList[0];
        //    TeamTypeCt.ScoringElm inningScoringArray = team.Scoring;
        //    List<InningScoreTypeCt> inningScoreList = new List<InningScoreTypeCt>(inningScoringArray.Inning);
        //    return inningScoreList;
        //}

        private static List<InningScoreType> GetScoring(Collection<TeamType> teamList)
        {
            TeamType team = teamList[0];
            Collection<InningScoreType> inningScoringArray = team.Scoring;
            List<InningScoreType> inningScoreList = new List<InningScoreType>(inningScoringArray);
            return inningScoreList;
        }

        private static int ParseRuns(string runsString)
        {
            // because of bad data in the runs field ("x" or "X"), runs was changed from int to string
            // if runs fails to parse, set it to 0
            bool success = int.TryParse(runsString, out int runs);
            return success ? runs : 0;
        }

        private void LoadModelDataInTsf()
        {
            if (HomePitcher == null || AwayPitcher == null)
            {
                Logger.Error("LoadModelDataInTsf(): HomePitcher or AwayPitcher is null");
                return;
            }

            Guid homeTeamId = HomeTeam.TeamId;
            Guid awayTeamId = AwayTeam.TeamId;
            Guid homePitcherId = HomePitcher.PlayerId;
            Guid awayPitcherId = AwayPitcher.PlayerId;

            string homeOrVistor = "H";
            Dictionary<string, double> homePlayerVersusTeam = _dataAccessMlb.GetScoreAverage(homeTeamId, awayTeamId, awayPitcherId, homeOrVistor);
            Dictionary<string, double> homePlayerVersusLeague = _dataAccessMlb.GetScoreAveragePvL(homeTeamId, awayTeamId, awayPitcherId, homeOrVistor);
            Dictionary<string, double> homeTeamVersusTeam = _dataAccessMlb.GetScoreAverageTvT(homeTeamId, awayTeamId, awayPitcherId, homeOrVistor);
            Dictionary<string, double> homeTeamVersusLeague = _dataAccessMlb.GetScoreAverageTvL(homeTeamId, awayTeamId, awayPitcherId, homeOrVistor);

            //todo add default league stats

            Dictionary<string, double> homeDictionary;

            if (homePlayerVersusTeam.Count != 0 && homePlayerVersusLeague.Count != 0)
            {
                homeDictionary = Utils.MixDictionaries(homePlayerVersusTeam, homePlayerVersusLeague);
            }
            else if (homePlayerVersusLeague.Count != 0 && homeTeamVersusTeam.Count != 0)
            {
                homeDictionary = Utils.MixDictionaries(homePlayerVersusLeague, homeTeamVersusTeam);
            }
            else if (homeTeamVersusTeam.Count != 0 && homeTeamVersusLeague.Count != 0)
            {
                homeDictionary = Utils.MixDictionaries(homeTeamVersusTeam, homeTeamVersusLeague);
            }
            else if (homeTeamVersusLeague.Count != 0)
            {
                homeDictionary = homeTeamVersusLeague;
            }
            //else
            //{
            //    //??
            //}

            homeDictionary = homeTeamVersusLeague;
            homeOrVistor = "V";
            //Dictionary<string, double> awayDictionary = _dataAccessMlb.GetScoreAverage(awayTeamId, homeTeamId, homePitcherId, homeOrVistor);
            Dictionary<string, double> awayPlayerVersusTeam = _dataAccessMlb.GetScoreAverage(awayTeamId, homeTeamId, homePitcherId, homeOrVistor);
            Dictionary<string, double> awayPlayerVersusLeague = _dataAccessMlb.GetScoreAveragePvL(awayTeamId, homeTeamId, homePitcherId, homeOrVistor);
            Dictionary<string, double> awayTeamVersusTeam = _dataAccessMlb.GetScoreAverageTvT(awayTeamId, homeTeamId, homePitcherId, homeOrVistor);
            Dictionary<string, double> awayTeamVersusLeague = _dataAccessMlb.GetScoreAverageTvL(awayTeamId, homeTeamId, homePitcherId, homeOrVistor);

            //todo add default league stats

            Dictionary<string, double> awayDictionary;


            if (awayPlayerVersusTeam.Count != 0 && awayPlayerVersusLeague.Count != 0)
            {
                awayDictionary = Utils.MixDictionaries(awayPlayerVersusTeam, awayPlayerVersusLeague);
            }
            else if (awayPlayerVersusLeague.Count != 0 && awayTeamVersusTeam.Count != 0)
            {
                awayDictionary = Utils.MixDictionaries(awayPlayerVersusLeague, awayTeamVersusTeam);
            }
            else if (awayTeamVersusTeam.Count != 0 && awayTeamVersusLeague.Count != 0)
            {
                awayDictionary = Utils.MixDictionaries(awayTeamVersusTeam, awayTeamVersusLeague);
            }
            else if (awayTeamVersusLeague.Count != 0)
            {
                awayDictionary = awayTeamVersusLeague;
            }
            //else
            //{
            //    //??
            //}

            awayDictionary = awayTeamVersusLeague;

            Dictionary<string, double> mergedDictionary = Utils.MergeDictionaries(homeDictionary, awayDictionary);
            ModelData[MlbModelDataKeys.InTsf] = mergedDictionary;
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

    public class MlbGameState
    {
        public string Status { get; set; }
        public int Inning { get; set; }
        public string InningHalf { get; set; }
        public int Balls { get; set; }
        public int Strikes { get; set; }
        public int Outs { get; set; }
        public int BaseSit { get; set; }
        public int Home { get; set; }
        public int Away { get; set; }
        public int Sequence { get; set; }
        public int AwayHits { get; set; }
        public int HomeHits { get; set; }

        public Dictionary<int, MlbInningScore> InningScores { get; }

        public MlbGameState()
        {
            InningScores = new Dictionary<int, MlbInningScore>();
            Sequence = 0;
            AwayHits = 0;
            HomeHits = 0;
            Balls = 0;
            Strikes = 0;
            Outs = 0;
            BaseSit = 0;
            Away = 0;
            Home = 0;
        }
    }

    public class MlbInningScore
    {
        public int Away { get; set; }
        public int Home { get; set; }
    }
}
