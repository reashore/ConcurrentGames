using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SportsIq.Models.SportRadar.Ncaafb.GameEvents;
using SportsIq.Models.SportRadar.Ncaafb.GameInfo;
using SportsIq.Pusher;

namespace SportsIq.SportsRadar.Ncaafb
{
    public class NcaafbGameEventEventArgs : EventArgs
    {
        public NcaafbGameEvent GameEvent { get; set; }
    }

    public interface IRadarNcaafb : IRadarBase
    {
        Uri GetGameInfoUri(Guid gameId);
        NcaafbGameInfo GetGameInfo(Guid gameId);

        Uri GetGameEventUri();
        event EventHandler<NcaafbGameEventEventArgs> RadarGameEvent;
        DateTime TimeOfLastRadarGameEventOrHeartbeat { get; set; }

        Uri GetGameSummaryUri(Guid gameId);
        NcaafbGameInfo GetGameSummary(Guid gameId);
    }

    public class RadarNcaafb : RadarBase, IRadarNcaafb
    {
        private readonly string _ncaafbGameInfoBaseUrl;
        private string _ncaafbGameEventBaseUrl;
        private readonly string _ncaafbAuthenticationKey;

        public RadarNcaafb(IPusherUtil pusherUtil)
        {
            PusherUtil = pusherUtil;
            _ncaafbGameInfoBaseUrl = ConfigurationManager.AppSettings["sportRadarNcaafbGameInfoBaseUrl"];
            _ncaafbGameEventBaseUrl = ConfigurationManager.AppSettings["sportRadarNcaafbGameEventBaseUrl"];
            _ncaafbAuthenticationKey = ConfigurationManager.AppSettings["sportRadarNcaafbAuthenticationKey"];
        }

        #region Game Info

        public Uri GetGameInfoUri(Guid gameId)
        {
            // this is the SportRadar boxscore format,
            // see: https://developer.sportradar.com/docs/read/basketball/WNBA_v4#game-boxscore
            const string format = "xml";
            string ncaafbGameInfoUrl = $"{_ncaafbGameInfoBaseUrl}/{gameId}/boxscore.{format}?api_key={_ncaafbAuthenticationKey}";
            Uri ncaafbGameInfoUri = new Uri(ncaafbGameInfoUrl);
            return ncaafbGameInfoUri;
        }

        public NcaafbGameInfo GetGameInfo(Guid gameId)
        {
            Uri ncaafbGameInfoUri = GetGameInfoUri(gameId);

            if (GameInfoXmlSerializer == null)
            {
                GameInfoXmlSerializer = new XmlSerializer(typeof(NcaafbGameInfo));
            }

            NcaafbGameInfo ncaafbGameInfo = GetGameInfo<NcaafbGameInfo>(ncaafbGameInfoUri);
            return ncaafbGameInfo;
        }

        #endregion

        #region Game Event

        public event EventHandler<NcaafbGameEventEventArgs> RadarGameEvent;

        private void OnRadarGameEvent(NcaafbGameEventEventArgs ncaafbGameEventEventArgs)
        {
            RadarGameEvent?.Invoke(this, ncaafbGameEventEventArgs);
        }

        public Uri GetGameEventUri()
        {
            _ncaafbGameEventBaseUrl = AdjustGameEventBaseUrlForSimulation(_ncaafbGameEventBaseUrl);
            string authenticationQueryString = $"?api_key={_ncaafbAuthenticationKey}";
            string ncaafbGameEventUrl = $"{_ncaafbGameEventBaseUrl}{authenticationQueryString}";
            Uri ncaafbGameEventUri = new Uri(ncaafbGameEventUrl);
            return ncaafbGameEventUri;
        }

        public override async Task SubscribeToGameEvents()
        {
            Uri ncaafbGameEventUri = GetGameEventUri();
            await SubscribeToGameEventsBase(ncaafbGameEventUri, RaiseGameEvent);
        }

        private void RaiseGameEvent(string jsonString)
        {
            TimeOfLastRadarGameEventOrHeartbeat = DateTime.Now;
            bool isHeartbeat = jsonString.Contains("heartbeat");
            NcaafbGameEvent ncaafbGameEvent = null;

            //if (isHeartbeat)
            //{
            //    Logger.Info("Radar heartbeat");
            //}
            
            if (!isHeartbeat)
            {
                Logger.Info(jsonString);
                ncaafbGameEvent = NcaafbGameEvent.FromJson(jsonString);
            }

            bool isGameEvent = ncaafbGameEvent != null;
            
            if (isGameEvent)
            {
                NcaafbGameEventEventArgs ncaafbGameEventEventArgs = new NcaafbGameEventEventArgs
                {
                    GameEvent = ncaafbGameEvent
                };
                OnRadarGameEvent(ncaafbGameEventEventArgs);
            }
        }

        #endregion

        #region Game Summary

        public Uri GetGameSummaryUri(Guid gameId)
        {
            // see: 
            const string format = "xml";
            string ncaafbGameSummaryUrl = $"{_ncaafbGameInfoBaseUrl}/{gameId}/summary.{format}?api_key={_ncaafbAuthenticationKey}";
            Uri ncaafbGameSummaryUri = new Uri(ncaafbGameSummaryUrl);
            return ncaafbGameSummaryUri;
        }

        public NcaafbGameInfo GetGameSummary(Guid gameId)
        {
            // the SportRadar game boxscore and game summary use the same schema and base URL
            Uri gameSummaryUri = GetGameSummaryUri(gameId);
            string gameSummaryString = ReadResponseFromUri(gameSummaryUri);
            StringReader stringReader = new StringReader(gameSummaryString);

            if (GameInfoXmlSerializer == null)
            {
                GameInfoXmlSerializer = new XmlSerializer(typeof(NcaafbGameInfo));
            }

            NcaafbGameInfo ncaafbGameSummary = (NcaafbGameInfo) GameInfoXmlSerializer.Deserialize(stringReader);

            return ncaafbGameSummary;
        }

        #endregion
    }
}
