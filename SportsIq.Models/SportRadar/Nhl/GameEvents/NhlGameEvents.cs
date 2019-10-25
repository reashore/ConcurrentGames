using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SportsIq.Models.SportRadar.Nhl.GameEvents
{
    //  NhlGameEvent nhlGameEvent = NhlGameEvent.FromJson(jsonString);

    public partial class NhlGameEvent
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

        [JsonProperty("on_ice")]
        public string OnIce { get; set; }

        [JsonProperty("zone")]
        public string Zone { get; set; }

        [JsonProperty("strength")]
        public string Strength { get; set; }

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

        [JsonProperty("clock")]
        public string Clock { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("updated")]
        public DateTimeOffset Updated { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("attribution")]
        public Attribution Attribution { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("period")]
        public Period Period { get; set; }

        [JsonProperty("statistics")]
        public List<Statistic> Statistics { get; set; }

        [JsonProperty("on_ice")]
        public List<OnIce> OnIce { get; set; }

        [JsonProperty("in_penalty")]
        public List<OnIce> InPenalty { get; set; }
    }

    public class Attribution
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("market")]
        public string Market { get; set; }

        [JsonProperty("reference")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Reference { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("team_goal", NullValueHandling = NullValueHandling.Ignore)]
        public string TeamGoal { get; set; }

        [JsonProperty("sr_id")]
        public string SrId { get; set; }

        [JsonProperty("players", NullValueHandling = NullValueHandling.Ignore)]
        public List<PlayerElement> Players { get; set; }

        [JsonProperty("points", NullValueHandling = NullValueHandling.Ignore)]
        public int Points { get; set; }
    }

    public class PlayerElement
    {
        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("jersey_number")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long JerseyNumber { get; set; }

        [JsonProperty("reference")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Reference { get; set; }

        [JsonProperty("position")]
        public Position Position { get; set; }

        [JsonProperty("primary_position")]
        public string PrimaryPosition { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("sr_id")]
        public string SrId { get; set; }
    }

    public class Location
    {
        [JsonProperty("coord_x")]
        public long CoordX { get; set; }

        [JsonProperty("coord_y")]
        public long CoordY { get; set; }

        [JsonProperty("action_area")]
        public string ActionArea { get; set; }
    }

    public class OnIce
    {
        [JsonProperty("team")]
        public Attribution Team { get; set; }
    }

    public class Period
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("number")]
        public long Number { get; set; }

        [JsonProperty("sequence")]
        public int Sequence { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Statistic
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("goal")]
        public bool Goal { get; set; }

        [JsonProperty("strength")]
        public string Strength { get; set; }

        [JsonProperty("zone")]
        public string Zone { get; set; }

        [JsonProperty("saved", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Saved { get; set; }

        [JsonProperty("team")]
        public Attribution Team { get; set; }

        [JsonProperty("player")]
        public StatisticPlayer Player { get; set; }
    }

    public class StatisticPlayer
    {
        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("jersey_number")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long JerseyNumber { get; set; }

        [JsonProperty("reference")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Reference { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("sr_id")]
        public string SrId { get; set; }
    }

    public class Game
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("coverage")]
        public string Coverage { get; set; }

        [JsonProperty("reference")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Reference { get; set; }

        [JsonProperty("scheduled")]
        public DateTimeOffset Scheduled { get; set; }

        [JsonProperty("period")]
        public int Period { get; set; }

        [JsonProperty("home")]
        public Attribution Home { get; set; }

        [JsonProperty("away")]
        public Attribution Away { get; set; }
    }

    public enum Position { D, F, G }

    #region JSON Conversion Helpers

    public partial class NhlGameEvent
    {
        public static NhlGameEvent FromJson(string json) => JsonConvert.DeserializeObject<NhlGameEvent>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this NhlGameEvent self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                PositionConverter.Singleton,
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
            string valueString = serializer.Deserialize<string>(reader);

            if (long.TryParse(valueString, out var value))
            {
                return value;
            }

            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            long value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    internal class PositionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Position) || t == typeof(Position?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            string value = serializer.Deserialize<string>(reader);

            switch (value)
            {
                case "D":
                    return Position.D;

                case "F":
                    return Position.F;

                case "G":
                    return Position.G;
            }

            throw new Exception("Cannot unmarshal type Position");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            Position value = (Position)untypedValue;

            switch (value)
            {
                case Position.D:
                    serializer.Serialize(writer, "D");
                    return;

                case Position.F:
                    serializer.Serialize(writer, "F");
                    return;

                case Position.G:
                    serializer.Serialize(writer, "G");
                    return;
            }

            throw new Exception("Cannot marshal type Position");
        }

        public static readonly PositionConverter Singleton = new PositionConverter();
    }

    #endregion
}
