using Newtonsoft.Json;
using System.Collections.Generic;

namespace SportsIq.Models.Markets
{
    public class Markets
    {
        [JsonProperty("game", Order = 1)]
        public string Game { get; set; }

        [JsonProperty("id", Order = 2)]
        public int Id { get; set; }

        [JsonProperty("period", Order = 3)]
        public string Period { get; set; }

        [JsonProperty("name", Order = 4)]
        public string Name { get; set; }

        [JsonProperty("markets", Order = 5)]
        public List<Market> MarketList = new List<Market>();

        [JsonIgnore]
        public string Key { get; set; }
    }

    public class Market
    {
        [JsonIgnore]
        [JsonProperty("tp")]
        public string Tp { get; set; }

        [JsonIgnore]
        [JsonProperty("player")]
        public string Player { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("rnrs")]
        public List<MarketRunner> MarketRunnerList = new List<MarketRunner>();

        
        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonIgnore]
        [JsonProperty("target")]
        public double Target { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }

    public class MarketRunner
    {
        [JsonProperty("t")]
        public double Total { get; set; }

        [JsonIgnore]
        [JsonProperty("h")]
        public double Handicap { get; set; }

        [JsonIgnore]
        [JsonProperty("probability")]
        public double Probability { get; set; }

        [JsonProperty("p")]
        public double Price { get; set; }

        [JsonProperty("s")]
        public string Side { get; set; }

        [JsonIgnore]
        [JsonProperty("ratio")]
        public double Ratio { get; set; }

        [JsonIgnore]
        [JsonProperty("_d")]
        public string D = string.Empty;
    }

    public class FormattedGame
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("match")]
        public string Match { get; set; }

        [JsonProperty("odds")]
        public List<Markets> MarketList { get; set; }

        public FormattedGame()
        {
            MarketList = new List<Markets>();
        }
    }

    public class Odds
    {
        [JsonProperty("offerId")]
        public int OfferId { get; set; }

        [JsonProperty("compId")]
        public string CompId { get; set; }

        [JsonProperty("match")]
        public string Match { get; set; }

        [JsonProperty("marketKey")]
        public string MarketKey { get; set; }

        [JsonProperty("bookmaker")]
        public string Bookmaker { get; set; }

        [JsonProperty("bookmaker_id")]
        public int BookmakerId { get; set; }

        [JsonProperty("target")]
        public double Target { get; set; }

        [JsonProperty("oddsRunnerList")]
        public List<OddsRunner> OddsRunnerList { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("radar_home")]
        public string RadarHome { get; set; }

        [JsonProperty("period")]
        public string Period { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("live")]
        public bool Live { get; set; }
    }

    public class OddsRunner
    {
        [JsonProperty("target")]
        public double Target { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("radar_team")]
        public string RadarTeam { get; set; }

        [JsonProperty("side")]
        public string Side { get; set; }
    }
}
