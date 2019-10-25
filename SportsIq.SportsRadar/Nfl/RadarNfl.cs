using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SportsIq.Models.SportRadar.Nfl.GameEvents;
using SportsIq.Models.SportRadar.Nfl.GameInfo;
using SportsIq.Pusher;

namespace SportsIq.SportsRadar.Nfl
{
    public class NflGameEventEventArgs : EventArgs
    {
        public NflGameEvent GameEvent { get; set; }
    }

    public interface IRadarNfl : IRadarBase
    {
        Uri GetGameInfoUri(Guid gameId);
        NflGameInfo GetGameInfo(Guid gameId);

        Uri GetGameEventUri();
        event EventHandler<NflGameEventEventArgs> RadarGameEvent;
        DateTime TimeOfLastRadarGameEventOrHeartbeat { get; set; }

        Uri GetGameSummaryUri(Guid gameId);
        NflGameInfo GetGameSummary(Guid gameId);
    }

    public class RadarNfl : RadarBase, IRadarNfl
    {
        private readonly string _nflGameInfoBaseUrl;
        private string _nflGameEventBaseUrl;
        private readonly string _nflAuthenticationKey;

        public RadarNfl(IPusherUtil pusherUtil)
        {
            PusherUtil = pusherUtil;
            // todo sportRadarNflGameInfoBaseUrl is invalid
            _nflGameInfoBaseUrl = ConfigurationManager.AppSettings["sportRadarNflGameInfoBaseUrl"];
            _nflGameEventBaseUrl = ConfigurationManager.AppSettings["sportRadarNflGameEventBaseUrl"];
            _nflAuthenticationKey = ConfigurationManager.AppSettings["sportRadarNflAuthenticationKey"];
        }

        #region Game Info

        public Uri GetGameInfoUri(Guid gameId)
        {
            // this is the SportRadar boxscore format,
            // see: https://developer.sportradar.com/docs/read/basketball/WNBA_v4#game-boxscore
            const string format = "xml";
            string nflGameInfoUrl = $"{_nflGameInfoBaseUrl}/{gameId}/boxscore.{format}?api_key={_nflAuthenticationKey}";
            Uri nflGameInfoUri = new Uri(nflGameInfoUrl);
            return nflGameInfoUri;
        }

        public NflGameInfo GetGameInfo(Guid gameId)
        {
            Uri nflGameInfoUri = GetGameInfoUri(gameId);

            if (GameInfoXmlSerializer == null)
            {
                GameInfoXmlSerializer = new XmlSerializer(typeof(NflGameInfo));
            }

            NflGameInfo nflGameInfo = GetGameInfo<NflGameInfo>(nflGameInfoUri);
            return nflGameInfo;
        }

        #endregion

        #region Game Event

        public event EventHandler<NflGameEventEventArgs> RadarGameEvent;

        private void OnRadarGameEvent(NflGameEventEventArgs nflGameEventEventArgs)
        {
            RadarGameEvent?.Invoke(this, nflGameEventEventArgs);
        }

        public Uri GetGameEventUri()
        {
            _nflGameEventBaseUrl = AdjustGameEventBaseUrlForSimulation(_nflGameEventBaseUrl);
            string authenticationQueryString = $"?api_key={_nflAuthenticationKey}";
            string nflGameEventUrl = $"{_nflGameEventBaseUrl}{authenticationQueryString}";
            Uri nflGameEventUri = new Uri(nflGameEventUrl);
            return nflGameEventUri;
        }

        public override async Task SubscribeToGameEvents()
        {
            Uri nflGameEventUri = GetGameEventUri();
            await SubscribeToGameEventsBase(nflGameEventUri, RaiseGameEvent);
        }

        private void RaiseGameEvent(string jsonString)
        {
            TimeOfLastRadarGameEventOrHeartbeat = DateTime.Now;
            bool isHeartbeat = jsonString.Contains("heartbeat");
            NflGameEvent nflGameEvent = null;

            if (isHeartbeat)
            {
               // Logger.Info("Radar heartbeat");
            }
            
            if (!isHeartbeat)
            {
                Logger.Info(jsonString);
                nflGameEvent = NflGameEvent.FromJson(jsonString);
            }

            bool isGameEvent = nflGameEvent != null;
            
            if (isGameEvent)
            {
                NflGameEventEventArgs nflGameEventEventArgs = new NflGameEventEventArgs
                {
                    GameEvent = nflGameEvent
                };
                OnRadarGameEvent(nflGameEventEventArgs);
            }
        }

        #endregion

        #region Game Summary

        public Uri GetGameSummaryUri(Guid gameId)
        {
            // see: 
            const string format = "xml";
            string nflGameSummaryUrl = $"{_nflGameInfoBaseUrl}/{gameId}/summary.{format}?api_key={_nflAuthenticationKey}";
            Uri nflGameSummaryUri = new Uri(nflGameSummaryUrl);
            return nflGameSummaryUri;
        }

        public NflGameInfo GetGameSummary(Guid gameId)
        {
            // the SportRadar game boxscore and game summary use the same schema and base URL
            Uri gameSummaryUri = GetGameSummaryUri(gameId);
            string gameSummaryString = ReadResponseFromUri(gameSummaryUri);
            StringReader stringReader = new StringReader(gameSummaryString);

            if (GameInfoXmlSerializer == null)
            {
                GameInfoXmlSerializer = new XmlSerializer(typeof(NflGameInfo));
            }

            NflGameInfo nflGameSummary = (NflGameInfo) GameInfoXmlSerializer.Deserialize(stringReader);

            return nflGameSummary;
        }

        #endregion
    }
}
