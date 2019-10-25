using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SportsIq.Models.SportRadar.Nba.GameEvents;
using SportsIq.Models.SportRadar.Nba.GameInfo;
using SportsIq.Pusher;

namespace SportsIq.SportsRadar.Nba
{
    public class NbaGameEventEventArgs : EventArgs
    {
        public NbaGameEvent GameEvent { get; set; }
    }

    public interface IRadarNba : IRadarBase
    {
        Uri GetGameInfoUri(Guid gameId);
        NbaGameInfo GetGameInfo(Guid gameId);

        Uri GetGameEventUri();
        event EventHandler<NbaGameEventEventArgs> RadarGameEvent;
        DateTime TimeOfLastRadarGameEventOrHeartbeat { get; set; }

        Uri GetGameSummaryUri(Guid gameId);
        NbaGameInfo GetGameSummary(Guid gameId);
    }

    public class RadarNba : RadarBase, IRadarNba
    {
        private readonly string _nbaGameInfoBaseUrl;
        private readonly string _nbaGameEventBaseUrl;
        private readonly string _nbaAuthenticationKey;

        public RadarNba(IPusherUtil pusherUtil)
        {
            PusherUtil = pusherUtil;
            _nbaGameInfoBaseUrl = ConfigurationManager.AppSettings["sportRadarNbaGameInfoBaseUrl"];
            _nbaGameEventBaseUrl = ConfigurationManager.AppSettings["sportRadarNbaGameEventBaseUrl"];
            _nbaAuthenticationKey = ConfigurationManager.AppSettings["sportRadarNbaAuthenticationKey"];
            TimeOfLastRadarGameEventOrHeartbeat = DateTime.Now;
        }

        #region Game Info

        public Uri GetGameInfoUri(Guid gameId)
        {
            // this is the SportRadar boxscore format, see https://developer.sportradar.com/docs/read/basketball/WNBA_v4#game-boxscore
            const string format = "xml";
            string nbaGameInfoUrl = $"{_nbaGameInfoBaseUrl}{gameId}/boxscore.{format}?api_key={_nbaAuthenticationKey}";
            Uri nbaGameInfoUri = new Uri(nbaGameInfoUrl);
            return nbaGameInfoUri;
        }

        public NbaGameInfo GetGameInfo(Guid gameId)
        {
            Uri nbaGameInfoUri = GetGameInfoUri(gameId);

            if (GameInfoXmlSerializer == null)
            {
                GameInfoXmlSerializer = new XmlSerializer(typeof(NbaGameInfo));
            }

            NbaGameInfo nbaGameInfo = GetGameInfo<NbaGameInfo>(nbaGameInfoUri);
            return nbaGameInfo;
        }

        #endregion

        #region Game Event

        public event EventHandler<NbaGameEventEventArgs> RadarGameEvent;

        private void OnRadarGameEvent(NbaGameEventEventArgs nbaGameEventEventArgs)
        {
            RadarGameEvent?.Invoke(this, nbaGameEventEventArgs);
        }

        public Uri GetGameEventUri()
        {
            string authenticationQueryString = $"?api_key={_nbaAuthenticationKey}";
            string nbaGameEventUrl = $"{_nbaGameEventBaseUrl}{authenticationQueryString}";
            Uri nbaGameEventUri = new Uri(nbaGameEventUrl);
            return nbaGameEventUri;
        }

        public override async Task SubscribeToGameEvents()
        {
            Uri nbaGameEventUri = GetGameEventUri();
            await SubscribeToGameEventsBase(nbaGameEventUri, RaiseGameEvent);
        }

        private void RaiseGameEvent(string jsonString)
        {
            try
            {
                TimeOfLastRadarGameEventOrHeartbeat = DateTime.Now;
                bool isHeartbeat = jsonString.Contains("heartbeat");
                NbaGameEvent mlbGameEvent = null;

                if (isHeartbeat)
                {
#if DEBUG
                      Logger.Info("Radar heartbeat");
#endif
                }

                if (!isHeartbeat)
                {
                    Logger.Info(jsonString);
                    mlbGameEvent = NbaGameEvent.FromJson(jsonString);
                }

                bool isGameEvent = mlbGameEvent != null;

                if (isGameEvent)
                {
                    NbaGameEventEventArgs nbaGameEventEventArgs = new NbaGameEventEventArgs
                    {
                        GameEvent = mlbGameEvent
                    };
                    OnRadarGameEvent(nbaGameEventEventArgs);
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
            string nbaGameSummaryUrl = $"{_nbaGameInfoBaseUrl}{gameId}/summary.{format}?api_key={_nbaAuthenticationKey}";
            Uri nbaGameSummaryUri = new Uri(nbaGameSummaryUrl);
            return nbaGameSummaryUri;
        }

        public NbaGameInfo GetGameSummary(Guid gameId)
        {
            // the SportRadar game boxscore and game summary use the same schema and base URL
            Uri gameSummaryUri = GetGameSummaryUri(gameId);
            string gameSummaryString = ReadResponseFromUri(gameSummaryUri);
            StringReader stringReader = new StringReader(gameSummaryString);

            if (GameInfoXmlSerializer == null)
            {
                GameInfoXmlSerializer = new XmlSerializer(typeof(NbaGameInfo));
            }

            NbaGameInfo nbaGameSummary = (NbaGameInfo)GameInfoXmlSerializer.Deserialize(stringReader);

            return nbaGameSummary;
        }

#endregion
    }
}
