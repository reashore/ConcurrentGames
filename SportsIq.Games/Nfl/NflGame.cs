using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Google.Cloud.PubSub.V1;
using log4net;
using Newtonsoft.Json;
using SportsIq.Analytica.Nfl;
using SportsIq.Distributor.Nfl;
using SportsIq.Models.Constants.Nfl;
using SportsIq.Models.Markets;
using SportsIq.Models.SportRadar.Nfl.GameEvents;
using SportsIq.Models.SportRadar.Nfl.GameInfo;
using SportsIq.NoSqlDataAccess;
using SportsIq.PubSub;
using SportsIq.SportsRadar.Nfl;
using SportsIq.SqlDataAccess.Nfl;
using SportsIq.Utilities;
using SportsIq.Pusher;
using static System.Convert;

namespace SportsIq.Games.Nfl
{
    public class NflGame : GameBase<NflTeam, NflGameInfo>, IGame
    {
        private static readonly ILog Logger;
        private readonly IDataAccessNfl _dataAccessNfl;
        private readonly IRadarNfl _radarNfl;
        private readonly IAnalyticaNfl _analyticaNfl;
        private readonly IDatastore _datastore;
        private readonly IDistributorNfl _distributorNfl;
        private readonly IPubSubUtil _pubSubUtil;
        private NflGameState NflGameState { get; }
        private readonly IPusherUtil _pusherUtil;
        private readonly List<MarketDescription> _marketList;

