using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// todo WARNING this file is just a copy from WNBA and must be regenerated

namespace SportsIq.Models.SportRadar.Nba.GameEvents
{
    // NbaGameEvent wnbaGameEvent = NbaGameEvent.FromJson(jsonString);

    public partial class NbaGameEvent
    {
        [JsonProperty("payload")]
        public Payload Payload { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("league")]
        public string League { get; set; }

        [JsonProperty("match")]
        public string Match { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("players")]
        public string Players { get; set; }

        [JsonProperty("event_type")]
        public string EventType { get; set; }

        [JsonProperty("event_category")]
        public string EventCategory { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("operation")]
        public string Operation { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public class Payload
    {
        [JsonProperty("game")]
        public Game Game { get; set; }

        [JsonProperty("event")]
        public Event Event { get; set; }
    }

    public class Event
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("event_type")]
        public string EventType { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("sequence")]
        public long Sequence { get; set; }

        [JsonProperty("clock")]
        public string Clock { get; set; }

        [JsonProperty("updated")]
        public DateTimeOffset Updated { get; set; }

        [JsonProperty("wall_clock")]
        public DateTimeOffset WallClock { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("home_points")]
        public object HomePoints { get; set; }

        [JsonProperty("away_points")]
        public object AwayPoints { get; set; }

        [JsonProperty("attribution")]
        public Attribution Attribution { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("possession")]
        public Possession Possession { get; set; }

        [JsonProperty("period")]
        public Period Period { get; set; }

        [JsonProperty("on_court")]
        public OnCourt OnCourt { get; set; }

        [JsonProperty("statistics")]
        public List<Statistic> Statistics { get; set; }
    }

    public class Possession
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("market")]
        public string Market { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("sr_id")]
        public string SrId { get; set; }
    }

    public class Attribution
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("market")]
        public string Market { get; set; }

     //   [JsonProperty("reference")]
     //   [JsonConverter(typeof(ParseStringConverter))]
     //   public long Reference { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("team_basket", NullValueHandling = NullValueHandling.Ignore)]
        public string TeamBasket { get; set; }

        [JsonProperty("sr_id")]
        public string SrId { get; set; }

        [JsonProperty("players", NullValueHandling = NullValueHandling.Ignore)]
        public List<Player> Players { get; set; }
    }

    public class Player
    {
        [JsonProperty("full_name")]
        public string FullName { get; set; }

        //[JsonProperty("jersey_number")]
        //[JsonConverter(typeof(ParseStringConverter))]
       public int JerseyNumber =0;

       // [JsonProperty("reference")]
      // [JsonConverter(typeof(ParseStringConverter))]
      //  public long Reference { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("sr_id")]
        public string SrId { get; set; }
    }

    public class Location
    {
        [JsonProperty("coord_x")]
        public int CoordX { get; set; }

        [JsonProperty("coord_y")]
        public int CoordY { get; set; }

        [JsonProperty("action_area")]
        public string ActionArea { get; set; }
    }

    public class OnCourt
    {
        [JsonProperty("home")]
        public Attribution Home { get; set; }

        [JsonProperty("away")]
        public Attribution Away { get; set; }
    }

    public class Period
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("sequence")]
        public int Sequence { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public List<Event> Events { get; set; }
    }

    public class Statistic
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("made", NullValueHandling = NullValueHandling.Ignore)]
        public bool Made { get; set; }

        [JsonProperty("three_point_shot", NullValueHandling = NullValueHandling.Ignore)]
        public bool ThreePointShot { get; set; }

        [JsonProperty("shot_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ShotType { get; set; }

        [JsonProperty("shot_distance", NullValueHandling = NullValueHandling.Ignore)]
        public double ShotDistance { get; set; }

        [JsonProperty("points", NullValueHandling = NullValueHandling.Ignore)]
        public int Points { get; set; }

        [JsonProperty("team")]
        public Attribution Team { get; set; }

        [JsonProperty("player")]
        public Player Player { get; set; }
    }

    public class Game
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("coverage")]
        public string Coverage { get; set; }

      //  [JsonProperty("reference")]
      //  [JsonConverter(typeof(ParseStringConverter))]
      //  public long Reference { get; set; }

        [JsonProperty("scheduled")]
        public DateTimeOffset Scheduled { get; set; }

        [JsonProperty("sr_id")]
        public string SrId { get; set; }

        [JsonProperty("home")]
        public Away Home { get; set; }

        [JsonProperty("away")]
        public Away Away { get; set; }
    }

    public class Away
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("market")]
        public string Market { get; set; }

      //  [JsonProperty("reference")]
     //   [JsonConverter(typeof(ParseStringConverter))]
     //   public long Reference { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }

        [JsonProperty("bonus")]
        public bool Bonus { get; set; }

        [JsonProperty("remaining_timeouts")]
        public int RemainingTimeouts { get; set; }

        [JsonProperty("sr_id")]
        public string SrId { get; set; }
    }

    #region JSON Conversion Helpers

    public partial class NbaGameEvent
    {
        public static NbaGameEvent FromJson(string json) => JsonConvert.DeserializeObject<NbaGameEvent>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this NbaGameEvent self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            }
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            string value = serializer.Deserialize<string>(reader);

            if (long.TryParse(value, out long l))
            {
                return l;
            }

            throw new Exception("Cannot unmarshal type int");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            int value = (int)untypedValue;
            serializer.Serialize(writer, value.ToString());
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    #endregion
}
