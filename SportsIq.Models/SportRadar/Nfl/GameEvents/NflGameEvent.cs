using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static System.Int32;

namespace SportsIq.Models.SportRadar.Nfl.GameEvents
{
    // NflGameEvent nflGameEvent = NflGameEvent.FromJson(jsonString);

    public partial class NflGameEvent
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

        [JsonProperty("team")]
        public string Team { get; set; }
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
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("sequence")]
        public long Sequence { get; set; }
        //public int Sequence { get; set; }

     //   [JsonProperty("reference")]
     //   [JsonConverter(typeof(ParseStringConverter))]
     //   public int Reference { get; set; }

        [JsonProperty("clock")]
        public string Clock { get; set; }

        [JsonProperty("home_points")]
        public int HomePoints { get; set; }

        [JsonProperty("away_points")]
        public int AwayPoints { get; set; }

        [JsonProperty("play_type")]
        public string PlayType { get; set; }

        [JsonProperty("play_clock")]
        public int PlayClock { get; set; }

        [JsonProperty("wall_clock")]
        public DateTimeOffset WallClock { get; set; }

        [JsonProperty("fake_punt")]
        public bool FakePunt { get; set; }

        [JsonProperty("fake_field_goal")]
        public bool FakeFieldGoal { get; set; }

        [JsonProperty("screen_pass")]
        public bool ScreenPass { get; set; }

        [JsonProperty("play_action")]
        public bool PlayAction { get; set; }

        [JsonProperty("run_pass_option")]
        public bool RunPassOption { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("alt_description")]
        public string AltDescription { get; set; }

        [JsonProperty("period")]
        public Period Period { get; set; }

        [JsonProperty("drive")]
        public Drive Drive { get; set; }

        [JsonProperty("start_situation")]
        public Situation StartSituation { get; set; }

        [JsonProperty("end_situation")]
        public Situation EndSituation { get; set; }

        [JsonProperty("statistics")]
        public List<Statistic> Statistics { get; set; }
    }

    public class Drive
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("sequence")]
        public int Sequence { get; set; }

        [JsonProperty("start_reason")]
        public string StartReason { get; set; }

        [JsonProperty("end_reason")]
        public string EndReason { get; set; }

        [JsonProperty("play_count")]
        public int PlayCount { get; set; }

        [JsonProperty("duration")]
        public string Duration { get; set; }

        [JsonProperty("first_downs")]
        public int FirstDowns { get; set; }

        [JsonProperty("gain")]
        public int Gain { get; set; }

        [JsonProperty("penalty_yards")]
        public int PenaltyYards { get; set; }
    }

    public class Situation
    {
        [JsonProperty("clock")]
        public string Clock { get; set; }

        [JsonProperty("down")]
        public int Down { get; set; }

        [JsonProperty("yfd")]
        public int Yfd { get; set; }

        [JsonProperty("possession")]
        public Location Possession { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }
    }

    public class Location
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("market")]
        public string Market { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

    //    [JsonProperty("reference")]
    //    [JsonConverter(typeof(ParseStringConverter))]
    //    public int Reference { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("yardline", NullValueHandling = NullValueHandling.Ignore)]
        public int? Yardline { get; set; }

        [JsonProperty("sr_id")]
        public string SrId { get; set; }
    }

    public class Period
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("sequence")]
        public int Sequence { get; set; }
    }

    public class Statistic
    {
        [JsonProperty("stat_type")]
        public string StatType { get; set; }

        [JsonProperty("attempt", NullValueHandling = NullValueHandling.Ignore)]
        public int? Attempt { get; set; }

        [JsonProperty("yards", NullValueHandling = NullValueHandling.Ignore)]
        public int? Yards { get; set; }

        [JsonProperty("gross_yards", NullValueHandling = NullValueHandling.Ignore)]
        public int? GrossYards { get; set; }

        [JsonProperty("touchback")]
        public int Touchback { get; set; }

        [JsonProperty("onside_attempt", NullValueHandling = NullValueHandling.Ignore)]
        public int? OnsideAttempt { get; set; }

        [JsonProperty("onside_success", NullValueHandling = NullValueHandling.Ignore)]
        public int? OnsideSuccess { get; set; }

        [JsonProperty("squib_kick", NullValueHandling = NullValueHandling.Ignore)]
        public int? SquibKick { get; set; }

        [JsonProperty("team")]
        public Location Team { get; set; }

        [JsonProperty("player", NullValueHandling = NullValueHandling.Ignore)]
        public Player Player { get; set; }

        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }
    }

    public class Player
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("jersey")]
        public string Jersey { get; set; }

    //    [JsonProperty("reference")]
    //    public string Reference { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("sr_id")]
        public string SrId { get; set; }
    }

    public class Game
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

   //     [JsonProperty("reference")]
   //     [JsonConverter(typeof(ParseStringConverter))]
   //     public int Reference { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("scheduled")]
        public DateTimeOffset Scheduled { get; set; }

        [JsonProperty("attendance")]
        public int Attendance { get; set; }

        [JsonProperty("utc_offset")]
        public int UtcOffset { get; set; }

        [JsonProperty("entry_mode")]
        public string EntryMode { get; set; }

        [JsonProperty("weather")]
        public string Weather { get; set; }

        [JsonProperty("quarter")]
        public int Quarter { get; set; }

        [JsonProperty("clock")]
        public string Clock { get; set; }

        [JsonProperty("sr_id")]
        public string SrId { get; set; }

        [JsonProperty("summary")]
        public Summary Summary { get; set; }
    }

    public class Summary
    {
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

        [JsonProperty("alias")]
        public string Alias { get; set; }

    //    [JsonProperty("reference")]
    //    [JsonConverter(typeof(ParseStringConverter))]
    //    public int Reference { get; set; }

        [JsonProperty("used_timeouts")]
        public int UsedTimeouts { get; set; }

        [JsonProperty("remaining_timeouts")]
        public int RemainingTimeouts { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }

        [JsonProperty("sr_id")]
        public string SrId { get; set; }
    }

    #region JSON Conversion Helpers

    public partial class NflGameEvent
    {
        public static NflGameEvent FromJson(string json) => JsonConvert.DeserializeObject<NflGameEvent>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this NflGameEvent self) => JsonConvert.SerializeObject(self, Converter.Settings);
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
        public override bool CanConvert(Type t) => t == typeof(int) || t == typeof(int?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            string value = serializer.Deserialize<string>(reader);

            if (TryParse(value, out int l))
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