        static NflGame()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(NflGame));
        }

        public NflGame(IDataAccessNfl dataAccessNfl, IRadarNfl radarNfl, IAnalyticaNfl analyticaNfl, IDatastore datastore, IDistributorNfl distributorNfl, IPubSubUtil pubSubUtil, IPusherUtil pusherUtil)
        {
            string isSimulationString = ConfigurationManager.AppSettings["isSimulation"];
            IsSimulation = ToBoolean(isSimulationString);

            PeriodList = new List<string> { "CG", "H1", "H2", "Q1", "Q2", "Q3", "Q4" };
            InitializePeriodScoring(PeriodList);

            ModelData[NflModelDataKeys.InMlf] = new Dictionary<string, double>();           // PRE MATCH MARKET LINES
            ModelData[NflModelDataKeys.InLMlf] = new Dictionary<string, double>();          // IN RUNNING MARKET LINES
            ModelData[NflModelDataKeys.Egt] = new Dictionary<string, double>();             // EVENT GAME TIME
            ModelData[NflModelDataKeys.Evs] = new Dictionary<string, double>();             // EVENT STATE
            ModelData[NflModelDataKeys.InTsf] = new Dictionary<string, double>();           // TEAM STATS
            ModelData[NflModelDataKeys.InSc] = new Dictionary<string, double>();            // SCORING CURVE
            ModelData[NflModelDataKeys.Xs] = new Dictionary<string, double>();              // EXTRA STATS
            ModelData[NflModelDataKeys.Settings] = new Dictionary<string, double>();        //model settings
            ModelData[NflModelDataKeys.Adjust] = new Dictionary<string, double>();          //market Adjustment settings

            NflGameState = new NflGameState();

            _dataAccessNfl = dataAccessNfl;
            _analyticaNfl = analyticaNfl;
            _datastore = datastore;
            _distributorNfl = distributorNfl;
            _radarNfl = radarNfl;
            _pubSubUtil = pubSubUtil;
            _pusherUtil = pusherUtil;
            _marketList = dataAccessNfl.GetMarketsDescriptions();
        }

        #region Public properties

        public Guid? HomeQuarterBackId { get; set; }
        public Guid? AwayQuarterBackId { get; set; }

        #endregion

        #region Radar Event Subscriptions and Handlers

        public void AddRadarGameEventHandler()
        {
            _radarNfl.RadarGameEvent += HandleGameEvent;
        }

        public void RemoveRadarGameEventHandler()
        {
            _radarNfl.RadarGameEvent -= HandleGameEvent;
        }

        public TimeSpan GetTimeSinceLastGameEventOrHeartbeat()
        {
            TimeSpan timeSinceLastGameEventOrHeartbeat = DateTime.Now - _radarNfl.TimeOfLastRadarGameEventOrHeartbeat;
            return timeSinceLastGameEventOrHeartbeat;
        }

        private void HandleGameEvent(object sender, NflGameEventEventArgs nflGameEventEventArgs)
        {
            NflGameEvent nflGameEvent = nflGameEventEventArgs.GameEvent;
            Guid gameId = nflGameEvent.Payload.Game.Id;

            if (gameId == GameId)
            {
                string eventType = nflGameEvent.Metadata.EventType;
                string message = $"Handling Radar game event: GameId = {gameId}, EventType = {eventType}";
                Logger.Info(message);


                switch (eventType)
                {
                    // case "setup":
                    // return;
                    // break;

                    // case "timeout":
                    //    return;

                    //  case "period_end":
                    //  return;

                    default:
                        ProcessRadarGameEvent1(nflGameEvent);
                        ModelUpdateRequired = true;
                        break;
                }

                //string message = $"Handling Radar game event: GameId = {gameId}, EventType = {eventType}";
                //Logger.Info(message);
            }
        }

        private void ProcessRadarGameEvent1(NflGameEvent nflGameEvent)
        {
            if (nflGameEvent.Metadata == null)
            {
                Logger.Error("ProcessRadarGameEvent1(): game event metadata was null");
                return;
            }

            //try
            //{
            //    bool newScore = false;

            //    if (nflGameEvent.Payload.Event.Sequence > 0)
            //    {
            //        if (nflGameEvent.Payload.Event.Sequence <= NflGameState.Sequence)
            //        {
            //            newScore = false;
            //            return;
            //        }

            //        newScore = true;
            //        NflGameState.Sequence = nflGameEvent.Payload.Event.Sequence;
            //    }

            //    string eventType = nflGameEvent.Metadata.EventType;

            //    // maybe better not to set the possession to null at this point.
            //    string possession = "N";

            //    // todo create an object from NFL event definition
            //    NflGameState.Status = nflGameEvent.Metadata.Status;
            //    Event payloadEvent = nflGameEvent.Payload.Event;
            //    Game playloadGame = nflGameEvent.Payload.Game;

            //    //drive info
            //    if (payloadEvent.Drive != null && payloadEvent.Drive.StartReason == "kickoff")
            //    {
            //        if (payloadEvent.StartSituation?.Possession != null)
            //        {
            //            if (payloadEvent.StartSituation.Possession.Id == AwayTeam.TeamId)
            //            {
            //                AwayTeam.ReceiveSecondHalf = false;
            //                HomeTeam.ReceiveSecondHalf = true;
            //            }

            //            if (payloadEvent.StartSituation.Possession.Id == HomeTeam.TeamId)
            //            {
            //                AwayTeam.ReceiveSecondHalf = true;
            //                HomeTeam.ReceiveSecondHalf = false;
            //            }
            //        }
            //    }

            //    if (eventType == "period_end")
            //    {
            //        NflGameState.YardsToGo = 10;
            //        NflGameState.Down = 1;
            //        NflGameState.Possession = 0;
            //    }

            //    if (payloadEvent.EndSituation != null)
            //    {
            //        Situation endSituation = payloadEvent.EndSituation;
            //        NflGameState.YardsToGo = endSituation.Yfd;
            //        NflGameState.Down = endSituation.Down;
            //        NflGameState.Clock = endSituation.Clock;
            //        NflGameState.RemainingSeconds = Utils.ConvertPeriodToGameString(playloadGame.Quarter, payloadEvent.EndSituation.Clock, 3600);
            //        NflGameState.Possession = 0;


            //        if (nflGameEvent.Payload.Event.EndSituation.Possession.Id == AwayTeam.TeamId)
            //        {
            //            NflGameState.Possession = 1;
            //            possession = "V";
            //        }

            //        if (nflGameEvent.Payload.Event.EndSituation.Possession.Id == HomeTeam.TeamId)
            //        {
            //            NflGameState.Possession = 2;
            //            possession = "H";
            //        }

            //        if (nflGameEvent.Payload.Event.EndSituation.Possession.Id !=
            //            nflGameEvent.Payload.Event.EndSituation.Location.Id)
            //        {
            //            NflGameState.FieldPosition = 100 - ToInt32(nflGameEvent.Payload.Event.EndSituation.Location.Yardline);
            //        }
            //        else
            //        {
            //            NflGameState.FieldPosition = ToInt32(nflGameEvent.Payload.Event.EndSituation.Location.Yardline);
            //        }

            //        NflGameState.Quarter = nflGameEvent.Payload.Game.Quarter;
            //        ModelData[NflModelDataKeys.Egt] = new Dictionary<string, double>
            //        {
            //            {"S", ToDouble(NflGameState.RemainingSeconds)},
            //            {"FP", ToDouble(NflGameState.FieldPosition)},
            //            {"DN", ToDouble(NflGameState.Down)},
            //            {"YF", ToDouble(NflGameState.YardsToGo)},
            //            {"P", ToDouble(NflGameState.Possession)}
            //        };
            //    }
            //    //ModelData[NflModelDataKeys.Egt] = new Dictionary<string, double>();

            //    int awayScore;      //nflGameEvent.Payload.Game.Summary.Away.Points;
            //    int homeScore;      // nflGameEvent.Payload.Game.Summary.Home.Points;

            //    if (payloadEvent.AwayPoints != null)
            //    {
            //        awayScore = payloadEvent.AwayPoints;
            //        homeScore = payloadEvent.HomePoints;
            //    }
            //    else
            //    {
            //        return;
            //    }

            //    double scoreCounter = 0;
            //    //check to see if there is any current score
            //    foreach (KeyValuePair<string, double> existingScore in ModelData[NflModelDataKeys.Evs])
            //    {
            //        scoreCounter += existingScore.Value;
            //    }

            //    Dictionary<string, double> evsDictionary = ModelData[NflModelDataKeys.Evs];

            //    if (scoreCounter > 0 && awayScore == 0 && homeScore == 0)
            //    {

            //    }
            //    else
            //    {
            //        // todo are the if checks necessary?
            //        if (!evsDictionary.ContainsKey("H,Q1"))
            //        {
            //            evsDictionary["V,Q1"] = 0;
            //            evsDictionary["H,Q1"] = 0;
            //        }

            //        if (!evsDictionary.ContainsKey("H,H1"))
            //        {
            //            evsDictionary["V,H1"] = 0;
            //            evsDictionary["H,H1"] = 0;
            //        }

            //        if (!evsDictionary.ContainsKey("H,Q2"))
            //        {
            //            evsDictionary["V,Q2"] = 0;
            //            evsDictionary["H,Q2"] = 0;
            //        }

            //        if (!evsDictionary.ContainsKey("H,H2"))
            //        {
            //            evsDictionary["V,H2"] = 0;
            //            evsDictionary["H,H2"] = 0;
            //        }

            //        if (!evsDictionary.ContainsKey("H,Q3"))
            //        {
            //            evsDictionary["V,Q3"] = 0;
            //            evsDictionary["H,Q3"] = 0;
            //        }

            //        if (!evsDictionary.ContainsKey("H,Q4"))
            //        {
            //            evsDictionary["V,Q4"] = 0;
            //            evsDictionary["H,Q4"] = 0;
            //        }

            //        evsDictionary["H,CG"] = homeScore;
            //        evsDictionary["V,CG"] = awayScore;

            //        switch (NflGameState.Quarter)
            //        {
            //            case 1:
            //                evsDictionary["V,Q1"] = awayScore;
            //                evsDictionary["H,Q1"] = homeScore;
            //                break;

            //            case 2:
            //                evsDictionary["V,Q2"] = awayScore - evsDictionary["V,Q1"];
            //                evsDictionary["H,Q2"] = homeScore - evsDictionary["H,Q1"];
            //                break;

            //            case 3:
            //                evsDictionary["V,Q3"] = awayScore - (evsDictionary["V,Q1"] + evsDictionary["V,Q2"]);
            //                evsDictionary["H,Q3"] = homeScore - (evsDictionary["H,Q1"] + evsDictionary["H,Q2"]);
            //                break;

            //            case 4:
            //                evsDictionary["V,Q4"] =
            //                    awayScore - (evsDictionary["V,Q1"] + evsDictionary["V,Q2"] + evsDictionary["V,Q3"]);
            //                evsDictionary["H,Q4"] =
            //                    homeScore - (evsDictionary["H,Q1"] + evsDictionary["H,Q2"] + evsDictionary["H,Q3"]);
            //                break;
            //        }

            //        evsDictionary["H,H1"] = evsDictionary["H,Q1"] + evsDictionary["H,Q2"];
            //        evsDictionary["V,H1"] = evsDictionary["V,Q1"] + evsDictionary["V,Q2"];
            //        evsDictionary["H,H2"] = evsDictionary["H,Q3"] + evsDictionary["H,Q4"];
            //        evsDictionary["V,H2"] = evsDictionary["V,Q3"] + evsDictionary["V,Q4"];
            //    }

            //    ModelUpdateRequired = true;
            //    string quarter = $"Q{NflGameState.Quarter}";
            //    string clock = NflGameState.Clock;

            //    if (NflGameState.Status == "halftime")
            //    {
            //        quarter = "HT";
            //        if (AwayTeam.ReceiveSecondHalf)
            //        {
            //            possession = "V";
            //            NflGameState.Possession = 1;
            //            ModelData[NflModelDataKeys.Egt]["P"] = NflGameState.Possession;
            //            ModelData[NflModelDataKeys.Egt]["FP"] = 25;
            //            ModelData[NflModelDataKeys.Egt]["DN"] = 1;
            //            ModelData[NflModelDataKeys.Egt]["YF"] = 10;
            //            ModelData[NflModelDataKeys.Egt]["S"] = 15 * 60 * 2;
            //        }

            //        if (HomeTeam.ReceiveSecondHalf)
            //        {
            //            possession = "H";
            //            NflGameState.Possession = 2;
            //            ModelData[NflModelDataKeys.Egt]["P"] = NflGameState.Possession;
            //            ModelData[NflModelDataKeys.Egt]["FP"] = 25;
            //            ModelData[NflModelDataKeys.Egt]["DN"] = 1;
            //            ModelData[NflModelDataKeys.Egt]["YF"] = 10;
            //            ModelData[NflModelDataKeys.Egt]["S"] = 15 * 60 * 2;
            //        }
            //    }

            //    if (NflGameState.Status == "closed")
            //    {
            //        quarter = "FN";
            //    }

            //    if (eventType == "period_end")
            //    {
            //        int qplus = NflGameState.Quarter + 1;
            //        quarter = $"Q{qplus}";
            //        clock = "15:00";
            //    }

            //    string messageJson = $@"{{
            //                            ""game"":               ""{GameId}"",
            //                            ""Status"":             ""{NflGameState.Status}"",
            //                            ""away_score"":         {awayScore},
            //                            ""home_score"":         {homeScore},
            //                             ""away_q1"":           {evsDictionary["V,Q1"]},
            //                            ""home_q1"":            {evsDictionary["H,Q1"]},
            //                            ""away_q2"":            {evsDictionary["V,Q2"]},
            //                            ""home_q2"":            {evsDictionary["H,Q2"]},
            //                             ""away_q3"":           {evsDictionary["V,Q3"]},
            //                            ""home_q3"":            {evsDictionary["H,Q3"]},
            //                            ""away_q4"":            {evsDictionary["V,Q4"]},
            //                            ""home_q4"":            {evsDictionary["H,Q4"]},
            //                            ""period"":             ""{quarter}"",
            //                            ""clock"":              ""{clock}"",
            //                            ""possession"":         ""{possession}"",
            //                            ""down"":               {NflGameState.Down},
            //                            ""Y2F"":                {NflGameState.YardsToGo},
            //                            ""FP"":                 {NflGameState.FieldPosition}
            //                        }}";

            //    if (!Utils.IsValidJson(messageJson))
            //    {
            //        throw new Exception("JSON is invalid: {jsonString}");
            //    }

            //    string messageKey = GameId + "score";
            //    const string eventName = "NFLTEAM";
            //    Logger.Info(messageJson);

            //    if (newScore)
            //    {
            //        _pusherUtil.SendScoreMessage(messageJson, eventName, messageKey);
            //    }
            //}
            //catch (Exception exception)
            //{
            //    Logger.Error(exception);
            //}

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
                case "marketOdds":
                    HandleMarketOddsUpdate(pubsubData, gameId);
                    break;

                case "liveOdds":
                    HandleLiveMarketOddsUpdate(pubsubData, gameId);
                    break;

                case "odds":
                    HandleOddsUpdate(pubsubData, gameId);
                    break;

                case "settings":
                    HandleSettings(pubsubData, gameId);
                    break;

                case "test":
                    HandleTest(pubsubData);
                    break;

                case "perMarket":
                    HandleMarketAdjust(pubsubData, gameId);
                    break;

                default:
                    Logger.Error($"Could not handle PubSub message type = '{pubSubMessageType}' body={pubsubData}");
                    return;
            }
        }

        private static string GetPubSubMessageAttribute(PubsubMessage pubsubMessage, string key)
        {
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
                        ModelData[NflModelDataKeys.Adjust] = ProcesAdjustment(marketDescription, adjustment,
                        ModelData[NflModelDataKeys.Adjust]);
                        Logger.Info($"Updated adjustment odds for {GameId} ({marketDescription.ShortName}  {adjustment.Value})");
                    }
                }
                ModelUpdateRequired = true;


            }
        }

        private void HandleMarketOddsUpdate(string pubsubData, Guid gameId)
        {
            if (GameId == gameId)
            {
                Odds odds = JsonConvert.DeserializeObject<Odds>(pubsubData);
                ModelData[NflModelDataKeys.InMlf] = ProcesOddsMessage(odds, ModelData[NflModelDataKeys.InMlf]);
                ModelUpdateRequired = true;
                Logger.Info($"Updated market odds for {GameId} ({Description})");
            }
        }

        private void HandleOddsUpdate(string pubsubData, Guid gameId)
        {
            if (GameId == gameId)
            {
                Dictionary<string, double> gameOdds = DeserializeData<Dictionary<string, double>>(pubsubData);
                //   LoadModelDataInMlf(gameOdds); //not needed for updates
                ModelData[NflModelDataKeys.InMlf] = gameOdds;
                ModelUpdateRequired = true;
                Logger.Info($"Updated manual odds for {GameId} ({Description})");
            }
        }

        private void HandleLiveMarketOddsUpdate(string pubsubData, Guid gameId)
        {
            if (GameId == gameId)
            {
                Odds odds = JsonConvert.DeserializeObject<Odds>(pubsubData);
                ModelData[NflModelDataKeys.InLMlf] = ProcesOddsMessage(odds, ModelData[NflModelDataKeys.InLMlf]);
                ModelUpdateRequired = true;
                Logger.Info($"Updated live odds for {GameId} ({Description})");
            }
        }

        private void HandleSettings(string pubsubData, Guid gameId)
        {
            string message = $"{GameId} ({Description}) Settings message: Body = {pubsubData}";
            Logger.Info(message);

            if (GameId == gameId)
            {
                ModelData[NflModelDataKeys.Settings] = JsonConvert.DeserializeObject<Dictionary<string, double>>(pubsubData);
                ModelUpdateRequired = true;
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
            ModelUpdateRequired = false;
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
            _analyticaNfl.IsTeamMode = IsTeamMode;
            Dictionary<int, List<Market>> result = _analyticaNfl.RunModel(ModelData, PeriodMarkets, Started, saveFile);


            //TODO move game total seconds to config
            //TODO move check to util method

            if (!ModelData.ContainsKey(NflModelDataKeys.Egt))
            {
                ModelData.Add(NflModelDataKeys.Egt, new Dictionary<string, double>());
                ModelData[NflModelDataKeys.Egt].Add("S", 3600);
            }

            if (!ModelData[NflModelDataKeys.Egt].ContainsKey("S"))
            {
                ModelData[NflModelDataKeys.Egt].Add("S", 3600);
            }

            _distributorNfl.SendMarkets(result, GameId, Description, ModelData[NflModelDataKeys.Egt]);
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
            MarketOddsDictionary = _datastore.GetMarketOdds(GameId);

            /*  SIMULATION ODDS
             {"CG,TO,T":0,"CG,TO,S1":0,"CG,TO,S2":0,"CG,SP,T":0,"CG,SP,S1":0,"CG,SP,S2":0,"CG,ML,T":0.0,"CG,ML,S1":0,"CG,ML,S2":0
             */
            if (IsSimulation && MarketOddsDictionary.Count == 0)
            {
                MarketOddsDictionary["CG,TO,T"] = 45.5;
                MarketOddsDictionary["CG,TO,S1"] = 1.91;
                MarketOddsDictionary["CG,TO,S2"] = 1.91;

                MarketOddsDictionary["CG,SP,T"] = 7;
                MarketOddsDictionary["CG,SP,S1"] = 1.89;
                MarketOddsDictionary["CG,SP,S2"] = 2.02;

                MarketOddsDictionary["CG,ML,T"] = 0;
                MarketOddsDictionary["CG,ML,S1"] = 3.45;
                MarketOddsDictionary["CG,ML,S2"] = 1.33;
            }
            /* END SIMULATION ODDS*/

            LoadModelDataInMlf(MarketOddsDictionary);

            ModelData[NflModelDataKeys.InTsf] = GetInTsf();
            ModelData[NflModelDataKeys.InSc] = GetInSc();
            ModelData[NflModelDataKeys.Xs] = GetXs();  //new

            bool isGameStarted = StartDateTime < DateTime.UtcNow;

            if (isGameStarted)
            {
                LoadCurrentScore();
            }
        }

        private void LoadPlayerModelData()
        {
            GameInfo = _radarNfl.GetGameInfo(GameId);
            //LoadModelDataForPlayers();

            MarketOddsDictionary = _datastore.GetMarketOdds(GameId);
            LoadModelDataInMlf(MarketOddsDictionary);
            LoadModelDataEvs();
            //ModelData[NflModelDataKeys.InTsf] = GetModelDataInTsf();

            bool isGameStarted = StartDateTime < DateTime.UtcNow;

            if (isGameStarted)
            {
                // LoadCurrentScore();
            }
        }

        private void LoadCurrentScore()
        {
            NflGameInfo nflGameInfo = _radarNfl.GetGameInfo(GameId);
            double awayCg = 0;
            double homeCg = 0;
            double homeH1 = 0;
            double awayH1 = 0;
            double homeH2 = 0;
            double awayH2 = 0;



            if (nflGameInfo.Status == IBaseGameAttributesStatus.Scheduled || nflGameInfo.Status == IBaseGameAttributesStatus.Closed)
            {
                return;
            }
            /*
            if (nflGameInfo.Coin_Toss[1].Quarter == "3")
            {
                if (nflGameInfo.Coin_Toss[1].Away.Decision == "receive")
                {
                    AwayTeam.receive2ndHalf = true;
                }
                else
                {
                    HomeTeam.receive2ndHalf = true;
                }
            }
            */

            try
            {
                foreach (PeriodType score in nflGameInfo.Scoring.Quarter)
                {
                    ModelData[NflModelDataKeys.Evs].Add($"V,Q{score.Sequence}", ToDouble(score.Away_Points));
                    ModelData[NflModelDataKeys.Evs].Add($"H,Q{score.Sequence}", ToDouble(score.Home_Points));


                    homeCg += ToDouble(score.Home_Points);
                    awayCg += ToDouble(score.Away_Points);

                    switch (score.Sequence)
                    {
                        case "1":
                            homeH1 += ToDouble(score.Home_Points);
                            awayH1 += ToDouble(score.Away_Points);
                            break;

                        case "2":
                            homeH1 += ToDouble(score.Home_Points);
                            awayH1 += ToDouble(score.Away_Points);
                            break;

                        case "3":
                            homeH2 += ToDouble(score.Home_Points);
                            awayH2 += ToDouble(score.Away_Points);
                            break;

                        case "4":
                            homeH2 += ToDouble(score.Home_Points);
                            awayH2 += ToDouble(score.Away_Points);
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Info(exception);
            }

            ModelData[NflModelDataKeys.Evs].Add("V,CG", ToDouble(awayCg));
            ModelData[NflModelDataKeys.Evs].Add("H,CG", ToDouble(homeCg));
            ModelData[NflModelDataKeys.Evs].Add("V,H1", ToDouble(awayH1));
            ModelData[NflModelDataKeys.Evs].Add("H,H1", ToDouble(homeH1));
            ModelData[NflModelDataKeys.Evs].Add("V,H2", ToDouble(awayH2));
            ModelData[NflModelDataKeys.Evs].Add("H,H2", ToDouble(homeH2));

            NflGameState.YardsToGo = ToInt32(nflGameInfo.Situation.Yfd);
            NflGameState.Down = ToInt32(nflGameInfo.Situation.Down);
            NflGameState.RemainingSeconds = Utils.ConvertPeriodToGameString(ToInt32(nflGameInfo.Quarter), nflGameInfo.Situation.Clock, 3600);
            NflGameState.Possession = Guid.Parse(nflGameInfo.Summary.Away.Id) == AwayTeam.TeamId ? 0 : 1;

            if (nflGameInfo.Situation.Possession.Id !=
                nflGameInfo.Situation.Location.Id)
            {
                NflGameState.FieldPosition = 100 - ToInt32(nflGameInfo.Situation.Location.Yardline);
            }
            else
            {
                NflGameState.FieldPosition = ToInt32(nflGameInfo.Situation.Location.Yardline);
            }

            // NflGameState.FP = ToInt32(nflGameInfo.Situation.Location.Yardline);
            NflGameState.Quarter = ToInt32(nflGameInfo.Quarter);

            // string possession;

            if (Guid.Parse(nflGameInfo.Situation.Possession.Id) == AwayTeam.TeamId)
            {
                NflGameState.Possession = 1;
            }

            if (Guid.Parse(nflGameInfo.Situation.Possession.Id) == HomeTeam.TeamId)
            {
                NflGameState.Possession = 2;
            }

            ModelData[NflModelDataKeys.Egt] = new Dictionary<string, double>
            {
                {"S", ToDouble(NflGameState.RemainingSeconds)},
                {"FP", ToDouble(NflGameState.FieldPosition)},
                {"DN", ToDouble(NflGameState.Down)},
                {"YF", ToDouble(NflGameState.YardsToGo)},
                {"P", ToDouble(NflGameState.Possession)}
            };
        }

        //private void LoadModelDataForPlayers()
        //{
        //    Dictionary<string, double> homePopDictionary = new Dictionary<string, double>();
        //    Dictionary<string, double> homePoscDictionary = new Dictionary<string, double>();
        //    Dictionary<string, double> homePotmDictionary = new Dictionary<string, double>();
        //    Dictionary<string, double> homePscoDictionary = new Dictionary<string, double>();
        //    Dictionary<string, double> homeSdomDictionary = new Dictionary<string, double>();
        //    Dictionary<string, double> homeSdvtmDictionary = new Dictionary<string, double>();

        //    Dictionary<string, double> awayPopDictionary = new Dictionary<string, double>();
        //    Dictionary<string, double> awayPoscDictionary = new Dictionary<string, double>();
        //    Dictionary<string, double> awayPotmDictionary = new Dictionary<string, double>();
        //    Dictionary<string, double> awayPscoDictionary = new Dictionary<string, double>();
        //    Dictionary<string, double> awaySdomDictionary = new Dictionary<string, double>();
        //    Dictionary<string, double> awaySdvtmDictionary = new Dictionary<string, double>();

        //    Guid homeTeamId = HomeTeam.TeamId;
        //    var side = "H";
        //    Dictionary<string, double> homeTtmDictionary = _dataAccessNfl.GetTtm(homeTeamId, side);

        //    Guid awayTeamId = AwayTeam.TeamId;
        //    side = "V";
        //    Dictionary<string, double> awayTtmDictionary = _dataAccessNfl.GetTtm(awayTeamId, side);

        //    foreach (NflPlayer nflPlayer in HomeTeam.PlayerList)
        //    {
        //        side = "H";
        //        Guid opponentTeamId = AwayTeam.TeamId;

        //        homePopDictionary = _dataAccessNfl.GetPop(nflPlayer.PlayerId, nflPlayer.Number, side);
        //        homePoscDictionary = _dataAccessNfl.GetPosc(nflPlayer.PlayerId, nflPlayer.Number, side);
        //        homePscoDictionary = _dataAccessNfl.GetPsco(nflPlayer.PlayerId, nflPlayer.Number, side);
        //        homeSdomDictionary = _dataAccessNfl.GetSdom(nflPlayer.PlayerId, nflPlayer.Number, side);
        //        homePotmDictionary = _dataAccessNfl.GetPotm(nflPlayer.PlayerId, nflPlayer.Number, side, opponentTeamId);
        //        homeSdvtmDictionary = _dataAccessNfl.GetSdvtm(nflPlayer.PlayerId, nflPlayer.Number, side, opponentTeamId);
        //    }

        //    foreach (NflPlayer nflPlayer in AwayTeam.PlayerList)
        //    {
        //        side = "V";
        //        Guid opponentTeamId = HomeTeam.TeamId;

        //        awayPopDictionary = _dataAccessNfl.GetPop(nflPlayer.PlayerId, nflPlayer.Number, side);
        //        awayPoscDictionary = _dataAccessNfl.GetPosc(nflPlayer.PlayerId, nflPlayer.Number, side);
        //        awayPscoDictionary = _dataAccessNfl.GetPsco(nflPlayer.PlayerId, nflPlayer.Number, side);
        //        awaySdomDictionary = _dataAccessNfl.GetSdom(nflPlayer.PlayerId, nflPlayer.Number, side);
        //        awayPotmDictionary = _dataAccessNfl.GetPotm(nflPlayer.PlayerId, nflPlayer.Number, side, opponentTeamId);
        //        awaySdvtmDictionary = _dataAccessNfl.GetSdvtm(nflPlayer.PlayerId, nflPlayer.Number, side, opponentTeamId);
        //    }

        //    ModelData[NflModelDataKeys.Pop] = Utils.MergeDictionaries(homePopDictionary, awayPopDictionary);
        //    ModelData[NflModelDataKeys.Posc] = Utils.MergeDictionaries(homePoscDictionary, awayPoscDictionary);
        //    ModelData[NflModelDataKeys.Psco] = Utils.MergeDictionaries(homePscoDictionary, awayPscoDictionary);
        //    ModelData[NflModelDataKeys.Sdom] = Utils.MergeDictionaries(homeSdomDictionary, awaySdomDictionary);
        //    ModelData[NflModelDataKeys.Potm] = Utils.MergeDictionaries(homePotmDictionary, awayPotmDictionary);
        //    ModelData[NflModelDataKeys.Sdvtm] = Utils.MergeDictionaries(homeSdvtmDictionary, awaySdvtmDictionary);
        //    ModelData[NflModelDataKeys.Ttm] = Utils.MergeDictionaries(homeTtmDictionary, awayTtmDictionary);
        //}

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

        private void LoadModelDataInMlf(Dictionary<string, double> marketOddsDictionary)
        {
            // Dictionary<string, double> marketOddsDictionary = _datastore.GetMarketOdds(GameId);

            if (marketOddsDictionary.Count > 0)
            {
                ModelData[NflModelDataKeys.InMlf] = marketOddsDictionary;
            }
            else
            {
                Dictionary<string, double> dictionary = ModelData[NflModelDataKeys.InMlf];
                InitializeOdds(dictionary);
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

        private static void LoadModelDataEvs()
        {
            //ModelData[NflModelDataKeys.Egt] = dictionaryEgt;
        }

        private string GetSaveFullFileName()
        {
            const string dateFormat = "yyyymmd";
            string dateTimeString = DateTime.Now.ToString(dateFormat);
            string saveFileName = $"{AwayTeam.ShortName}_{HomeTeam.ShortName}_{GameId}_{dateTimeString}.ana";

            string baseDirectory = Utils.GetBaseDirectory();
            string analyticaSavedModelsDirectory = Path.Combine(baseDirectory, "AnalyticaSavedModels");

            if (!Directory.Exists(analyticaSavedModelsDirectory))
            {
                Directory.CreateDirectory(analyticaSavedModelsDirectory);
            }

            string saveFullFileName = Path.Combine(analyticaSavedModelsDirectory, saveFileName);
            return saveFullFileName;
        }

        private Dictionary<string, double> GetInTss()
        {
            Dictionary<string, double> dictionary1 = _dataAccessNfl.GetTeamInTssFge(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary2 = _dataAccessNfl.GetTeamInTssFge(HomeTeam.TeamId, "V");
            Dictionary<string, double> dictionary3 = _dataAccessNfl.GetTeamInTssFge(AwayTeam.TeamId, "H");
            Dictionary<string, double> dictionary4 = _dataAccessNfl.GetTeamInTssFge(AwayTeam.TeamId, "V");

            Dictionary<string, double> dictionary5 = _dataAccessNfl.GetTeamInTss(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary6 = _dataAccessNfl.GetTeamInTss(HomeTeam.TeamId, "V");
            Dictionary<string, double> dictionary7 = _dataAccessNfl.GetTeamInTss(AwayTeam.TeamId, "H");
            Dictionary<string, double> dictionary8 = _dataAccessNfl.GetTeamInTss(AwayTeam.TeamId, "V");

            Dictionary<string, double> result = new Dictionary<string, double>();

            result = Utils.MergeDictionaries(dictionary1, result);
            result = Utils.MergeDictionaries(dictionary2, result);
            result = Utils.MergeDictionaries(dictionary3, result);
            result = Utils.MergeDictionaries(dictionary4, result);

            result = Utils.MergeDictionaries(dictionary5, result);
            result = Utils.MergeDictionaries(dictionary6, result);
            result = Utils.MergeDictionaries(dictionary7, result);
            result = Utils.MergeDictionaries(dictionary8, result);

            return result;
        }

        private Dictionary<string, double> GetInSc()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            Dictionary<string, double> dictionary1 = _dataAccessNfl.GetTeamInSc(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary2 = _dataAccessNfl.GetTeamInSc(AwayTeam.TeamId, "V");

            result = Utils.MergeDictionaries(dictionary1, result);
            result = Utils.MergeDictionaries(dictionary2, result);

            return result;
        }

        private Dictionary<string, double> GetXs()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            //todo add data access for Xs

            Dictionary<string, double> dictionary1 = _dataAccessNfl.GetTeamXs(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary2 = _dataAccessNfl.GetTeamXs(HomeTeam.TeamId, "V");
            Dictionary<string, double> dictionary3 = _dataAccessNfl.GetTeamXs(AwayTeam.TeamId, "H");
            Dictionary<string, double> dictionary4 = _dataAccessNfl.GetTeamXs(AwayTeam.TeamId, "V");

            result = Utils.MergeDictionaries(dictionary1, result);
            result = Utils.MergeDictionaries(dictionary2, result);
            result = Utils.MergeDictionaries(dictionary3, result);
            result = Utils.MergeDictionaries(dictionary4, result);

            return result;
        }

        private Dictionary<string, double> GetInTsf()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            Dictionary<string, double> dictionary1 = _dataAccessNfl.GetTeamIntsf(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary2 = _dataAccessNfl.GetTeamIntsf(HomeTeam.TeamId, "V");
            Dictionary<string, double> dictionary3 = _dataAccessNfl.GetTeamIntsf(AwayTeam.TeamId, "H");
            Dictionary<string, double> dictionary4 = _dataAccessNfl.GetTeamIntsf(AwayTeam.TeamId, "V");

            result = Utils.MergeDictionaries(dictionary1, result);
            result = Utils.MergeDictionaries(dictionary2, result);
            result = Utils.MergeDictionaries(dictionary3, result);
            result = Utils.MergeDictionaries(dictionary4, result);

            return result;
        }

        #endregion
    }

    public class NflGameState
    {
        public string Status { get; set; }
        public int Period { get; set; }
        public string Clock { get; set; }
        public int Quarter { get; set; }
        public int Possession { get; set; }
        public int FieldPosition { get; set; }             
        public int Down { get; set; }
        public int YardsToGo { get; set; }            
        public int RemainingSeconds { get; set; }                
        public long Sequence { get; set; }

        public NflGameState()
        {
            Sequence = 0;
            Quarter = 1;
            RemainingSeconds = 3600;
        }
    }

    public class NflTeam : Team
    {
        //public Guid QuarterBackId {get; set;}
        public bool ReceiveSecondHalf { get; set; }

        public NflTeam()
            {
                ReceiveSecondHalf = false;
           }
    }

    // todo rename
    public class Adjustment
    {
        public int MarketId { get; set; }
        public double Value { get; set; }
    }
}
