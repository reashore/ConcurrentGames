using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Google.Cloud.PubSub.V1;
using log4net;
using Newtonsoft.Json;
using SportsIq.Analytica.Nhl;
using SportsIq.Distributor.Nhl;
using SportsIq.Models.Constants.Nhl;
using SportsIq.Models.Markets;
using SportsIq.Models.SportRadar.Nhl.GameEvents;
using SportsIq.Models.SportRadar.Nhl.GameInfo;
using SportsIq.NoSqlDataAccess;
using SportsIq.PubSub;
using SportsIq.Pusher;
using SportsIq.SportsRadar.Nhl;
using SportsIq.SqlDataAccess.Nhl;
using SportsIq.Utilities;
using static System.Convert;

namespace SportsIq.Games.Nhl
{
    public class NhlGame : GameBase<Team, NhlGameInfo>, IGame
    {
        private static readonly ILog Logger;
        private readonly IAnalyticaNhl _analyticaNhl;
        private readonly IDataAccessNhl _dataAccessNhl;
        private readonly IRadarNhl _radarNhl;
        private readonly IDatastore _datastore;
        private readonly IDistributorNhl _distributorNhl;
        private readonly IPubSubUtil _pubSubUtil;
        private readonly IPusherUtil _pusherUtil;
        private NhlGameState NhlGameState { get; }

