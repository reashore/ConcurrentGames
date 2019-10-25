using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Google.Cloud.PubSub.V1;
using log4net;
using Newtonsoft.Json;
using SportsIq.Analytica.Ncaafb;
using SportsIq.Distributor.Ncaafb;
using SportsIq.Models.Constants.Ncaafb;
using SportsIq.Models.Markets;
using SportsIq.Models.SportRadar.Ncaafb.GameEvents;
using SportsIq.Models.SportRadar.Ncaafb.GameInfo;
using SportsIq.NoSqlDataAccess;
using SportsIq.PubSub;
using SportsIq.SportsRadar.Ncaafb;
using SportsIq.SqlDataAccess.Ncaafb;
using SportsIq.Utilities;
using SportsIq.Pusher;
using static System.Convert;

namespace SportsIq.Games.Ncaafb
{
    public class NcaafbGame : GameBase<Team, NcaafbGameInfo>, IGame
    {
        private static readonly ILog Logger;
        private readonly IDataAccessNcaafb _dataAccessNcaafb;
        private readonly IRadarNcaafb _radarNcaafb;
        private readonly IAnalyticaNcaafb _analyticaNcaafb;
        private readonly IDatastore _datastore;
        private readonly IDistributorNcaafb _distributorNcaafb;
        private readonly IPubSubUtil _pubSubUtil;
        private NcaafbGameState NcaafbGameState { get; }
        private readonly IPusherUtil _pusherUtil;

