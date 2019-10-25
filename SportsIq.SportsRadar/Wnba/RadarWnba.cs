using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SportsIq.Models.SportRadar.Wnba.GameEvents;
using SportsIq.Models.SportRadar.Wnba.GameInfo;
using SportsIq.Pusher;

namespace SportsIq.SportsRadar.Wnba
{
    public class WnbaGameEventEventArgs : EventArgs
    {
        public WnbaGameEvent GameEvent { get; set; }
    }

    public interface IRadarWnba : IRadarBase
    {
        Uri GetGameInfoUri(Guid gameId);
        WnbaGameInfo GetGameInfo(Guid gameId);

        Uri GetGameEventUri();
        event EventHandler<WnbaGameEventEventArgs> RadarGameEvent;
        DateTime TimeOfLastRadarGameEventOrHeartbeat { get; set; }

        Uri GetGameSummaryUri(Guid gameId);
        WnbaGameInfo GetGameSummary(Guid gameId);
    }

    public class RadarWnba : RadarBase, IRadarWnba
    {
        private readonly string _wnbaGameInfoBaseUrl;
        private readonly string _wnbaGameEventBaseUrl;
        private readonly string _wnbaAuthenticationKey;

        public RadarWnba(IPusherUtil pusherUtil)
        {
            PusherUtil = pusherUtil;
            _wnbaGameInfoBaseUrl = ConfigurationManager.AppSettings["sportRadarWnbaGameInfoBaseUrl"];
            _wnbaGameEventBaseUrl = ConfigurationManager.AppSettings["sportRadarWnbaGameEventBaseUrl"];
            _wnbaAuthenticationKey = ConfigurationManager.AppSettings["sportRadarWnbaAuthenticationKey"];
            TimeOfLastRadarGameEventOrHeartbeat = DateTime.Now;
        }

        #region Game Info

        public Uri GetGameInfoUri(Guid gameId)
        {
            // this is the SportRadar boxscore format, see https://developer.sportradar.com/docs/read/basketball/WNBA_v4#game-boxscore
            const string format = "xml";
            string wnbaGameInfoUrl = $"{_wnbaGameInfoBaseUrl}{gameId}/boxscore.{format}?api_key={_wnbaAuthenticationKey}";
            Uri wnbaGameInfoUri = new Uri(wnbaGameInfoUrl);
            return wnbaGameInfoUri;
        }

        public WnbaGameInfo GetGameInfo(Guid gameId)
        {
            Uri wnbaGameInfoUri = GetGameInfoUri(gameId);

            if (GameInfoXmlSerializer == null)
            {
                GameInfoXmlSerializer = new XmlSerializer(typeof(WnbaGameInfo));
            }

            WnbaGameInfo wnbaGameInfo = GetGameInfo<WnbaGameInfo>(wnbaGameInfoUri);
            return wnbaGameInfo;
        }

        #endregion

        #region Game Event

        public event EventHandler<WnbaGameEventEventArgs> RadarGameEvent;

        private void OnRadarGameEvent(WnbaGameEventEventArgs wnbaGameEventEventArgs)
        {
            RadarGameEvent?.Invoke(this, wnbaGameEventEventArgs);
        }

        public Uri GetGameEventUri()
        {
            string authenticationQueryString = $"?api_key={_wnbaAuthenticationKey}";
            string wnbaGameEventUrl = $"{_wnbaGameEventBaseUrl}{authenticationQueryString}";
            Uri wnbaGameEventUri = new Uri(wnbaGameEventUrl);
            return wnbaGameEventUri;
        }

        public override async Task SubscribeToGameEvents()
        {
            Uri wnbaGameEventUri = GetGameEventUri();
            await SubscribeToGameEventsBase(wnbaGameEventUri, RaiseGameEvent);
        }

        private void RaiseGameEvent(string jsonString)
        {
            try
            {
                TimeOfLastRadarGameEventOrHeartbeat = DateTime.Now;
                bool isHeartbeat = jsonString.Contains("heartbeat");
                WnbaGameEvent mlbGameEvent = null;

                if (isHeartbeat)
                {
                  //  Logger.Info("Radar heartbeat");
                }

                if (!isHeartbeat)
                {
                    mlbGameEvent = WnbaGameEvent.FromJson(jsonString);
                }

                bool isGameEvent = mlbGameEvent != null;

                if (isGameEvent)
                {
                    WnbaGameEventEventArgs wnbaGameEventEventArgs = new WnbaGameEventEventArgs
                    {
                        GameEvent = mlbGameEvent
                    };
                    OnRadarGameEvent(wnbaGameEventEventArgs);
                }
            }
            catch (Exception e)
            {
                Logger.Info(e);
            }
        }

        #endregion

        #region Game Summary

        public Uri GetGameSummaryUri(Guid gameId)
        {
            // see: https://developer.sportradar.com/docs/read/basketball/WNBA_v4#game-summary
            const string format = "xml";
            string wnbaGameSummaryUrl = $"{_wnbaGameInfoBaseUrl}{gameId}/summary.{format}?api_key={_wnbaAuthenticationKey}";
            Uri wnbaGameSummaryUri = new Uri(wnbaGameSummaryUrl);
            return wnbaGameSummaryUri;
        }

        public WnbaGameInfo GetGameSummary(Guid gameId)
        {
            // the SportRadar game boxscore and game summary use the same schema and base URL
            Uri gameSummaryUri = GetGameSummaryUri(gameId);
            string gameSummaryString = ReadResponseFromUri(gameSummaryUri);
            StringReader stringReader = new StringReader(gameSummaryString);

            if (GameInfoXmlSerializer == null)
            {
                GameInfoXmlSerializer = new XmlSerializer(typeof(WnbaGameInfo));
            }

            WnbaGameInfo wnbaGameSummary = (WnbaGameInfo)GameInfoXmlSerializer.Deserialize(stringReader);

            return wnbaGameSummary;
        }

        #endregion
    }
}
