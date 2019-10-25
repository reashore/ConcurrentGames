using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SportsIq.Models.SportRadar.Mlb.GameEvents
{
    // MlbGameEvent mlbGameEvent = MlbGameEvent.FromJson(jsonString);

    public partial class MlbGameEvent
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

        [JsonProperty("inning")]
        public int Inning { get; set; }

        [JsonProperty("inning_half")]
        public string InningHalf { get; set; }

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

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("inning")]
        public int Inning { get; set; }

        [JsonProperty("inning_half")]
        public string InningHalf { get; set; }

        [JsonProperty("sequence_number")]
        public int SequenceNumber { get; set; }

        [JsonProperty("hitter_id")]
        public Guid HitterId { get; set; }

        [JsonProperty("atbat_id")]
        public Guid AtbatId { get; set; }

        [JsonProperty("outcome_id")]
        public string OutcomeId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("flags")]
        public Flags Flags { get; set; }

        [JsonProperty("count")]
        public Count Count { get; set; }

        [JsonProperty("pitcher")]
        public Pitcher Pitcher { get; set; }

        [JsonProperty("hitter")]
        public Hitter Hitter { get; set; }

        [JsonProperty("runners")]
        public List<Runner> Runners { get; set; }
    }

    public class Count
    {
        [JsonProperty("balls")]
        public int Balls { get; set; }

        [JsonProperty("strikes")]
        public int Strikes { get; set; }

        [JsonProperty("outs")]
        public int Outs { get; set; }

        [JsonProperty("pitch_count")]
        public int PitchCount { get; set; }
    }

    public class Flags
    {
        [JsonProperty("is_ab_over")]
        public bool IsAbOver { get; set; }

        [JsonProperty("is_bunt")]
        public bool IsBunt { get; set; }

        [JsonProperty("is_bunt_shown")]
        public bool IsBuntShown { get; set; }

        [JsonProperty("is_hit")]
        public bool IsHit { get; set; }

        [JsonProperty("is_wild_pitch")]
        public bool IsWildPitch { get; set; }

        [JsonProperty("is_passed_ball")]
        public bool IsPassedBall { get; set; }

        [JsonProperty("is_double_play")]
        public bool IsDoublePlay { get; set; }

        [JsonProperty("is_triple_play")]
        public bool IsTriplePlay { get; set; }
    }

    public class Hitter
    {
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("preferred_name")]
        public string PreferredName { get; set; }

        [JsonProperty("jersey_number")]
        [JsonConverter(typeof(ParseStringConverter))]
        public int JerseyNumber { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }
    }

    public class Pitcher
    {
        [JsonProperty("pitch_type")]
        public string PitchType { get; set; }

        // changed from int to string
        [JsonProperty("pitch_speed")]
        public string PitchSpeed { get; set; }

        [JsonProperty("pitch_zone")]
        public int PitchZone { get; set; }

        [JsonProperty("pitch_x")]
        public int PitchX { get; set; }

        [JsonProperty("pitch_y")]
        public int PitchY { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("pitcher_hand")]
        public string PitcherHand { get; set; }

        [JsonProperty("hitter_hand")]
        public string HitterHand { get; set; }

        [JsonProperty("pitch_count")]
        public int PitchCount { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("preferred_name")]
        public string PreferredName { get; set; }

        [JsonProperty("jersey_number")]
        [JsonConverter(typeof(ParseStringConverter))]
        public int JerseyNumber { get; set; }
    }

    public class Runner
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("starting_base")]
        public int StartingBase { get; set; }

        [JsonProperty("ending_base")]
        public int EndingBase { get; set; }

        [JsonProperty("outcome_id")]
        public string OutcomeId { get; set; }

        [JsonProperty("out")]
        public bool Out { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("preferred_name")]
        public string PreferredName { get; set; }

        [JsonProperty("jersey_number")]
        [JsonConverter(typeof(ParseStringConverter))]
        public int JerseyNumber { get; set; }
    }

    public class Game
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("coverage")]
        public string Coverage { get; set; }

        [JsonProperty("game_number")]
        public int GameNumber { get; set; }

        [JsonProperty("double_header")]
        public bool DoubleHeader { get; set; }

        [JsonProperty("mlb_id")]
        [JsonConverter(typeof(ParseStringConverter))]
        public int MlbId { get; set; }

        [JsonProperty("day_night")]
        public string DayNight { get; set; }

        [JsonProperty("scheduled")]
        public DateTimeOffset Scheduled { get; set; }

        [JsonProperty("home_team")]
        public Guid HomeTeam { get; set; }

        [JsonProperty("away_team")]
        public Guid AwayTeam { get; set; }

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

        [JsonProperty("abbr")]
        public string Abbr { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("runs")]
        public int Runs { get; set; }

        [JsonProperty("hits")]
        public int Hits { get; set; }

        [JsonProperty("errors")]
        public int Errors { get; set; }
    }

    #region JSON Conversion Helpers

    public partial class MlbGameEvent
    {
        public static MlbGameEvent FromJson(string json) => JsonConvert.DeserializeObject<MlbGameEvent>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this MlbGameEvent self) => JsonConvert.SerializeObject(self, Converter.Settings);
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
            if (reader.TokenType == JsonToken.Null) return null;
            string value = serializer.Deserialize<string>(reader);

            if (int.TryParse(value, out int l))
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