        static NhlGame()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(NhlGame));
        }

        public NhlGame(IDataAccessNhl dataAccessNhl, IRadarNhl radarNhl, IAnalyticaNhl analyticaNhl, IDatastore datastore, IDistributorNhl distributorNhl, IPubSubUtil pubSubUtil, IPusherUtil pusherUtil)
        {
            PeriodList = new List<string> { "P0", "P1", "P2", "P3", "P4" };
            InitializePeriodScoring(PeriodList);

            ModelData[NhlModelDataKeys.InMlf] = new Dictionary<string, double>();
            ModelData[NhlModelDataKeys.Egt] = new Dictionary<string, double>();
            ModelData[NhlModelDataKeys.Evs] = new Dictionary<string, double>();
            ModelData[NhlModelDataKeys.InTsf] = new Dictionary<string, double>();
            ModelData[NhlModelDataKeys.InPp] = new Dictionary<string, double>();

            ModelData[NhlModelDataKeys.Poi] = new Dictionary<string, double>();
            ModelData[NhlModelDataKeys.Pib] = new Dictionary<string, double>();
            ModelData[NhlModelDataKeys.ScoreS] = new Dictionary<string, double>();
            ModelData[NhlModelDataKeys.Egt] = new Dictionary<string, double>();
            ModelData[NhlModelDataKeys.InPp] = new Dictionary<string, double>();
            ModelData[NhlModelDataKeys.ScoreG] = new Dictionary<string, double>();
            ModelData[NhlModelDataKeys.InSgp] = new Dictionary<string, double>();
            ModelData[NhlModelDataKeys.InSsl] = new Dictionary<string, double>();
            ModelData[NhlModelDataKeys.InSst] = new Dictionary<string, double>();
            ModelData[NhlModelDataKeys.InSsg] = new Dictionary<string, double>();
            ModelData[NhlModelDataKeys.InGgp] = new Dictionary<string, double>();
            ModelData[NhlModelDataKeys.InGsl] = new Dictionary<string, double>();
            ModelData[NhlModelDataKeys.InGst] = new Dictionary<string, double>();



            NhlGameState = new NhlGameState();

            _dataAccessNhl = dataAccessNhl;
            _radarNhl = radarNhl;
            _analyticaNhl = analyticaNhl;
            _datastore = datastore;
            _distributorNhl = distributorNhl;
            _pubSubUtil = pubSubUtil;
            _pusherUtil = pusherUtil;
        }

        #region Radar Event Subscriptions and Handlers

        public TimeSpan GetTimeSinceLastGameEventOrHeartbeat()
        {
            TimeSpan timeSinceLastGameEventOrHeartbeat = DateTime.Now - _radarNhl.TimeOfLastRadarGameEventOrHeartbeat;
            return timeSinceLastGameEventOrHeartbeat;
        }

        public void AddRadarGameEventHandler()
        {
            _radarNhl.RadarGameEvent += HandleRadarGameEvent;
        }

        public void RemoveRadarGameEventHandler()
        {
            _radarNhl.RadarGameEvent -= HandleRadarGameEvent;
        }

        private void HandleRadarGameEvent(object sender, NhlGameEventEventArgs nhlGameEventEventArgs)
        {
            NhlGameEvent nhlGameEvent = nhlGameEventEventArgs.GameEvent;
            Guid gameId = nhlGameEvent.Payload.Game.Id;

            if (gameId == GameId)
            {
                string eventType = nhlGameEvent.Payload.Event.EventType;

                switch (eventType)
                {
                    case "event_over":
                        GameOver = true;
                        Logger.Info($"Game over        {GameId} ({Description}) ******************************************************");
                        break;

                    default:
                        Logger.Info($"Radar  Event for {GameId} ({Description}) of type = {eventType}");
                        break;
                }

                // what radar events trigger these calls
                ProcessRadarGameEvent(nhlGameEvent);
                ModelUpdateRequired = true; //move into the handler to choose when to update
            }
        }

        private void ProcessRadarGameEvent(NhlGameEvent nhlGameEvent)
        {
            if (nhlGameEvent.Metadata == null)
            {
                return;
            }

            try
            {

                Dictionary<string, double> ScoreGDictionary = ModelData[NhlModelDataKeys.ScoreG];
                Dictionary<string, double> ScoreSDictionary = ModelData[NhlModelDataKeys.ScoreS];
                Dictionary<string, double> inppDictionary = ModelData[NhlModelDataKeys.InPp];
                Dictionary<string, double> evsDictionary = ModelData[NhlModelDataKeys.Evs];

                Event payloadEvent = nhlGameEvent.Payload.Event;
                Game payloadGame = nhlGameEvent.Payload.Game;

                NhlGameState.Status = nhlGameEvent.Metadata.Status;
                if (payloadEvent.Period != null)
                {
                    NhlGameState.Period = nhlGameEvent.Payload.Event.Period.Sequence;
                }

                if (nhlGameEvent.Payload.Game.Away != null)
                {
                    NhlGameState.Away = nhlGameEvent.Payload.Game.Away.Points;
                    NhlGameState.Home = nhlGameEvent.Payload.Game.Home.Points;
                }

                NhlGameState.Clock = payloadEvent.Clock;

                List<Guid> homePenaltyPlayers = new List<Guid>();
                List<Guid> awayPenaltyPlayers = new List<Guid>();

                List<Guid> homeOnIcePlayers = new List<Guid>();
                List<Guid> awayOnIcePlayers = new List<Guid>();
                //NhlGameState
                List<Guid> homeGoalie = new List<Guid>();
                List<Guid> awayGoalie = new List<Guid>();

                int count = 0;
                // Guid homeGoalie;
                // Guid awayGoalie;

                if (NhlGameState.Period > 3)
                {
                    return;

                }

                string eventType = nhlGameEvent.Payload.Event.EventType;
                switch (eventType)
                {
                    case "penalty": /* NEW   PENALTY   */

                        Penalty playerPenalty = new Penalty();
                        playerPenalty.duration = payloadEvent.Duration;
                        playerPenalty.startSeconds =
                            Utils.ConvertPeriodToGameStringNHL(NhlGameState.Period, payloadEvent.Clock,
                                2700); //payloadEvent.Clock;
                        playerPenalty.endSeconds = playerPenalty.startSeconds + (payloadEvent.Duration * 60);

                        //penalty side
                        string side = "H";
                        if (payloadEvent.Attribution.Id == AwayTeam.TeamId)
                        {
                            side = "V";
                        }


                        /* Find the Penalty Player */
                        foreach (Statistic playerStat in payloadEvent.Statistics)
                        {
                            if (playerStat.Type == "penalty")
                            {
                                Guid penaltyPlayer = playerStat.Player.Id;
                                playerPenalty.player_id = penaltyPlayer;
                            }
                        }


                        //check to see if the player is in the penalty list already
                        Penalty p = NhlGameState.AwayPenalties.Find(s => s.event_id == payloadEvent.Id);
                        if (p == null && side == "V")
                        {
                            NhlGameState.AwayPenalties.Add(playerPenalty);
                        }

                        p = NhlGameState.HomePenalties.Find(s => s.event_id == payloadEvent.Id);
                        if (p == null && side == "H")
                        {
                            NhlGameState.HomePenalties.Add(playerPenalty);
                        }

                        if (!IsTeamMode)
                        {
                            //props mode only
                            if (payloadEvent.InPenalty.Count > 0)
                            {
                                foreach (OnIce team in payloadEvent.InPenalty)
                                {
                                    if (team.Team.Id == AwayTeam.TeamId)
                                    {
                                        foreach (PlayerElement pp in team.Team.Players)
                                        {
                                            if (pp.Id != null)
                                            {
                                                awayPenaltyPlayers.Add(pp.Id);
                                            }
                                        }
                                    }

                                    if (team.Team.Id == HomeTeam.TeamId)
                                    {
                                        foreach (PlayerElement pp in team.Team.Players)
                                        {
                                            if (pp.Id != null)
                                            {
                                                homePenaltyPlayers.Add(pp.Id);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        break;


                    case "shotsaved":
                        //get team
                        if (payloadEvent.Statistics != null & !IsTeamMode)
                        {
                            foreach (Statistic playerStat in payloadEvent.Statistics)
                            {
                                if (playerStat.Type == "shotagainst")
                                {
                                    Guid savePlayer = playerStat.Player.Id;
                                    Guid saveTeam = playerStat.Team.Id;
                                    //todo filter duplicates
                                    if (AwayTeam.TeamId == saveTeam)
                                    {
                                        Player pp = AwayTeam.PlayerList.Find(x => x.PlayerId == savePlayer);
                                        pp.playerStats["SA"][NhlGameState.Period] += 1;
                                        pp.playerStats["SV"][NhlGameState.Period] += 1;
                                    }

                                    //playerPenalty.player_id = penaltyPlayer;
                                }
                            }
                        }

                        break;

                    case "shotmissed":
                        if (payloadEvent.Statistics != null & !IsTeamMode)
                        {
                            foreach (Statistic playerStat in payloadEvent.Statistics)
                            {
                                if (playerStat.Type == "block")
                                {
                                    Guid savePlayer = playerStat.Player.Id;
                                    Guid saveTeam = playerStat.Team.Id;
                                    //todo filter duplicates
                                    if (AwayTeam.TeamId == saveTeam)
                                    {
                                        Player player = AwayTeam.PlayerList.Find(x => x.PlayerId == savePlayer);
                                        player.playerStats["B"][NhlGameState.Period] += 1;
                                    }
                                    else
                                    {
                                        Player player = HomeTeam.PlayerList.Find(x => x.PlayerId == savePlayer);
                                        player.playerStats["B"][NhlGameState.Period] += 1;
                                    }
                                }
                            }
                        }

                        break;

                    case "goal":
                        //get team
                        if (payloadEvent.Statistics != null)
                        {
                            foreach (Statistic playerStat in payloadEvent.Statistics)
                            {
                                if (playerStat.Type == "shotagainst")
                                {
                                    Guid savePlayer = playerStat.Player.Id;
                                    Guid saveTeam = playerStat.Team.Id;
                                    //todo filter duplicates
                                    if (AwayTeam.TeamId == saveTeam)
                                    {
                                        Player pp = AwayTeam.PlayerList.Find(x => x.PlayerId == savePlayer);
                                        pp.playerStats["SA"][NhlGameState.Period] += 1;
                                    }
                                    else
                                    {
                                        Player pp = HomeTeam.PlayerList.Find(x => x.PlayerId == savePlayer);
                                        pp.playerStats["SA"][NhlGameState.Period] += 1;
                                    }
                                }


                                if (playerStat.Type == "shot" && playerStat.Goal && playerStat.Player != null)
                                {
                                    Guid savePlayer = playerStat.Player.Id;
                                    Guid saveTeam = playerStat.Team.Id;
                                    //todo filter duplicates
                                    if (AwayTeam.TeamId == saveTeam)
                                    {
                                        Player pp = AwayTeam.PlayerList.Find(x => x.PlayerId == savePlayer);
                                        pp.playerStats["G"][NhlGameState.Period] += 1;
                                        pp.playerStats["P"][NhlGameState.Period] += 1;
                                    }
                                    else
                                    {
                                        Player pp = HomeTeam.PlayerList.Find(x => x.PlayerId == savePlayer);
                                        pp.playerStats["G"][NhlGameState.Period] += 1;
                                        pp.playerStats["P"][NhlGameState.Period] += 1;
                                    }
                                }

                                if (playerStat.Type == "assist")
                                {
                                    Guid savePlayer = playerStat.Player.Id;
                                    Guid saveTeam = playerStat.Team.Id;
                                    //todo filter duplicates
                                    if (AwayTeam.TeamId == saveTeam)
                                    {
                                        Player pp = AwayTeam.PlayerList.Find(x => x.PlayerId == savePlayer);
                                        if (pp != null)
                                        {
                                            pp.playerStats["A"][NhlGameState.Period] += 1;
                                            pp.playerStats["P"][NhlGameState.Period] += 1;
                                        }
                                    }
                                    else
                                    {
                                        Player pp = HomeTeam.PlayerList.Find(x => x.PlayerId == savePlayer);
                                        if (pp != null)
                                        {
                                            pp.playerStats["A"][NhlGameState.Period] += 1;
                                            pp.playerStats["P"][NhlGameState.Period] += 1;
                                        }
                                    }
                                }
                            }
                        }

                        break;


                }

                Dictionary<string, double> egtDictionary = ModelData[NhlModelDataKeys.Egt];
                egtDictionary["T"] = NhlGameState.Seconds;
                egtDictionary["VP"] = awayPenaltyPlayers.Count;
                egtDictionary["HP"] = homePenaltyPlayers.Count;

                //END PLAYER PENALTIES AND GAME SECONDS

                //PLAYERS ON ICE
                if (payloadEvent.OnIce != null)
                {
                    foreach (OnIce team in payloadEvent.OnIce)
                    {
                        if (team.Team.Id == AwayTeam.TeamId)
                        {
                            foreach (PlayerElement player in team.Team.Players)
                            {
                                if (player.Position != Position.G)
                                {
                                    awayOnIcePlayers.Add(player.Id);
                                }
                                else
                                {
                                    awayGoalie.Add(player.Id);
                                }
                            }
                        }

                        if (team.Team.Id == HomeTeam.TeamId)
                        {
                            foreach (PlayerElement player in team.Team.Players)
                            {
                                if (player.Position != Position.G)
                                {
                                    homeOnIcePlayers.Add(player.Id);
                                }
                                else
                                {
                                    homeGoalie.Add(player.Id);
                                }
                            }
                        }
                    }

                    if (!IsTeamMode)
                    {
                        Dictionary<string, double> poiDictionary = ModelData[NhlModelDataKeys.Poi];
                        count = 1;
                        foreach (Guid homePlayerID in homeOnIcePlayers)
                        {
                            Player siqHomePlayer = HomeTeam.PlayerList.Find(x => x.PlayerId == homePlayerID);
                            if (siqHomePlayer != null)
                            {
                                poiDictionary[$"H,{count}"] = siqHomePlayer.Number;
                            }

                            count++;
                        }

                        count = 1;
                        foreach (Guid awayPlayerID in awayOnIcePlayers)
                        {
                            Player siqPlayer = HomeTeam.PlayerList.Find(x => x.PlayerId == awayPlayerID);
                            if (siqPlayer != null)
                            {
                                poiDictionary[$"V,{count}"] = siqPlayer.Number;
                            }

                            count++;
                        }
                    }
                }

                //PLAYERS IN BOX
                if (!IsTeamMode)
                {
                    Dictionary<string, double> pibDictionary = ModelData[NhlModelDataKeys.Pib];
                    count = 1;
                    foreach (Guid homePlayerID in homePenaltyPlayers)
                    {
                        Player siqHomePlayer = HomeTeam.PlayerList.Find(x => x.PlayerId == homePlayerID);
                        if (siqHomePlayer != null)
                        {
                            pibDictionary[$"H,{count}"] = siqHomePlayer.Number;
                        }

                        count++;
                    }

                    count = 1;
                    foreach (Guid awayPlayerID in awayPenaltyPlayers)
                    {
                        Player siqPlayer = HomeTeam.PlayerList.Find(x => x.PlayerId == awayPlayerID);
                        if (siqPlayer != null)
                        {
                            pibDictionary[$"V,{count}"] = siqPlayer.Number;
                        }

                        count++;
                    }


                    //PENALTY END TIMES
                    count = 1;

                    foreach (Guid awayPlayerID in awayPenaltyPlayers)
                    {
                        Penalty siqPenalty = NhlGameState.AwayPenalties.Find(x =>
                            x.player_id == awayPlayerID && x.endSeconds >= NhlGameState.Seconds);
                        if (siqPenalty != null)
                        {
                            Player siqPlayer = AwayTeam.PlayerList.Find(x => x.PlayerId == siqPenalty.player_id);

                            if (siqPlayer != null)
                            {
                                inppDictionary[$"V,{count}"] = siqPenalty.endSeconds;
                            }
                        }

                        count++;
                    }

                    count = 1;
                    foreach (Guid playerID in homePenaltyPlayers)
                    {
                        Penalty siqPenalty = NhlGameState.AwayPenalties.Find(x =>
                            x.player_id == playerID && x.endSeconds >= NhlGameState.Seconds);
                        if (siqPenalty != null)
                        {
                            Player siqPlayer = HomeTeam.PlayerList.Find(x => x.PlayerId == siqPenalty.player_id);

                            if (siqPlayer != null)
                            {
                                inppDictionary[$"H,{count}"] = siqPenalty.endSeconds;
                            }
                        }

                        count++;
                    }

                    //PLAYER SCORE 


                    List<string> stats = new List<string>();
                    stats.Add("A");
                    stats.Add("B");
                    stats.Add("P");
                    stats.Add("G");

                    //UPDATE THE PLAYERS THAT ARE CURRENTLY ON ICE
                    foreach (Player player in HomeTeam.PlayerList)
                    {
                        foreach (string stat in stats)
                        {
                            if (player.playerStats.ContainsKey(stat))
                            {
                                foreach (KeyValuePair<int, double> statEl in player.playerStats[stat])
                                {
                                    string key = $"H,P{statEl.Key},{stat},P{player.Number}";
                                    double value = statEl.Value;
                                    ScoreSDictionary[key] = value;
                                }
                            }
                        }
                    }

                    foreach (Player player in AwayTeam.PlayerList)
                    {
                        foreach (string stat in stats)
                        {
                            if (player.playerStats.ContainsKey(stat))
                            {
                                foreach (KeyValuePair<int, double> statEl in player.playerStats[stat])
                                {
                                    string key = $"V,P{statEl.Key},{stat},P{player.Number}";
                                    double value = statEl.Value;
                                    ScoreSDictionary[key] = value;
                                }
                            }
                        }
                    }


                    //GOALIE SCORE 


                    stats = new List<string>();
                    stats.Add("SA");
                    stats.Add("SV");

                    //GOALIE SCORE BOARD
                    if (homeGoalie.Count != 0)
                    {
                        Player player = HomeTeam.PlayerList.Find(x => x.PlayerId == homeGoalie[0]);
                        if (player != null)
                        {
                            foreach (string stat in stats)
                            {
                                if (player.playerStats.ContainsKey(stat))
                                {
                                    foreach (KeyValuePair<int, double> statEl in player.playerStats[stat])
                                    {
                                        string key = $"H,P{statEl.Key},{stat}";
                                        double value = statEl.Value;
                                        ScoreGDictionary[key] = value;
                                    }
                                }
                            }
                        }
                    }


                    if (awayGoalie.Count != 0)
                    {
                        Player player = HomeTeam.PlayerList.Find(x => x.PlayerId == awayGoalie[0]);
                        if (player != null)
                        {
                            foreach (string stat in stats)
                            {
                                if (player.playerStats.ContainsKey(stat))
                                {
                                    foreach (KeyValuePair<int, double> statEl in player.playerStats[stat])
                                    {
                                        string key = $"V,P{statEl.Key},{stat}";
                                        double value = statEl.Value;
                                        ScoreGDictionary[key] = value;
                                    }
                                }
                            }
                        }
                    }


                }

                //EVS   EVENT SCORE
                double awayScoreTest = 0;
                double homeScoreTest = 0;


                if (NhlGameState.Away != 0)
                {
                    foreach (KeyValuePair<string, double> scoreItem in evsDictionary)
                    {
                        if (scoreItem.Key == "V,P0")
                        {
                            continue;
                        }

                        if (scoreItem.Key.Contains("V"))
                        {
                            if (!scoreItem.Key.Contains($"P{NhlGameState.Period}"))
                            {
                                awayScoreTest += scoreItem.Value;
                            }
                        }


                    }
                }

                //fix period score incorrect score
                if (Utils.AreNotEqual(NhlGameState.Away, awayScoreTest))
                {
                    string key = $"V,P{NhlGameState.Period}";
                    evsDictionary[key] = NhlGameState.Away - awayScoreTest;
                }


                if (NhlGameState.Home != 0)
                {
                    foreach (KeyValuePair<string, double> scoreItem in evsDictionary)
                    {
                        if (scoreItem.Key == "H,P0")
                        {
                            continue;
                        }

                        if (scoreItem.Key.Contains("H"))
                        {
                            if (!scoreItem.Key.Contains($"P{NhlGameState.Period}"))
                            {
                                homeScoreTest += scoreItem.Value;
                            }
                        }
                    }
                }

                //fix period score incorrect score
                if (Utils.AreNotEqual(NhlGameState.Home, homeScoreTest))
                {
                    string key = $"H,P{NhlGameState.Period}";
                    evsDictionary[key] = NhlGameState.Home - homeScoreTest;
                }







                string awayOnIceString = JsonConvert.SerializeObject(awayOnIcePlayers);
                string awayInBoxString = JsonConvert.SerializeObject(awayPenaltyPlayers);
                string homeOnIceString = JsonConvert.SerializeObject(homeOnIcePlayers);
                string homeInBoxString = JsonConvert.SerializeObject(homePenaltyPlayers);
                //string goalieString = JsonConvert.SerializeObject(ScoreGDictionary);
                //string playerString = JsonConvert.SerializeObject(ScoreSDictionary);
                string evsString = JsonConvert.SerializeObject(evsDictionary);
                string inppString = JsonConvert.SerializeObject(inppDictionary);

                // messageJsonObj = new 

                string messageJson = $@"{{
                                        ""game"":""{GameId}"",
                                        ""away_score"":{NhlGameState.Away},
                                        ""away_onIce"":{awayOnIceString},
                                        ""away_penalty"":{awayInBoxString},
                                        ""home_onIce"":{homeOnIceString},
                                        ""home_penalty"":{homeInBoxString},
                                        ""home_score"":{NhlGameState.Home},
                                        ""period"":{NhlGameState.Period},
                                        ""clock"":""{NhlGameState.Clock}"",
                                        ""event_state"":{ evsString},
                                        ""penalty_end"":{inppString}
                                        }}";


                ////""goalie_score"":{ goalieString},
                //                ""player_score"":{ playerString},

                if (!Utils.IsValidJson(messageJson))
                {
                    throw new Exception("JSON is invalid: {jsonString}");
                }

                //    Logger.Info(messageJson);

                const string eventName = "NHLTEAM";
                _pusherUtil.SendScoreMessage(messageJson, eventName, GameId.ToString());
                ModelUpdateRequired = true;
            }
            catch (Exception e)
            {
                Logger.Info(e);

            }
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
            Guid gameId = Utils.ParseGuid(gameIdString);
            string pubsubData = pubsubMessage.Data.ToStringUtf8();

            //Logger.Info($"PubSub Event for {GameId} of type = {pubSubMessageType}");

            switch (pubSubMessageType)
            {
                //case "pitcher_update":
                //    HandlePitcherUpdate(pubsubData);
                //    break;

                case "marketOdds":
                    HandleMarketOddsUpdate(pubsubData, gameId);
                    break;

                case "odds":  //odds from puncher
                    HandleOddsUpdate(pubsubData, gameId);
                    break;

                //     case "liveOdds":    NO LIVE ODDS INPUT CURRENTLY
                //         HandleLiveMarketOddsUpdate(pubsubData, gameId);
                //         break;

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


        private void HandleMarketOddsUpdate(string pubsubData, Guid gameId)
        {
            if (GameId == gameId)
            {
                Odds odds = JsonConvert.DeserializeObject<Odds>(pubsubData);
                ModelData[NhlModelDataKeys.InMlf] = ProcesOddsMessage(odds, ModelData[NhlModelDataKeys.InMlf]);
                ModelUpdateRequired = true;
                Logger.Info($"Updated market odds for {GameId} ({Description})");
            }
        }

        private void HandleOddsUpdate(string pubsubData, Guid gameId)
        {
            if (GameId == gameId)
            {
                Dictionary<string, double> gameOdds = DeserializeData<Dictionary<string, double>>(pubsubData);
                ModelData[NhlModelDataKeys.InMlf] = gameOdds;
                ModelUpdateRequired = true;
                Logger.Info($"Updated manual odds for {GameId} ({Description})");
            }
        }

        /* LIVE ODDS UPDATE
        private void HandleLiveMarketOddsUpdate(string pubsubData, Guid gameId)
        {
            if (GameId == gameId)
            {
                Odds odds = JsonConvert.DeserializeObject<Odds>(pubsubData);
                ModelData[NhlModelDataKeys.InLMlf] = ProcesOddsMessage(odds, ModelData[NhlModelDataKeys.InLMlf]);
                ModelUpdateRequired = true;
                Logger.Info($"Updated live odds for {GameId} ({Description})");
            }
        }*/

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
            //   ModelUpdateRequired = true;
        }

        public void RunModel()
        {
            if (!InitialModelDataLoadComplete)
            {
                const string message = "LoadModelData() must be called before RunModel()";
                Logger.Error(message);
                throw new Exception(message);
            }
            Logger.Info($"running model {AwayTeam.ShortName}  {HomeTeam.ShortName}");
            // clear periodMarkets each time
            PeriodMarkets.Clear();
            string saveFile = GetSaveFullFileName();
            _analyticaNhl.IsTeamMode = IsTeamMode;
            Dictionary<int, List<Market>> result = _analyticaNhl.RunModel(ModelData, PeriodMarkets, Started, saveFile);
            Logger.Info($"Sending {AwayTeam.ShortName}  {HomeTeam.ShortName}");
            if (IsTeamMode)
            {
                _distributorNhl.SendMarkets(result, GameId, Description, ModelData[NhlModelDataKeys.Egt]);
            }
            else
            {
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
                                Player playerA = AwayTeam.PlayerList.Find(x => x.Number == number);
                                if (market.MarketRunnerList[0].Price.IsNotEqualToZero() &&
                                    market.MarketRunnerList[0].Price.IsNotEqualTo(1))
                                {
                                    if (playerA.Stats != null && market.Tp != null &&
                                        !playerA.Stats.ContainsKey(market.Tp))
                                    {
                                        playerA.Stats.Add(market.Tp, new OddsAndStats());
                                        playerA.Side = side;
                                        playerA.comp_id = Convert.ToString(GameId);
                                        playerA.team = Convert.ToString(AwayTeam.TeamId);
                                        playerA.player = playerA.FullName;
                                    }

                                    playerA.Stats[market.Tp].odds.Add(market);
                                }

                                break;

                            case "home":
                                Player playerH = HomeTeam.PlayerList.Find(x => x.Number == number);
                                if (market.MarketRunnerList[0].Price.IsNotEqualToZero() &&
                                    market.MarketRunnerList[0].Price.IsNotEqualTo(1))
                                {
                                    if (playerH.Stats != null && market.Tp != null &&
                                        !playerH.Stats.ContainsKey(market.Tp))
                                    {
                                        playerH.Stats.Add(market.Tp, new OddsAndStats());
                                        playerH.Side = side;
                                        playerH.comp_id = Convert.ToString(GameId);
                                        playerH.team = Convert.ToString(HomeTeam.TeamId);
                                        playerH.player = playerH.FullName;
                                    }

                                    playerH.Stats[market.Tp].odds.Add(market);
                                }

                                break;
                        }
                    }


                    List<Player> playersMessages = new List<Player>();
                    List<string> types = new List<string> { "P", "G", "A", "B", "GA" };

                    foreach (Player player in HomeTeam.PlayerList)
                    {
                        foreach (KeyValuePair<string, OddsAndStats> playerStatMarket in player.Stats)
                        {
                            playerStatMarket.Value.MainMarket =
                                playerStatMarket.Value.odds.OrderBy(v => v.Weight).First();
                            playerStatMarket.Value.MainMarket.Active = true;
                        }

                        foreach (string type in types)
                        {
                            if (!player.Stats.ContainsKey(type))
                            {
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
                            playerStatMarket.Value.MainMarket.Active = true;
                        }

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
                    _distributorNhl.SendPlayerMarkets(messageStr, GameId, Description);


                }
            }



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
            GameInfo = _radarNhl.GetGameInfo(GameId);
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

        private void LoadPlayerModelData()
        {
            List<Player> teamTest = AwayTeam.PlayerList;

            NhlGameState.AwayGoalies = AwayTeam.PlayerList.FindAll(X => X.Position == "G").Select(X => X.PlayerId).ToList();

            NhlGameState.HomeGoalies = HomeTeam.PlayerList.FindAll(X => X.Position == "G").Select(X => X.PlayerId).ToList();

            GameInfo = _radarNhl.GetGameInfo(GameId);
            LoadModelDataPlayer();
            LoadModelDataInMlf();

            // AwayTeam.PlayerList;

            bool isGameStarted = StartDateTime < DateTime.UtcNow;

            /// List<NhlPlayer> aways = AwayTeam.PlayerList;

            if (isGameStarted)
            {
                LoadCurrentScore();
            }

            InitialModelDataLoadComplete = true;

        }

        private void LoadModelDataPlayer()
        {

            Dictionary<string, double> homeInGgpDictionary = new Dictionary<string, double>();
            Dictionary<string, double> homeInGgpDictionary1 = new Dictionary<string, double>();

            Dictionary<string, double> homeInGslDictionary = new Dictionary<string, double>();
            Dictionary<string, double> homeInGslDictionary1 = new Dictionary<string, double>();

            Dictionary<string, double> homeInGstDictionary = new Dictionary<string, double>();
            Dictionary<string, double> homeInGstDictionary1 = new Dictionary<string, double>();

            Dictionary<string, double> homeInSgpDictionary = new Dictionary<string, double>();

            Dictionary<string, double> homeInSsgDictionary = new Dictionary<string, double>();

            Dictionary<string, double> homeInSslDictionary = new Dictionary<string, double>();
            Dictionary<string, double> homeInSstDictionary = new Dictionary<string, double>();



            Dictionary<string, double> awayInGgpDictionary = new Dictionary<string, double>();
            Dictionary<string, double> awayInGgpDictionary1 = new Dictionary<string, double>();

            Dictionary<string, double> awayInGslDictionary = new Dictionary<string, double>();
            Dictionary<string, double> awayInGslDictionary1 = new Dictionary<string, double>();

            Dictionary<string, double> awayInGstDictionary = new Dictionary<string, double>();
            Dictionary<string, double> awayInSgpDictionary = new Dictionary<string, double>();
            Dictionary<string, double> awayInSsgDictionary = new Dictionary<string, double>();
            Dictionary<string, double> awayInSslDictionary = new Dictionary<string, double>();
            Dictionary<string, double> awayInSstDictionary = new Dictionary<string, double>();


            Guid homeTeamId = HomeTeam.TeamId;
            Guid awayTeamId = AwayTeam.TeamId;
            List<int> window = new List<int>
           {
               1,
               3,
               5,
               10,
               20,
               82,
               1000
           };


            List<string> playerStats = new List<string>
              {
                  "A",
                  "G",
                  "B",
                  "P"
              };

            List<string> playerStatsLabels = new List<string>
              {
                  "assists",
                  "goals",
                  "blocks",
                  "points"
              };

            List<string> goalieStats = new List<string>
              {
                  "SA",
                  "SV"
              };

            List<string> goalieStatsLabels = new List<string>
              {
                  "shots",
                  "saves"
              };



            var side = "H";
            //load home goalie games played
            try
            {
                Logger.Info("homeInGgpDictionary");
                homeInGgpDictionary = _dataAccessNhl.GetInggp(NhlGameState.HomeGoalies[0], 1, side);
            }
            catch (Exception e)
            {
                Logger.Info($"homeInGgpDictionary {e}");
            }

            try
            {
                if (NhlGameState.HomeGoalies.Count == 2)
                {
                    Logger.Info("homeInGgpDictionary 2nd goalie");
                    homeInGgpDictionary1 = _dataAccessNhl.GetInggp(NhlGameState.HomeGoalies[1], 2, side);
                }
            }
            catch (Exception e)
            {
                Logger.Info($"homeInGgpDictionary 2nd goalie {e}");
            }

            homeInGgpDictionary = Utils.MergeDictionaries(homeInGgpDictionary, homeInGgpDictionary1);





            try
            {
                foreach (int days in window)
                {
                    for (int s = 0; s < goalieStats.Count; s++)
                    {
                        string stat = goalieStats[s];
                        string statLable = goalieStatsLabels[s];

                        Logger.Info("homeInGslDictionary");
                        Utils.AddToDictionary(homeInGslDictionary,
                            _dataAccessNhl.GetIngsl(NhlGameState.HomeGoalies[0], days, 1, side, stat, statLable));
                        Logger.Info("homeInGstDictionary");
                        Utils.AddToDictionary(homeInGstDictionary,
                            _dataAccessNhl.GetIngst(NhlGameState.HomeGoalies[0], AwayTeam.TeamId, days, 1, side, stat,
                                statLable));

                        if (NhlGameState.HomeGoalies.Count == 2)
                        {
                            Logger.Info("homeInGslDictionary");
                            Utils.AddToDictionary(homeInGslDictionary,
                                _dataAccessNhl.GetIngsl(NhlGameState.HomeGoalies[1], days, 2, side, stat, statLable));

                            Logger.Info("homeInGstDictionary");
                            Utils.AddToDictionary(homeInGstDictionary,
                                _dataAccessNhl.GetIngst(NhlGameState.HomeGoalies[1], AwayTeam.TeamId, days, 2, side,
                                    stat, statLable));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Info($"error loading homeInGslDictionary / homeInGstDictionary {e}");
            }


            try
            {
                foreach (Player player in HomeTeam.PlayerList)
                {
                    if (player.Position == "G")
                    {
                        continue;
                    }

                    Logger.Info("homeInSgpDictionary");
                    Utils.AddToDictionary(homeInSgpDictionary,
                        _dataAccessNhl.GetInsgp(player.PlayerId, side, player.Number));
                }
            }
            catch (Exception e)
            {
                Logger.Info($"homeInSgpDictionary {e}");
            }


            int limit = 0;
            try
            {
                foreach (Player player in HomeTeam.PlayerList)
                {

                    if (player.Position == "G")
                    {
                        continue;
                    }
                    foreach (int days in window)
                    {

                        for (int s = 0; s < playerStats.Count; s++)
                        {
                            string stat = playerStats[s];
                            string statLable = playerStatsLabels[s];

                            Logger.Info("homeInSsgDictionary");
                            Utils.AddToDictionary(homeInSsgDictionary, _dataAccessNhl.GetInssg(player.PlayerId,
                                NhlGameState.AwayGoalies[0],
                                days, 1, side, player.Number, stat, statLable));

                            if (NhlGameState.AwayGoalies.Count == 2)
                            {
                                Logger.Info("homeInSsgDictionary");
                                Utils.AddToDictionary(homeInSsgDictionary, _dataAccessNhl.GetInssg(player.PlayerId,
                                    NhlGameState.AwayGoalies[1],
                                    days, 2, side, player.Number, stat, statLable));
                            }

                            Logger.Info("homeInSslDictionary");
                            Utils.AddToDictionary(homeInSslDictionary,
                                _dataAccessNhl.GetInssl(player.PlayerId, days, side, player.Number, stat, statLable));

                            Logger.Info("homeInSsTDictionary");
                            Utils.AddToDictionary(homeInSstDictionary,
                                _dataAccessNhl.GetInsst(player.PlayerId, AwayTeam.TeamId, days, side, player.Number,
                                    stat, statLable));
                        }
                    }

                    limit++;
                    if (limit > 3)
                    {
                        // break;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Info($"player stats error {e}");
            }


            //  }

            //  foreach (Player wnbaPlayer in AwayTeam.PlayerList)
            //  {
            side = "V";
            awayInGgpDictionary = _dataAccessNhl.GetInggp(NhlGameState.AwayGoalies[0], 1, side);
            Logger.Info("awayInGgpDictionary");


            if (NhlGameState.AwayGoalies.Count == 2)
            {
                Logger.Info("awayInGgpDictionary1");
                awayInGgpDictionary1 = _dataAccessNhl.GetInggp(NhlGameState.AwayGoalies[1], 2, side);
            }
            awayInGgpDictionary = Utils.MergeDictionaries(awayInGgpDictionary, awayInGgpDictionary1);

            foreach (int days in window)
            {
                for (int s = 0; s < goalieStats.Count; s++)
                {
                    string stat = goalieStats[s];
                    string statLable = goalieStatsLabels[s];

                    Logger.Info("awayInGslDictionary");
                    Utils.AddToDictionary(awayInGslDictionary,
                        _dataAccessNhl.GetIngsl(NhlGameState.AwayGoalies[0], days, 1, side, stat, statLable));
                    Logger.Info("awayInGstDictionary");
                    Utils.AddToDictionary(awayInGstDictionary,
                        _dataAccessNhl.GetIngst(NhlGameState.AwayGoalies[0], HomeTeam.TeamId, days, 1, side, stat, statLable));


                    if (NhlGameState.AwayGoalies.Count == 2)
                    {

                        Logger.Info("awayInGslDictionary");
                        Utils.AddToDictionary(awayInGslDictionary,
                            _dataAccessNhl.GetIngsl(NhlGameState.AwayGoalies[1], days, 2, side, stat, statLable));

                        Logger.Info("awayInGstDictionary");
                        Utils.AddToDictionary(awayInGstDictionary,
                            _dataAccessNhl.GetIngst(NhlGameState.AwayGoalies[1], HomeTeam.TeamId, days, 2, side, stat, statLable));
                    }
                }

            }

            foreach (Player player in AwayTeam.PlayerList)
            {
                Logger.Info("awayInSgpDictionary");
                Utils.AddToDictionary(awayInSgpDictionary, _dataAccessNhl.GetInsgp(player.PlayerId, side, player.Number));
            }

            limit = 0;
            foreach (Player player in AwayTeam.PlayerList)
            {
                if (player.Position == "G")
                {
                    continue;
                }

                foreach (int days in window)
                {

                    for (int s = 0; s < playerStats.Count; s++)
                    {
                        string stat = playerStats[s];
                        string statLable = playerStatsLabels[s];


                        Logger.Info("awayInSsgDictionary");
                        Utils.AddToDictionary(awayInSsgDictionary, _dataAccessNhl.GetInssg(player.PlayerId,
                            NhlGameState.HomeGoalies[0],
                            days, 1, side, player.Number, stat, statLable));


                        if (NhlGameState.HomeGoalies.Count == 2)
                        {
                            Logger.Info("awayInSsgDictionary");
                            Utils.AddToDictionary(awayInSsgDictionary, _dataAccessNhl.GetInssg(player.PlayerId,
                                NhlGameState.HomeGoalies[1],
                                days, 2, side, player.Number, stat, statLable));
                        }

                        Logger.Info("awayInSslDictionary");
                        Utils.AddToDictionary(awayInSslDictionary,
                            _dataAccessNhl.GetInssl(player.PlayerId, days, side, player.Number, stat, statLable));

                        Logger.Info("awayInSsTDictionary");
                        Utils.AddToDictionary(awayInSstDictionary,
                            _dataAccessNhl.GetInsst(player.PlayerId, HomeTeam.TeamId, days, side, player.Number, stat, statLable));
                    }
                }

                limit++;
                if (limit > 3)
                {
                    //   break;
                }
            }



            // }


            ModelData[NhlModelDataKeys.InGgp] = Utils.MergeDictionaries(homeInGgpDictionary, awayInGgpDictionary);  //Goalie Games Played
            ModelData[NhlModelDataKeys.InGsl] = Utils.MergeDictionaries(homeInGslDictionary, awayInGslDictionary);  //Goalie Stats vs League
            ModelData[NhlModelDataKeys.InGst] = Utils.MergeDictionaries(homeInGstDictionary, awayInGstDictionary);  //Goalie Stats vs Team
            ModelData[NhlModelDataKeys.InSgp] = Utils.MergeDictionaries(homeInSgpDictionary, awayInSgpDictionary); //Skater Games Played  
            ModelData[NhlModelDataKeys.InSsg] = Utils.MergeDictionaries(homeInSsgDictionary, awayInSsgDictionary);  //Skater stats vs Goalie  integers
            ModelData[NhlModelDataKeys.InSsl] = Utils.MergeDictionaries(homeInSslDictionary, awayInSslDictionary);  //Skater Stats vs League  decimal  84 records numeric indexes
            ModelData[NhlModelDataKeys.InSst] = Utils.MergeDictionaries(homeInSstDictionary, awayInSstDictionary);  //Skater Stats vs team  not decimal, 112 records numeric index

            //homeInSslDictionary
        }


        private void LoadCurrentScore()
        {
            try
            {
                NhlGameInfo nhlGameInfo = _radarNhl.GetGameSummary(GameId);

                // todo the code below was commented out because it applies to MLB and not NHL

                if (nhlGameInfo.Status != IBaseGameAttributesStatus.Inprogress)
                {
                    return;
                }



                NhlGameState.Period = ToInt32(nhlGameInfo.Period);
                NhlGameState.Seconds = Utils.ConvertPeriodToGameStringNHL(NhlGameState.Period, nhlGameInfo.Clock, 2700);
                NhlGameState.Home = ToInt32(nhlGameInfo.Team[0].Points);
                NhlGameState.Away = ToInt32(nhlGameInfo.Team[1].Points);

                Collection<PeriodScoreType> homeScoring = nhlGameInfo.Team[0].Scoring[0].Period;
                ModelData[NhlModelDataKeys.Evs][$"H,P0"] = NhlGameState.Home;

                //load game score
                for (int period = 0; period < homeScoring.Count; period++)
                {
                    string goalsString = homeScoring[period].Points;
                    int goals = ToInt32(goalsString);
                    string periodString = homeScoring[period].Sequence;
                    ModelData[NhlModelDataKeys.Evs][$"H,P{periodString}"] = goals;
                }


                //load stats
                if (nhlGameInfo.Team != null && nhlGameInfo.Team.Count == 2 & !IsTeamMode)
                {

                }


                Collection<PeriodScoreType> awayScoring = nhlGameInfo.Team[1].Scoring[0].Period;
                ModelData[NhlModelDataKeys.Evs][$"V,P0"] = NhlGameState.Away;
                //away score
                for (int period = 0; period < awayScoring.Count; period++)
                {
                    string goalsString = awayScoring[period].Points;
                    int goals = ToInt32(goalsString);
                    string periodString = homeScoring[period].Number;
                    ModelData[NhlModelDataKeys.Evs][$"V,P{periodString}"] = goals;
                }


                if (!IsTeamMode)
                {
                    for (int i = 0; i < nhlGameInfo.Team.Count; i++)
                    {
                        if (nhlGameInfo.Team[i].Home == true)
                        {
                            for (int plyr = 0; plyr < nhlGameInfo.Team[i].Players.Count; plyr++)
                            {
                                if (nhlGameInfo.Team[0].Players[plyr].Id == null)
                                {
                                    continue;
                                }

                                Guid playerId = Utils.ParseGuid(nhlGameInfo.Team[i].Players[plyr].Id);
                                Player player = HomeTeam.PlayerList.Find(x => x.PlayerId == playerId);
                                if (player != null)
                                {
                                    for (int period = 0;
                                        period < nhlGameInfo.Team[i].Players[plyr].Statistics.Periods.Count;
                                        period++)
                                    {
                                        if (period > 3)
                                        {
                                            break;
                                        }

                                        #region STATS

                                        /*   nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Penalty.Assists;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Penalty.Goals;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Penalty.Kill_Pct;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Penalty.Kill_PctSpecified;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Penalty.Missed_Shots;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Penalty.Opportunities;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Penalty.Percentage;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Penalty.PercentageSpecified;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Penalty.Points;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Penalty.Shots;

                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Evenstrength.Goals;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Evenstrength.Kill_Pct;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Evenstrength.Kill_PctSpecified;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Evenstrength.Missed_Shots;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Evenstrength.Opportunities;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Evenstrength.Percentage;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Evenstrength.PercentageSpecified;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Evenstrength.Points;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Evenstrength.Shots;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Evenstrength.Assists;


                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Assists;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Blocked_Att;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Blocked_Shots;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Emptynet_Goals;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Faceoff_Win_Pct;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Faceoff_Win_PctSpecified;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Faceoffs_Won;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Faceoffs_Lost
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Giveaways;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Goals;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Hits;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Missed_Shots;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Kill_Pct;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Number_Of_Shifts;
                                           nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Shooting_Pct;
                                           */
                                        //nhlGameInfo.Team[0].Players[plyr].Statistics.Periods[period].Points;

                                        #endregion


                                        if (nhlGameInfo.Team[i].Players[plyr].Position == PositionType.G)
                                        {
                                            if (nhlGameInfo.Team[i].Players[plyr].Goaltending != null)
                                            {

                                                player.playerStats["SA"][period + 1] =
                                                    ToDouble(nhlGameInfo.Team[i].Players[plyr].Goaltending
                                                        .Periods[period]
                                                        .Shots_Against);

                                                player.playerStats["SV"][period + 1] =
                                                    ToDouble(
                                                        nhlGameInfo.Team[i].Players[plyr].Goaltending.Periods[period].Saves);
                                            }
                                        }
                                        else
                                        {

                                            player.playerStats["A"][period + 1] =
                                                ToDouble(nhlGameInfo.Team[i].Players[plyr].Statistics.Periods[period]
                                                    .Assists);
                                            player.playerStats["G"][period + 1] =
                                                ToDouble(nhlGameInfo.Team[i].Players[plyr].Statistics.Periods[period]
                                                    .Goals);
                                            player.playerStats["B"][period + 1] =
                                                ToDouble(nhlGameInfo.Team[i].Players[plyr].Statistics.Periods[period]
                                                    .Blocked_Shots);

                                            double points =
                                                ToDouble(nhlGameInfo.Team[i].Players[plyr].Statistics.Periods[period]
                                                    .Assists) +
                                                ToDouble(nhlGameInfo.Team[i].Players[plyr].Statistics.Periods[period]
                                                    .Goals);
                                            player.playerStats["P"][period + 1] = points;
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            for (int plyr = 0; plyr < nhlGameInfo.Team[i].Players.Count; plyr++)
                            {
                                Guid playerId = Utils.ParseGuid(nhlGameInfo.Team[i].Players[plyr].Id);
                                Player player = AwayTeam.PlayerList.Find(x => x.PlayerId == playerId);
                                if (player != null)
                                {
                                    for (int period = 0;
                                        period < nhlGameInfo.Team[i].Players[plyr].Statistics.Periods.Count;
                                        period++)
                                    {

                                        if (nhlGameInfo.Team[i].Players[plyr].Position == PositionType.G)
                                        {
                                            if (nhlGameInfo.Team[i].Players[plyr].Goaltending != null)
                                            {
                                                player.playerStats["SA"][period + 1] =
                                                    ToDouble(nhlGameInfo.Team[i].Players[plyr].Goaltending
                                                        .Periods[period]
                                                        .Shots_Against);

                                                player.playerStats["SV"][period + 1] =
                                                    ToDouble(
                                                        nhlGameInfo.Team[i].Players[plyr].Goaltending.Periods[period].Saves);
                                            }
                                        }
                                        else
                                        {

                                            player.playerStats["A"][period + 1] =
                                                ToDouble(nhlGameInfo.Team[i].Players[plyr].Statistics.Periods[period]
                                                    .Assists);
                                            player.playerStats["G"][period + 1] =
                                                ToDouble(nhlGameInfo.Team[i].Players[plyr].Statistics.Periods[period]
                                                    .Goals);
                                            player.playerStats["B"][period + 1] =
                                                ToDouble(nhlGameInfo.Team[i].Players[plyr].Statistics.Periods[period]
                                                    .Blocked_Shots);

                                            double points =
                                                ToDouble(nhlGameInfo.Team[i].Players[plyr].Statistics.Periods[period]
                                                    .Assists) +
                                                ToDouble(nhlGameInfo.Team[i].Players[plyr].Statistics.Periods[period]
                                                    .Goals);
                                            player.playerStats["P"][period + 1] = points;
                                        }

                                    }

                                }
                            }

                        }

                    }
                }

                //todo add penlity
            }
            catch (Exception e)
            {
                Logger.Info(e);
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
                ModelData[NhlModelDataKeys.InMlf] = marketOddsDictionary;
            }
            else
            {
                Dictionary<string, double> dictionary = ModelData[NhlModelDataKeys.InMlf];
                InitializeOdds(dictionary);
            }
        }

        private static void InitializeOdds(IDictionary<string, double> dictionary)
        {
            List<string> lineTypeList = new List<string> { "TO", "SP", "ML" };
            List<string> periodList = new List<string> { "P0", "P1", "P2", "P3", "P4" };
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
            // todo the code below was commented out because it is for MLB and not NHL

            Dictionary<string, double> gameScoreDictionary = new Dictionary<string, double>();
            Dictionary<string, double> dictionaryEgt = new Dictionary<string, double>();

            //if (GameInfo.Status != IBaseGameAttributesStatus.Inprogress)
            //{
            //    for (int inning = 1; inning <= 9; inning++)
            //    {
            //        string key = $"V,I{inning}";
            //        gameScoreDictionary[key] = 0d;

            //        key = $"H,I{inning}";
            //        gameScoreDictionary[key] = 0d;
            //    }

            //    ModelData[NhlModelDataKeys.Evs] = gameScoreDictionary;

            //    dictionaryEgt["I"] = 1;
            //    dictionaryEgt["T"] = 1;
            //    dictionaryEgt["O"] = 0;
            //    dictionaryEgt["B"] = 0;
            //    dictionaryEgt["S"] = 0;
            //    dictionaryEgt["RS"] = 0;
            //    ModelData[NhlModelDataKeys.Egt] = dictionaryEgt;

            //    return;
            //}

            //List<InningScoreType> homeScoreList = GetScoring(GameInfo.Home);
            //List<InningScoreType> awayScoreList = GetScoring(GameInfo.Away);

            //// todo handle innings beyond 9 ************************************************
            //if (homeScoreList.Count > 9)
            //{
            //    string message = $"homeScoringList is too long, truncating scoring list: Count = {homeScoreList.Count} ***********************************";
            //    Logger.Error(message);
            //    homeScoreList = homeScoreList.Take(9).ToList();
            //}

            //// todo handle innings beyond 9 ************************************************
            //if (awayScoreList.Count > 9)
            //{
            //    string message = $"awayScoringList is too long, truncating scoring list: Count = {awayScoreList.Count} ***********************************";
            //    Logger.Error(message);
            //    awayScoreList = awayScoreList.Take(9).ToList();
            //}

            //string homeOrVisitor = "H";
            //LoadGameScoreDictionary(homeScoreList, gameScoreDictionary, homeOrVisitor);

            //homeOrVisitor = "V";
            //LoadGameScoreDictionary(awayScoreList, gameScoreDictionary, homeOrVisitor);

            //ModelData[NhlModelDataKeys.Evs] = gameScoreDictionary;

            ////EGT
            ////update with game info details
            //dictionaryEgt["I"] = 0;
            //dictionaryEgt["T"] = 0;
            //dictionaryEgt["O"] = 0;
            //dictionaryEgt["B"] = 0;
            //dictionaryEgt["S"] = 0;
            //dictionaryEgt["RS"] = 0;
            //ModelData[NhlModelDataKeys.Egt] = dictionaryEgt;
        }

        //private static void LoadGameScoreDictionary(IEnumerable<InningScoreType> scoringList, IDictionary<string, double> gameScoreDictionary, string homeOrVistor)
        //{
        //    foreach (InningScoreType homeScoring in scoringList)
        //    {
        //        string inningNumber = homeScoring.Number;
        //        int runs = ParseRuns(homeScoring.Runs);

        //        string key = $"{homeOrVistor},I{inningNumber}";
        //        gameScoreDictionary[key] = runs;
        //    }
        //}

        //private static List<InningScoreType> GetScoring(IReadOnlyList<TeamType> teamList)
        //{
        //    TeamType team = teamList[0];
        //    Collection<InningScoreType> inningScoringArray = team.Scoring;
        //    List<InningScoreType> inningScoreList = new List<InningScoreType>(inningScoringArray);
        //    return inningScoreList;
        //}

        private static int ParseRuns(string runsString)
        {
            // because of bad data in the runs field ("x" or "X"), runs was changed from int to string
            // if runs fails to parse, set it to 0
            bool success = int.TryParse(runsString, out int runs);
            return success ? runs : 0;
        }

        private void LoadModelDataInTsf()
        {
            Guid homeTeamId = HomeTeam.TeamId;
            Guid awayTeamId = AwayTeam.TeamId;

            string homeOrVistor = "H";
            //  Dictionary<string, double> homeDictionary = _dataAccessNhl.GetScoreAverage(homeTeamId, awayTeamId, awayPitcherId, homeOrVistor);

            homeOrVistor = "V";
            //  Dictionary<string, double> awayDictionary = _dataAccessNhl.GetScoreAverage(awayTeamId, homeTeamId, homePitcherId, homeOrVistor);

            //   Dictionary<string, double> mergedDictionary = Utils.MergeDictionaries(homeDictionary, awayDictionary);

            //            ModelData[NhlModelDataKeys.InTsf] = mergedDictionary;
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



    // todo this class was copied from MLB and needs to be updated
    public class NhlGameState
    {
        public string Status { get; set; }
        public int Period { get; set; }
        public int Home { get; set; }
        public int Away { get; set; }
        public int Sequence { get; set; }
        public int Seconds { get; set; }
        public string Clock { get; set; }
        public List<Penalty> AwayPenalties { get; set; }
        public List<Penalty> HomePenalties { get; set; }
        public List<Guid> AwayGoalies { get; set; }
        public List<Guid> HomeGoalies { get; set; }

        public Dictionary<int, NhlPeriodScore> PeriodScores { get; }

        public NhlGameState()
        {
            PeriodScores = new Dictionary<int, NhlPeriodScore>();
            Sequence = 0;
            Away = 0;
            Home = 0;
            AwayPenalties = new List<Penalty>();
            HomePenalties = new List<Penalty>();
        }
    }



    public class Penalty
    {
        public int duration { get; set; }
        public int startSeconds { get; set; }
        public int endSeconds { get; set; }
        public Guid player_id { get; set; }
        public Guid event_id { get; set; }
    }

    public class NhlPeriodScore
    {
        public int Away { get; set; }
        public int Home { get; set; }
    }
}