        static NcaafbGame()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(NcaafbGame));
        }

        public NcaafbGame(IDataAccessNcaafb dataAccessNcaafb, IRadarNcaafb radarNcaafb, IAnalyticaNcaafb analyticaNcaafb, IDatastore datastore, IDistributorNcaafb distributorNcaafb, IPubSubUtil pubSubUtil, IPusherUtil pusherUtil)
        {
            Logger.Info("Starting");
            string isSimulationString = ConfigurationManager.AppSettings["isSimulation"];
            IsSimulation = ToBoolean(isSimulationString);

            PeriodList = new List<string> { "CG", "H1", "H2", "Q1", "Q2", "Q3", "Q4" };
            InitializePeriodScoring(PeriodList);

            ModelData[NcaafbModelDataKeys.InMlf] = new Dictionary<string, double>();           // PRE MATCH MARKET LINES
            ModelData[NcaafbModelDataKeys.InLMlf] = new Dictionary<string, double>();          // IN RUNNING MARKET LINES
            ModelData[NcaafbModelDataKeys.Egt] = new Dictionary<string, double>();             // EVENT GAME TIME
            ModelData[NcaafbModelDataKeys.Evs] = new Dictionary<string, double>();             // EVENT STATE
            ModelData[NcaafbModelDataKeys.InTsf] = new Dictionary<string, double>();           // TEAM STATS
            ModelData[NcaafbModelDataKeys.InSc] = new Dictionary<string, double>();            // SCORING CURVE
            ModelData[NcaafbModelDataKeys.Xs] = new Dictionary<string, double>();              // EXTRA STATS

            NcaafbGameState = new NcaafbGameState();

            _dataAccessNcaafb = dataAccessNcaafb;
            _analyticaNcaafb = analyticaNcaafb;
            _datastore = datastore;
            _distributorNcaafb = distributorNcaafb;
            _radarNcaafb = radarNcaafb;
            _pubSubUtil = pubSubUtil;
            _pusherUtil = pusherUtil;
        }

        #region Radar Event Subscriptions and Handlers

        public void AddRadarGameEventHandler()
        {
            _radarNcaafb.RadarGameEvent += HandleGameEvent;
        }

        public void RemoveRadarGameEventHandler()
        {
            _radarNcaafb.RadarGameEvent -= HandleGameEvent;
        }

        public TimeSpan GetTimeSinceLastGameEventOrHeartbeat()
        {
            TimeSpan timeSinceLastGameEventOrHeartbeat = DateTime.Now - _radarNcaafb.TimeOfLastRadarGameEventOrHeartbeat;
            return timeSinceLastGameEventOrHeartbeat;
        }

        private void HandleGameEvent(object sender, NcaafbGameEventEventArgs ncaafbGameEventEventArgs)
        {
            NcaafbGameEvent ncaafbGameEvent = ncaafbGameEventEventArgs.GameEvent;
            Guid gameId = ncaafbGameEvent.Payload.Game.Id;

            if (gameId == GameId)
            {
                //string eventType = ncaafbGameEvent.Metadata.EventType;

                // switch (eventType) {}
                //string message = $"Handling Radar game event: GameId = {gameId}, EventType = {eventType}";
                //Logger.Info(message);

                ProcessRadarGameEvent1(ncaafbGameEvent);
                ModelUpdateRequired = true;
            }
        }

        private void ProcessRadarGameEvent1(NcaafbGameEvent ncaafbGameEvent)
        {
            if (ncaafbGameEvent.Metadata == null)
            {
                Logger.Error("ProcessRadarGameEvent1(): game event metadata was null");
                return;
            }

            try
            {
                string eventType = ncaafbGameEvent.Metadata.EventType;
                string possession = "N";

                // todo create an object from NFL event definition
                NcaafbGameState.Status = ncaafbGameEvent.Metadata.Status;
                Event payloadEvent = ncaafbGameEvent.Payload.Event;
                Situation endSituation = payloadEvent.EndSituation;
                NcaafbGameState.YardsToGo = endSituation.Yfd;
                NcaafbGameState.Down = endSituation.Down;
                NcaafbGameState.Clock = endSituation.Clock;
                NcaafbGameState.Seconds = Utils.ConvertPeriodToGameString(payloadEvent.Period.Sequence, payloadEvent.EndSituation.Clock, 3600);
                NcaafbGameState.Possession = 0;

                if (ncaafbGameEvent.Payload.Event.EndSituation.Possession.Id == AwayTeam.TeamId)
                {
                    NcaafbGameState.Possession = 1;
                    possession = "V";
                }

                if (ncaafbGameEvent.Payload.Event.EndSituation.Possession.Id == HomeTeam.TeamId)
                {
                    NcaafbGameState.Possession = 2;
                    possession = "H";
                }

                NcaafbGameState.FieldPosition = ToInt32(ncaafbGameEvent.Payload.Event.EndSituation.Location.Yardline);
                NcaafbGameState.Quarter = ncaafbGameEvent.Payload.Game.Quarter;
                ModelData[NcaafbModelDataKeys.Egt] = new Dictionary<string, double>
                {
                    {"S", ToDouble(NcaafbGameState.Seconds)},
                    {"FP", ToDouble(NcaafbGameState.FieldPosition)},
                    {"DN", ToDouble(NcaafbGameState.Down)},
                    {"YF", ToDouble(NcaafbGameState.YardsToGo)},
                    {"P", ToDouble(NcaafbGameState.Possession)}
                };

                //ModelData[NcaafbModelDataKeys.Egt] = new Dictionary<string, double>();

                int awayScore = ncaafbGameEvent.Payload.Game.Summary.Away.Points;
                int homeScore = ncaafbGameEvent.Payload.Game.Summary.Home.Points;
                Dictionary<string, double> evsDictionary = ModelData[NcaafbModelDataKeys.Evs];

                // todo are the if checks necessary?
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

                evsDictionary["H,CG"] = homeScore;
                evsDictionary["V,CG"] = awayScore;

                switch (NcaafbGameState.Quarter)
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

                evsDictionary["H,H1"] = evsDictionary["H,Q1"] + evsDictionary["H,Q2"];
                evsDictionary["V,H1"] = evsDictionary["V,Q1"] + evsDictionary["V,Q2"];
                evsDictionary["H,H2"] = evsDictionary["H,Q3"] + evsDictionary["H,Q4"];
                evsDictionary["V,H2"] = evsDictionary["V,Q3"] + evsDictionary["V,Q4"];

                ModelUpdateRequired = true;
                string quarter = $"Q{NcaafbGameState.Quarter}";
                string clock = NcaafbGameState.Clock;

                switch (NcaafbGameState.Status)
                {
                    case "halftime":
                        quarter = "HT";
                        break;

                    case "closed":
                        quarter = "FN";
                        break;
                }

                if (eventType == "period_end")
                {
                    int qplus = NcaafbGameState.Quarter + 1;
                    quarter = $"Q{qplus}";
                    clock = "15:00";
                }

                string messageJson = $@"{{
                                        ""game"":               ""{GameId}"",
                                        ""Status"":             ""{NcaafbGameState.Status}"",
                                        ""away_score"":         {awayScore},
                                        ""home_score"":         {homeScore},
                                         ""away_q1"":           {evsDictionary["V,Q1"]},
                                        ""home_q1"":            {evsDictionary["H,Q1"]},
                                        ""away_q2"":            {evsDictionary["V,Q2"]},
                                        ""home_q2"":            {evsDictionary["H,Q2"]},
                                         ""away_q3"":           {evsDictionary["V,Q3"]},
                                        ""home_q3"":            {evsDictionary["H,Q3"]},
                                        ""away_q4"":            {evsDictionary["V,Q4"]},
                                        ""home_q4"":            {evsDictionary["H,Q4"]},
                                        ""period"":             ""{quarter}"",
                                        ""clock"":              ""{clock}"",
                                        ""possession"":         ""{possession}"",
                                        ""down"":               {NcaafbGameState.Down},
                                        ""Y2F"":                {NcaafbGameState.YardsToGo},
                                        ""FP"":                 {NcaafbGameState.FieldPosition}
                                    }}";

                if (!Utils.IsValidJson(messageJson))
                {
                    throw new Exception("JSON is invalid: {jsonString}");
                }

                string messageKey = GameId + "score";
                const string eventName = "NFLTEAM";
                Logger.Info(messageJson);
                _pusherUtil.SendScoreMessage(messageJson, eventName, messageKey);

            }
            catch (Exception exception)
            {
                Logger.Error(exception);
            }
            //NcaafbGameState.Quarter = ncaafbGameEvent.Metadata.Quarter;
            //NcaafbGameState.Possession = ncaafbGameEvent.Metadata.Possession;

            //Event payloadEvent = ncaafbGameEvent.Payload.Event;

            // check to make sure its moving forward
            //int sequence = payloadEvent.SequenceNumber;

            //if (sequence <= NcaafbGameState.Sequence)
            //{
            //    return;
            //}

            //NcaafbGameState.Sequence = sequence;
            //bool isHit = false;

            /* TO DO ADD PROCESSING LOGIC FOR NFL STREAMING EVENTS */
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

                case "test":
                    HandleTest(pubsubData);
                    break;

                default:
                    Logger.Error($"Could not handle PubSub message type = '{pubSubMessageType}'");
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

        private void HandleMarketOddsUpdate(string pubsubData, Guid gameId)
        {
            if (GameId == gameId)
            {
                Odds odds = JsonConvert.DeserializeObject<Odds>(pubsubData);
                ModelData[NcaafbModelDataKeys.InMlf] = ProcesOddsMessage(odds, ModelData[NcaafbModelDataKeys.InMlf]);
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
                ModelData[NcaafbModelDataKeys.InMlf] = gameOdds;
                ModelUpdateRequired = true;
                Logger.Info($"Updated manual odds for {GameId} ({Description})");
            }
        }

        private void HandleLiveMarketOddsUpdate(string pubsubData, Guid gameId)
        {
            if (GameId == gameId)
            {
                Odds odds = JsonConvert.DeserializeObject<Odds>(pubsubData);
                ModelData[NcaafbModelDataKeys.InLMlf] = ProcesOddsMessage(odds, ModelData[NcaafbModelDataKeys.InLMlf]);
                ModelUpdateRequired = true;
                Logger.Info($"Updated live odds for {GameId} ({Description})");
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
            InitialModelDataLoadComplete = true;
            LoadCompleteCountdownEvent.Signal();
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
            _analyticaNcaafb.IsTeamMode = IsTeamMode;
            Dictionary<int, List<Market>> result = _analyticaNcaafb.RunModel(ModelData, PeriodMarkets, Started, saveFile);
            _distributorNcaafb.SendMarkets(result, GameId, Description);
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
            ModelData[NcaafbModelDataKeys.InTsf] = GetInTsf();
            //ModelData[NcaafbModelDataKeys.InSc] = GetInSc();
            //ModelData[NcaafbModelDataKeys.Xs] = GetXs();  //new

            bool isGameStarted = StartDateTime < DateTime.UtcNow;

            if (isGameStarted)
            {
                LoadCurrentScore();
            }
        }

        private void LoadPlayerModelData()
        {
            GameInfo = _radarNcaafb.GetGameInfo(GameId);
            //LoadModelDataForPlayers();

            MarketOddsDictionary = _datastore.GetMarketOdds(GameId);
            LoadModelDataInMlf(MarketOddsDictionary);
            LoadModelDataEvs();
            //ModelData[NcaafbModelDataKeys.InTsf] = GetModelDataInTsf();

            bool isGameStarted = StartDateTime < DateTime.UtcNow;

            if (isGameStarted)
            {
                LoadCurrentScore();
            }
        }

        private void LoadCurrentScore()
        {
            NcaafbGameInfo ncaafbGameInfo = _radarNcaafb.GetGameInfo(GameId);
            double awayCg = 0;
            double homeCg = 0;
            double homeH1 = 0;
            double awayH1 = 0;
            double homeH2 = 0;
            double awayH2 = 0;

            foreach (PeriodType score in ncaafbGameInfo.Scoring.Quarter)
            {
                ModelData[NcaafbModelDataKeys.Evs].Add($"V,Q{score.Sequence}", ToDouble(score.Away_Points));
                ModelData[NcaafbModelDataKeys.Evs].Add($"H,Q{score.Sequence}", ToDouble(score.Home_Points));


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

            ModelData[NcaafbModelDataKeys.Evs].Add("V,CG", ToDouble(awayCg));
            ModelData[NcaafbModelDataKeys.Evs].Add("H,CG", ToDouble(homeCg));
            ModelData[NcaafbModelDataKeys.Evs].Add("V,H1", ToDouble(awayH1));
            ModelData[NcaafbModelDataKeys.Evs].Add("H,H1", ToDouble(homeH1));
            ModelData[NcaafbModelDataKeys.Evs].Add("V,H2", ToDouble(awayH2));
            ModelData[NcaafbModelDataKeys.Evs].Add("H,H2", ToDouble(homeH2));

            NcaafbGameState.YardsToGo = ToInt32(ncaafbGameInfo.Situation.Yfd);
            NcaafbGameState.Down = ToInt32(ncaafbGameInfo.Situation.Down);
            NcaafbGameState.Seconds = Utils.ConvertPeriodToGameString(ToInt32(ncaafbGameInfo.Quarter), ncaafbGameInfo.Situation.Clock, 3600);
            NcaafbGameState.Possession = Guid.Parse(ncaafbGameInfo.Summary.Away.Id) == AwayTeam.TeamId ? 0 : 1;
            NcaafbGameState.FieldPosition = ToInt32(ncaafbGameInfo.Situation.Location.Yardline);
            NcaafbGameState.Quarter = ToInt32(ncaafbGameInfo.Quarter);

            string possession;

            if (Guid.Parse(ncaafbGameInfo.Situation.Possession.Id) == AwayTeam.TeamId)
            {
                possession = "V";
            }

            if (Guid.Parse(ncaafbGameInfo.Situation.Possession.Id) == HomeTeam.TeamId)
            {
                possession = "H";
            }

            ModelData[NcaafbModelDataKeys.Egt] = new Dictionary<string, double>
            {
                {"S", ToDouble(NcaafbGameState.Seconds)},
                {"FP", ToDouble(NcaafbGameState.FieldPosition)},
                {"DN", ToDouble(NcaafbGameState.Down)},
                {"YF", ToDouble(NcaafbGameState.YardsToGo)},
                {"P", ToDouble(NcaafbGameState.Possession)}
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
        //    Dictionary<string, double> homeTtmDictionary = _dataAccessNcaafb.GetTtm(homeTeamId, side);

        //    Guid awayTeamId = AwayTeam.TeamId;
        //    side = "V";
        //    Dictionary<string, double> awayTtmDictionary = _dataAccessNcaafb.GetTtm(awayTeamId, side);

        //    foreach (NcaafbPlayer ncaafbPlayer in HomeTeam.PlayerList)
        //    {
        //        side = "H";
        //        Guid opponentTeamId = AwayTeam.TeamId;

        //        homePopDictionary = _dataAccessNcaafb.GetPop(ncaafbPlayer.PlayerId, ncaafbPlayer.Number, side);
        //        homePoscDictionary = _dataAccessNcaafb.GetPosc(ncaafbPlayer.PlayerId, ncaafbPlayer.Number, side);
        //        homePscoDictionary = _dataAccessNcaafb.GetPsco(ncaafbPlayer.PlayerId, ncaafbPlayer.Number, side);
        //        homeSdomDictionary = _dataAccessNcaafb.GetSdom(ncaafbPlayer.PlayerId, ncaafbPlayer.Number, side);
        //        homePotmDictionary = _dataAccessNcaafb.GetPotm(ncaafbPlayer.PlayerId, ncaafbPlayer.Number, side, opponentTeamId);
        //        homeSdvtmDictionary = _dataAccessNcaafb.GetSdvtm(ncaafbPlayer.PlayerId, ncaafbPlayer.Number, side, opponentTeamId);
        //    }

        //    foreach (NcaafbPlayer ncaafbPlayer in AwayTeam.PlayerList)
        //    {
        //        side = "V";
        //        Guid opponentTeamId = HomeTeam.TeamId;

        //        awayPopDictionary = _dataAccessNcaafb.GetPop(ncaafbPlayer.PlayerId, ncaafbPlayer.Number, side);
        //        awayPoscDictionary = _dataAccessNcaafb.GetPosc(ncaafbPlayer.PlayerId, ncaafbPlayer.Number, side);
        //        awayPscoDictionary = _dataAccessNcaafb.GetPsco(ncaafbPlayer.PlayerId, ncaafbPlayer.Number, side);
        //        awaySdomDictionary = _dataAccessNcaafb.GetSdom(ncaafbPlayer.PlayerId, ncaafbPlayer.Number, side);
        //        awayPotmDictionary = _dataAccessNcaafb.GetPotm(ncaafbPlayer.PlayerId, ncaafbPlayer.Number, side, opponentTeamId);
        //        awaySdvtmDictionary = _dataAccessNcaafb.GetSdvtm(ncaafbPlayer.PlayerId, ncaafbPlayer.Number, side, opponentTeamId);
        //    }

        //    ModelData[NcaafbModelDataKeys.Pop] = Utils.MergeDictionaries(homePopDictionary, awayPopDictionary);
        //    ModelData[NcaafbModelDataKeys.Posc] = Utils.MergeDictionaries(homePoscDictionary, awayPoscDictionary);
        //    ModelData[NcaafbModelDataKeys.Psco] = Utils.MergeDictionaries(homePscoDictionary, awayPscoDictionary);
        //    ModelData[NcaafbModelDataKeys.Sdom] = Utils.MergeDictionaries(homeSdomDictionary, awaySdomDictionary);
        //    ModelData[NcaafbModelDataKeys.Potm] = Utils.MergeDictionaries(homePotmDictionary, awayPotmDictionary);
        //    ModelData[NcaafbModelDataKeys.Sdvtm] = Utils.MergeDictionaries(homeSdvtmDictionary, awaySdvtmDictionary);
        //    ModelData[NcaafbModelDataKeys.Ttm] = Utils.MergeDictionaries(homeTtmDictionary, awayTtmDictionary);
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
            if (marketOddsDictionary.Count > 0)
            {
                ModelData[NcaafbModelDataKeys.InMlf] = marketOddsDictionary;
            }
            else
            {
                Dictionary<string, double> dictionary = ModelData[NcaafbModelDataKeys.InMlf];
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
            //ModelData[NcaafbModelDataKeys.Egt] = dictionaryEgt;
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
            Dictionary<string, double> dictionary1 = _dataAccessNcaafb.GetTeamInTssFge(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary2 = _dataAccessNcaafb.GetTeamInTssFge(HomeTeam.TeamId, "V");
            Dictionary<string, double> dictionary3 = _dataAccessNcaafb.GetTeamInTssFge(AwayTeam.TeamId, "H");
            Dictionary<string, double> dictionary4 = _dataAccessNcaafb.GetTeamInTssFge(AwayTeam.TeamId, "V");

            Dictionary<string, double> dictionary5 = _dataAccessNcaafb.GetTeamInTss(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary6 = _dataAccessNcaafb.GetTeamInTss(HomeTeam.TeamId, "V");
            Dictionary<string, double> dictionary7 = _dataAccessNcaafb.GetTeamInTss(AwayTeam.TeamId, "H");
            Dictionary<string, double> dictionary8 = _dataAccessNcaafb.GetTeamInTss(AwayTeam.TeamId, "V");

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

            Dictionary<string, double> dictionary1 = _dataAccessNcaafb.GetTeamInSc(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary2 = _dataAccessNcaafb.GetTeamInSc(HomeTeam.TeamId, "V");
            Dictionary<string, double> dictionary3 = _dataAccessNcaafb.GetTeamInSc(AwayTeam.TeamId, "H");
            Dictionary<string, double> dictionary4 = _dataAccessNcaafb.GetTeamInSc(AwayTeam.TeamId, "V");

            result = Utils.MergeDictionaries(dictionary1, result);
            result = Utils.MergeDictionaries(dictionary2, result);
            result = Utils.MergeDictionaries(dictionary3, result);
            result = Utils.MergeDictionaries(dictionary4, result);

            return result;
        }

        private Dictionary<string, double> GetXs()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            //todo add data access for Xs
            /*
            Dictionary<string, double> dictionary1 = _dataAccessNcaafb.GetTeamXs(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary2 = _dataAccessNcaafb.GetTeamXs(HomeTeam.TeamId, "V");
            Dictionary<string, double> dictionary3 = _dataAccessNcaafb.GetTeamXs(AwayTeam.TeamId, "H");
            Dictionary<string, double> dictionary4 = _dataAccessNcaafb.GetTeamXs(AwayTeam.TeamId, "V");

            result = Utils.MergeDictionaries(dictionary1, result);
            result = Utils.MergeDictionaries(dictionary2, result);
            result = Utils.MergeDictionaries(dictionary3, result);
            result = Utils.MergeDictionaries(dictionary4, result);
            */

            return result;
        }

        private Dictionary<string, double> GetInTsf()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            Dictionary<string, double> dictionary1 = _dataAccessNcaafb.GetTeamIntsf(HomeTeam.TeamId, "H");
            Dictionary<string, double> dictionary2 = _dataAccessNcaafb.GetTeamIntsf(HomeTeam.TeamId, "V");
            Dictionary<string, double> dictionary3 = _dataAccessNcaafb.GetTeamIntsf(AwayTeam.TeamId, "H");
            Dictionary<string, double> dictionary4 = _dataAccessNcaafb.GetTeamIntsf(AwayTeam.TeamId, "V");

            result = Utils.MergeDictionaries(dictionary1, result);
            result = Utils.MergeDictionaries(dictionary2, result);
            result = Utils.MergeDictionaries(dictionary3, result);
            result = Utils.MergeDictionaries(dictionary4, result);

            return result;
        }

        #endregion
    }

    public class NcaafbGameState
    {
        public string Status { get; set; }
        public int Period { get; set; }
        public string Clock { get; set; }
        public int Quarter { get; set; }
        public int Possession { get; set; }
        public int FieldPosition { get; set; }              //field position
        public int Down { get; set; }
        public int YardsToGo { get; set; }                  //yards to go
        public int Seconds { get; set; }                    //time left in seconds
        public long Sequence { get; set; }

        public NcaafbGameState()
        {
            Sequence = 0;
        }
    }

    //public class NcaafbPeriodScore
    //{
    //    public int Away { get; set; }
    //    public int Home { get; set; }
    //}
}
