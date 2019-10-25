using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using SportsIq.Models.Markets;

namespace SportsIq.Games
{
    public interface IGame
    {
        Guid GameId { get; set; }
        bool IsTeamMode { get; set; }
        CountdownEvent LoadCompleteCountdownEvent {get; set; }

        void LoadModelData();
        void RunModel();

        void AddRadarGameEventHandler();
        void RemoveRadarGameEventHandler();

        void AddPubSubEventHandler();
        void RemovePubSubEventHandler();

        TimeSpan GetTimeSinceLastGameEventOrHeartbeat();

        string Description { get; set; }
        bool InitialModelDataLoadComplete { get; set; }
        bool ModelUpdateRequired { get; set; }
        TimeSpan ModelUpdateInterval { get; }
        bool GameOver { get; set; }
        bool IsSimulation { get; set; }
    }

    public abstract class GameBase<TTeam, TGameInfo>
    {
        public Guid GameId { get; set; }
        public DateTime StartDateTime { get; set; }
        public bool Started { get; set; }
        public TTeam HomeTeam { get; set; }
        public TTeam AwayTeam { get; set; }
        public bool IsTeamMode { get; set; }            // switches between Team and Player mode
        public CountdownEvent LoadCompleteCountdownEvent {get; set; }
        protected Dictionary<string, Dictionary<string, double>> ModelData { get; }
        protected List<string> PeriodList { get; set; }
        protected Dictionary<string, PeriodScore> PeriodScoreDictionary { get; }
        protected Dictionary<int, List<Market>> PeriodMarkets { get; }
        protected TGameInfo GameInfo { get; set; }
        protected Dictionary<string, double> MarketOddsDictionary { get; set; }
        public bool InitialModelDataLoadComplete { get; set; }
        public bool ModelUpdateRequired { get; set; }
        public TimeSpan ModelUpdateInterval { get; }
        public string Description { get; set; }
        public bool GameOver { get; set; }
        public bool IsSimulation { get; set; }
        protected int GameTimeSeconds { get; set; }
        
        protected GameBase()
        {
            ModelData = new Dictionary<string, Dictionary<string, double>>();
            PeriodMarkets = new Dictionary<int, List<Market>>();
            PeriodScoreDictionary = new Dictionary<string, PeriodScore>();
            InitialModelDataLoadComplete = false;
            ModelUpdateRequired = true;
            ModelUpdateInterval = TimeSpan.FromSeconds(2);
        }
    }

    public class Team
    {
        public Guid TeamId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public List<Player> PlayerList { get; set; }
    }

    public class Player
    {
        [JsonProperty("player_id")]
        public Guid PlayerId { get; set; }
        [JsonProperty("player")]
        public string FullName { get; set; }
        [JsonProperty("num")]
        public int Number { get; set; }
        public string comp_id { get; set; }

        [JsonIgnore]
        public string player { get; set; }
        public string Position { get; set; }
        public string team { get; set; }
        [JsonProperty("side")]
        public string Side { get; set; }
        public Dictionary<string, Dictionary<int,double>> playerStats { get; set; }
        public Dictionary<string, OddsAndStats> Stats { get; set; }

        public Player()
        {
            Stats = new Dictionary<string, OddsAndStats>();
            playerStats = new Dictionary<string, Dictionary<int, double>>();
            playerStats.Add("G",new Dictionary<int, double>());
            playerStats["G"].Add(1,0);
            playerStats["G"].Add(2, 0);
            playerStats["G"].Add(3, 0);
            playerStats["G"].Add(4, 0);

            playerStats.Add("P", new Dictionary<int, double>());
            playerStats["P"].Add(1, 0);
            playerStats["P"].Add(2, 0);
            playerStats["P"].Add(3, 0);
            playerStats["P"].Add(4, 0);

            playerStats.Add("B", new Dictionary<int, double>());
            playerStats["B"].Add(1, 0);
            playerStats["B"].Add(2, 0);
            playerStats["B"].Add(3, 0);
            playerStats["B"].Add(4, 0);


            playerStats.Add("A", new Dictionary<int, double>());
            playerStats["A"].Add(1, 0);
            playerStats["A"].Add(2, 0);
            playerStats["A"].Add(3, 0);
            playerStats["A"].Add(4, 0);


            playerStats.Add("SA", new Dictionary<int, double>());
            playerStats["SA"].Add(1, 0);
            playerStats["SA"].Add(2, 0);
            playerStats["SA"].Add(3, 0);
            playerStats["SA"].Add(4, 0);


            playerStats.Add("SV", new Dictionary<int, double>());
            playerStats["SV"].Add(1, 0);
            playerStats["SV"].Add(2, 0);
            playerStats["SV"].Add(3, 0);
            playerStats["SV"].Add(4, 0);


            playerStats.Add("GA", new Dictionary<int, double>());
            playerStats["GA"].Add(1, 0);
            playerStats["GA"].Add(2, 0);
            playerStats["GA"].Add(3, 0);
            playerStats["GA"].Add(4, 0);

        }

    }


    public class periodStat
    {
        public double stat { get; set; }
        public int period { get; set; }
    }


    // todo this class does not belong in here
    public class OddsAndStats
    {
        public double stat { get; set; }
        [JsonIgnore]
        public List<Market> odds { get; set; }
        public Market MainMarket { get; set; }
        public string period { get; set; }

        public OddsAndStats()
        {
            stat = 0;
            odds = new List<Market>();
            period = "CG";
        }
    }

    public class PeriodScore
    {
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
    }
}
