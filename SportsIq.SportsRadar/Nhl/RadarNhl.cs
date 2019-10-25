using System;
using System.Configuration;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SportsIq.Models.SportRadar.Nhl.GameEvents;
using SportsIq.Models.SportRadar.Nhl.GameInfo;
using SportsIq.Pusher;

namespace SportsIq.SportsRadar.Nhl
{

    public class NhlGameEventEventArgs : EventArgs
    {
        public NhlGameEvent GameEvent { get; set; }
    }

    public interface IRadarNhl : IRadarBase
    {
        Uri GetGameInfoUri(Guid gameId);
        NhlGameInfo GetGameInfo(Guid gameId);

        Uri GetGameEventUri();
        event EventHandler<NhlGameEventEventArgs> RadarGameEvent;
        DateTime TimeOfLastRadarGameEventOrHeartbeat { get; set; }

        Uri GetGameSummaryUri(Guid gameId);
        NhlGameInfo GetGameSummary(Guid gameId);
    }

    public class RadarNhl : RadarBase, IRadarNhl
    {
        private readonly string _nhlGameInfoBaseUrl;
        private readonly string _nhlGameEventBaseUrl;
        private readonly string _nhlAuthenticationKey;

        public RadarNhl(IPusherUtil pusherUtil)
        {
            PusherUtil = pusherUtil;
            _nhlGameInfoBaseUrl = ConfigurationManager.AppSettings["sportRadarNhlGameInfoBaseUrl"];
            _nhlGameEventBaseUrl = ConfigurationManager.AppSettings["sportRadarNhlGameEventBaseUrl"];
            _nhlAuthenticationKey = ConfigurationManager.AppSettings["sportRadarNhlAuthenticationKey"];
            TimeOfLastRadarGameEventOrHeartbeat = DateTime.Now;
        }

        #region Game Info

        public Uri GetGameInfoUri(Guid gameId)
        {
            // this is the SportRadar boxscore format,
            // see:  https://developer.sportradar.com/docs/read/baseball/nhl_v65#game-boxscore
            const string format = "xml";
            string nhlGameInfoUrl = $"{_nhlGameInfoBaseUrl}{gameId}/boxscore.{format}?api_key={_nhlAuthenticationKey}";
            Uri nhlGameInfoUri = new Uri(nhlGameInfoUrl);
            return nhlGameInfoUri;
        }

        public NhlGameInfo GetGameInfo(Guid gameId)
        {
            Uri nhlGameInfoUri = GetGameInfoUri(gameId);

            if (GameInfoXmlSerializer == null)
            {
                GameInfoXmlSerializer = new XmlSerializer(typeof(NhlGameInfo));
            }

            NhlGameInfo nhlGameInfo = GetGameInfo<NhlGameInfo>(nhlGameInfoUri);
            return nhlGameInfo;
        }

        #endregion

        #region Game Event

        public event EventHandler<NhlGameEventEventArgs> RadarGameEvent;

        private void OnRadarGameEvent(NhlGameEventEventArgs nhlGameEventEventArgs)
        {
            RadarGameEvent?.Invoke(this, nhlGameEventEventArgs);
        }

        public Uri GetGameEventUri()
        {
            string queryString = $"?api_key={_nhlAuthenticationKey}";
            string nhlGameEventUrl = $"{_nhlGameEventBaseUrl}{queryString}";
            Uri nhlGameEventUri = new Uri(nhlGameEventUrl);
            return nhlGameEventUri;
        }

        public override async Task SubscribeToGameEvents()
        {
            Uri nhlGameEventUri = GetGameEventUri();
            await SubscribeToGameEventsBase(nhlGameEventUri, RaiseRadarGameEvent);
        }

        private void RaiseRadarGameEvent(string jsonString)
        {
            TimeOfLastRadarGameEventOrHeartbeat = DateTime.Now;
            bool isHeartbeat = jsonString.Contains("heartbeat");
            NhlGameEvent nhlGameEvent = null;

            if (isHeartbeat)
            {
                Logger.Info("Radar heartbeat");
            }

            if (!isHeartbeat)
            {
                // Logger.Info(jsonString);
                nhlGameEvent = NhlGameEvent.FromJson(jsonString);
            }

            bool isGameEvent = nhlGameEvent != null;

            if (isGameEvent)
            {
                NhlGameEventEventArgs nhlGameEventEventArgs = new NhlGameEventEventArgs
                {
                    GameEvent = nhlGameEvent
                };
                OnRadarGameEvent(nhlGameEventEventArgs);
            }
        }

        #endregion

        #region Game Summary

        public Uri GetGameSummaryUri(Guid gameId)
        {
            // see: https://developer.sportradar.com/docs/read/baseball/MLB_v65#game-summary
            const string format = "xml";
            string nhlGameSummaryUrl = $"{_nhlGameInfoBaseUrl}{gameId}/summary.{format}?api_key={_nhlAuthenticationKey}";
            Uri nhlGameSummaryUri = new Uri(nhlGameSummaryUrl);
            return nhlGameSummaryUri;
        }

        public NhlGameInfo GetGameSummary(Guid gameId)
        {
            try
            {
                // the SportRadar game boxscore and game summary use the same schema and base URL
                Uri gameSummaryUri = GetGameSummaryUri(gameId);
                string gameSummaryString = ReadResponseFromUri(gameSummaryUri);
                StringReader stringReader = new StringReader(gameSummaryString);

                if (GameInfoXmlSerializer == null)
                {
                    GameInfoXmlSerializer = new XmlSerializer(typeof(NhlGameInfo));
                }

                NhlGameInfo nhlNhlGameInfo = (NhlGameInfo)GameInfoXmlSerializer.Deserialize(stringReader);
                return nhlNhlGameInfo;
            }
            catch (Exception e)
            {
                Logger.Info(e);

                return new NhlGameInfo();
            }
        }

        #endregion
    }
}
