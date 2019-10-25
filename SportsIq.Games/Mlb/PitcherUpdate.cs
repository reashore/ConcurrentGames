using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SportsIq.Games.Mlb
{
    // PitcherUpdate pitcherUpdate = PitcherUpdate.FromJson(jsonString);

    public partial class PitcherUpdate
    {
        [JsonProperty("games")]
        public List<Guid> GameList { get; set; }
    }

    #region JSON Conversion Helpers

    public partial class PitcherUpdate
    {
        public static PitcherUpdate FromJson(string json) => JsonConvert.DeserializeObject<PitcherUpdate>(json, Converter.Settings);
    }

    //public static class Serialize
    //{
    //    public static string ToJson(this PitcherUpdate self) => JsonConvert.SerializeObject(self, Converter.Settings);
    //}

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

    #endregion
}