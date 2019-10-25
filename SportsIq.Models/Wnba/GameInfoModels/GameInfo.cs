using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static System.Int32;

// GameInfo gameInfo = GameInfo.FromJson(jsonString);

namespace SportsIq.Models.Wnba.GameInfoModels
{


    public partial class GameInfo
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("coverage")]
        public string Coverage { get; set; }

        [JsonProperty("neutral_site")]
        public bool NeutralSite { get; set; }

        [JsonProperty("scheduled")]
        public DateTimeOffset Scheduled { get; set; }

        [JsonProperty("duration")]
        public string Duration { get; set; }

        [JsonProperty("attendance")]
        public long Attendance { get; set; }

        [JsonProperty("lead_changes")]
        public long LeadChanges { get; set; }

        [JsonProperty("times_tied")]
        public long TimesTied { get; set; }

        [JsonProperty("clock")]
        public string Clock { get; set; }

        [JsonProperty("quarter")]
        public long Quarter { get; set; }

        [JsonProperty("track_on_court")]
        public bool TrackOnCourt { get; set; }

        [JsonProperty("reference")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Reference { get; set; }

        [JsonProperty("entry_mode")]
        public string EntryMode { get; set; }

        [JsonProperty("home")]
        public Away Home { get; set; }

        [JsonProperty("away")]
        public Away Away { get; set; }
    }

    public partial class Away
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("market")]
        public string Market { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("points")]
        public long Points { get; set; }

        [JsonProperty("bonus")]
        public bool Bonus { get; set; }

        [JsonProperty("reference")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Reference { get; set; }

        [JsonProperty("scoring")]
        public List<Scoring> Scoring { get; set; }

        [JsonProperty("leaders")]
        public Leaders Leaders { get; set; }
    }

    public partial class Leaders
    {
        [JsonProperty("points")]
        public List<Assist> Points { get; set; }

        [JsonProperty("rebounds")]
        public List<Assist> Rebounds { get; set; }

        [JsonProperty("assists")]
        public List<Assist> Assists { get; set; }
    }

    public partial class Assist
    {
        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("jersey_number")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long JerseyNumber { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("primary_position")]
        public string PrimaryPosition { get; set; }

        [JsonProperty("reference")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Reference { get; set; }

        [JsonProperty("statistics")]
        public Statistics Statistics { get; set; }
    }

    public partial class Statistics
    {
        [JsonProperty("minutes")]
        public string Minutes { get; set; }

        [JsonProperty("field_goals_made")]
        public long FieldGoalsMade { get; set; }

        [JsonProperty("field_goals_att")]
        public long FieldGoalsAtt { get; set; }

        [JsonProperty("field_goals_pct")]
        public double FieldGoalsPct { get; set; }

        [JsonProperty("three_points_made")]
        public long ThreePointsMade { get; set; }

        [JsonProperty("three_points_att")]
        public long ThreePointsAtt { get; set; }

        [JsonProperty("three_points_pct")]
        public long ThreePointsPct { get; set; }

        [JsonProperty("two_points_made")]
        public long TwoPointsMade { get; set; }

        [JsonProperty("two_points_att")]
        public long TwoPointsAtt { get; set; }

        [JsonProperty("two_points_pct")]
        public double TwoPointsPct { get; set; }

        [JsonProperty("blocked_att")]
        public long BlockedAtt { get; set; }

        [JsonProperty("free_throws_made")]
        public long FreeThrowsMade { get; set; }

        [JsonProperty("free_throws_att")]
        public long FreeThrowsAtt { get; set; }

        [JsonProperty("free_throws_pct")]
        public double FreeThrowsPct { get; set; }

        [JsonProperty("offensive_rebounds")]
        public long OffensiveRebounds { get; set; }

        [JsonProperty("defensive_rebounds")]
        public long DefensiveRebounds { get; set; }

        [JsonProperty("rebounds")]
        public long Rebounds { get; set; }

        [JsonProperty("assists")]
        public long Assists { get; set; }

        [JsonProperty("turnovers")]
        public long Turnovers { get; set; }

        [JsonProperty("steals")]
        public long Steals { get; set; }

        [JsonProperty("blocks")]
        public long Blocks { get; set; }

        [JsonProperty("assists_turnover_ratio")]
        public double AssistsTurnoverRatio { get; set; }

        [JsonProperty("personal_fouls")]
        public long PersonalFouls { get; set; }

        [JsonProperty("tech_fouls")]
        public long TechFouls { get; set; }

        [JsonProperty("flagrant_fouls")]
        public long FlagrantFouls { get; set; }

        [JsonProperty("pls_min")]
        public long PlsMin { get; set; }

        [JsonProperty("points")]
        public long Points { get; set; }

        [JsonProperty("double_double")]
        public bool DoubleDouble { get; set; }

        [JsonProperty("triple_double")]
        public bool TripleDouble { get; set; }

        [JsonProperty("effective_fg_pct")]
        public double EffectiveFgPct { get; set; }

        [JsonProperty("efficiency")]
        public long Efficiency { get; set; }

        [JsonProperty("efficiency_game_score")]
        public double EfficiencyGameScore { get; set; }

        [JsonProperty("points_in_paint")]
        public long PointsInPaint { get; set; }

        [JsonProperty("points_in_paint_att")]
        public long PointsInPaintAtt { get; set; }

        [JsonProperty("points_in_paint_made")]
        public long PointsInPaintMade { get; set; }

        [JsonProperty("points_in_paint_pct")]
        public double PointsInPaintPct { get; set; }

        [JsonProperty("true_shooting_att")]
        public double TrueShootingAtt { get; set; }

        [JsonProperty("true_shooting_pct")]
        public double TrueShootingPct { get; set; }

        [JsonProperty("fouls_drawn")]
        public long FoulsDrawn { get; set; }

        [JsonProperty("offensive_fouls")]
        public long OffensiveFouls { get; set; }

        [JsonProperty("points_off_turnovers")]
        public long PointsOffTurnovers { get; set; }

        [JsonProperty("second_chance_pts")]
        public long SecondChancePts { get; set; }
    }

    public partial class Scoring
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("number")]
        public long Number { get; set; }

        [JsonProperty("sequence")]
        public long Sequence { get; set; }

        [JsonProperty("points")]
        public long Points { get; set; }
    }

    public partial class GameInfo
    {
        public static GameInfo FromJson(string json) => JsonConvert.DeserializeObject<GameInfo>(json, SportsIq.Models.Wnba.GameInfoModels.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this GameInfo self) => JsonConvert.SerializeObject(self, SportsIq.Models.Wnba.GameInfoModels.Converter.Settings);
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
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
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
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}