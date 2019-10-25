using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SportsIq.Models.SportRadar.Ncaabb.GameEvents;
using SportsIq.Models.SportRadar.Ncaabb.GameInfo;
using SportsIq.Pusher;

namespace SportsIq.SportsRadar.Ncaabb
{
    public class NcaabbGameEventEventArgs : EventArgs
    {
        public NcaabbGameEvent GameEvent { get; set; }
    }

    public interface IRadarNcaabb : IRadarBase
    {
        Uri GetGameInfoUri(Guid gameId);
        NcaabbGameInfo GetGameInfo(Guid gameId);

        Uri GetGameEventUri();
        event EventHandler<NcaabbGameEventEventArgs> RadarGameEvent;
        DateTime TimeOfLastRadarGameEventOrHeartbeat { get; set; }

        Uri GetGameSummaryUri(Guid gameId);
        NcaabbGameInfo GetGameSummary(Guid gameId);
    }

    public class RadarNcaabb : RadarBase, IRadarNcaabb
    {
        private readonly string _ncaabbGameInfoBaseUrl;
        private readonly string _ncaabbGameEventBaseUrl;
        private readonly string _ncaabbAuthenticationKey;

        public RadarNcaabb(IPusherUtil pusherUtil)
        {
            PusherUtil = pusherUtil;
            _ncaabbGameInfoBaseUrl = ConfigurationManager.AppSettings["sportRadarNcaabbGameInfoBaseUrl"];
            _ncaabbGameEventBaseUrl = ConfigurationManager.AppSettings["sportRadarNcaabbGameEventBaseUrl"];
            _ncaabbAuthenticationKey = ConfigurationManager.AppSettings["sportRadarNcaabbAuthenticationKey"];
            TimeOfLastRadarGameEventOrHeartbeat = DateTime.Now;
        }

        #region Game Info

        public Uri GetGameInfoUri(Guid gameId)
        {
            // this is the SportRadar boxscore format, see https://developer.sportradar.com/docs/read/basketball/WNBA_v4#game-boxscore
            const string format = "xml";
            string ncaabbGameInfoUrl = $"{_ncaabbGameInfoBaseUrl}{gameId}/boxscore.{format}?api_key={_ncaabbAuthenticationKey}";
            Uri ncaabbGameInfoUri = new Uri(ncaabbGameInfoUrl);
            return ncaabbGameInfoUri;
        }

        public NcaabbGameInfo GetGameInfo(Guid gameId)
        {
            Uri ncaabbGameInfoUri = GetGameInfoUri(gameId);

            if (GameInfoXmlSerializer == null)
            {
                GameInfoXmlSerializer = new XmlSerializer(typeof(NcaabbGameInfo));
            }

            NcaabbGameInfo ncaabbGameInfo = GetGameInfo<NcaabbGameInfo>(ncaabbGameInfoUri);
            return ncaabbGameInfo;
        }

        #endregion

        #region Game Event

        public event EventHandler<NcaabbGameEventEventArgs> RadarGameEvent;

        private void OnRadarGameEvent(NcaabbGameEventEventArgs ncaabbGameEventEventArgs)
        {
            RadarGameEvent?.Invoke(this, ncaabbGameEventEventArgs);
        }

        public Uri GetGameEventUri()
        {
            string authenticationQueryString = $"?api_key={_ncaabbAuthenticationKey}";
            string ncaabbGameEventUrl = $"{_ncaabbGameEventBaseUrl}{authenticationQueryString}";
            Uri ncaabbGameEventUri = new Uri(ncaabbGameEventUrl);
            return ncaabbGameEventUri;
        }

        public override async Task SubscribeToGameEvents()
        {
            Uri ncaabbGameEventUri = GetGameEventUri();
            await SubscribeToGameEventsBase(ncaabbGameEventUri, RaiseGameEvent);
        }

        private void RaiseGameEvent(string jsonString)
        {
            TimeOfLastRadarGameEventOrHeartbeat = DateTime.Now;
            bool isHeartbeat = jsonString.Contains("heartbeat");
            NcaabbGameEvent mlbGameEvent = null;

            if (isHeartbeat)
            {
                //  Logger.Info("Radar heartbeat");
            }

            if (!isHeartbeat)
            {
                mlbGameEvent = NcaabbGameEvent.FromJson(jsonString);
            }

            bool isGameEvent = mlbGameEvent != null;

            if (isGameEvent)
            {
                NcaabbGameEventEventArgs ncaabbGameEventEventArgs = new NcaabbGameEventEventArgs
                {
                    GameEvent = mlbGameEvent
                };
                OnRadarGameEvent(ncaabbGameEventEventArgs);
            }
        }

        #endregion

        #region Game Summary

        public Uri GetGameSummaryUri(Guid gameId)
        {
            // see: https://developer.sportradar.com/docs/read/basketball/WNBA_v4#game-summary
            const string format = "xml";
            string ncaabbGameSummaryUrl = $"{_ncaabbGameInfoBaseUrl}{gameId}/summary.{format}?api_key={_ncaabbAuthenticationKey}";
            Uri ncaabbGameSummaryUri = new Uri(ncaabbGameSummaryUrl);
            return ncaabbGameSummaryUri;
        }

        public NcaabbGameInfo GetGameSummary(Guid gameId)
        {
            // the SportRadar game boxscore and game summary use the same schema and base URL
            Uri gameSummaryUri = GetGameSummaryUri(gameId);
            string gameSummaryString = ReadResponseFromUri(gameSummaryUri);
            StringReader stringReader = new StringReader(gameSummaryString);

            if (GameInfoXmlSerializer == null)
            {
                GameInfoXmlSerializer = new XmlSerializer(typeof(NcaabbGameInfo));
            }

            NcaabbGameInfo ncaabbGameSummary = (NcaabbGameInfo)GameInfoXmlSerializer.Deserialize(stringReader);

            return ncaabbGameSummary;
        }

        #endregion
    }
}
