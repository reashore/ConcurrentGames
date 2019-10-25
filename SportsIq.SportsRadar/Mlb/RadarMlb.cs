using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SportsIq.Models.SportRadar.Mlb.GameEvents;
using SportsIq.Models.SportRadar.Mlb.GameInfo;
using SportsIq.Pusher;

namespace SportsIq.SportsRadar.Mlb
{
    public class MlbGameEventEventArgs : EventArgs
    {
        public MlbGameEvent GameEvent { get; set; }
    }

    public interface IRadarMlb : IRadarBase
    {
        Uri GetGameInfoUri(Guid gameId);
        MlbGameInfo GetGameInfo(Guid gameId);

        Uri GetGameEventUri();
        event EventHandler<MlbGameEventEventArgs> RadarGameEvent;
        DateTime TimeOfLastRadarGameEventOrHeartbeat { get; set;}

        Uri GetGameSummaryUri(Guid gameId);
        MlbGameInfo GetGameSummary(Guid gameId);
    }

    public class RadarMlb : RadarBase, IRadarMlb
    {
        private readonly string _mlbGameInfoBaseUrl;
        private readonly string _mlbGameEventBaseUrl;
        private readonly string _mlbAuthenticationKey;

        public RadarMlb(IPusherUtil pusherUtil)
        {
            PusherUtil = pusherUtil;
            _mlbGameInfoBaseUrl = ConfigurationManager.AppSettings["sportRadarMlbGameInfoBaseUrl"];
            _mlbGameEventBaseUrl = ConfigurationManager.AppSettings["sportRadarMlbGameEventBaseUrl"];
            _mlbAuthenticationKey = ConfigurationManager.AppSettings["sportRadarMlbAuthenticationKey"];
        }

        #region Game Info

        public Uri GetGameInfoUri(Guid gameId)
        {
            // this is the SportRadar boxscore format,
            // see:  https://developer.sportradar.com/docs/read/baseball/MLB_v65#game-boxscore
            const string format = "xml";
            string mlbGameInfoUrl = $"{_mlbGameInfoBaseUrl}{gameId}/boxscore.{format}?api_key={_mlbAuthenticationKey}";
            Uri mlbGameInfoUri = new Uri(mlbGameInfoUrl);
            return mlbGameInfoUri;
        }

        public MlbGameInfo GetGameInfo(Guid gameId)
        {
            Uri mlbGameInfoUri = GetGameInfoUri(gameId);

            if (GameInfoXmlSerializer == null)
            {
                GameInfoXmlSerializer = new XmlSerializer(typeof(MlbGameInfo));
            }

            MlbGameInfo mlbGameInfo = GetGameInfo<MlbGameInfo>(mlbGameInfoUri);
            return mlbGameInfo;
        }

        #endregion

        #region Game Event

        public event EventHandler<MlbGameEventEventArgs> RadarGameEvent;

        private void OnRadarGameEvent(MlbGameEventEventArgs mlbGameEventEventArgs)
        {
            RadarGameEvent?.Invoke(this, mlbGameEventEventArgs);
        }

        public Uri GetGameEventUri()
        {
            string queryString = $"?api_key={_mlbAuthenticationKey}";
            string mlbGameEventUrl = $"{_mlbGameEventBaseUrl}{queryString}";
            Uri mlbGameEventUri = new Uri(mlbGameEventUrl);
            return mlbGameEventUri;
        }

        public override async Task SubscribeToGameEvents()
        {
            Uri mlbGameEventUri = GetGameEventUri();
            await SubscribeToGameEventsBase(mlbGameEventUri, RaiseRadarGameEvent);
        }

        private void RaiseRadarGameEvent(string jsonString)
        {
            TimeOfLastRadarGameEventOrHeartbeat = DateTime.Now;
            bool isHeartbeat = jsonString.Contains("heartbeat");
            MlbGameEvent mlbGameEvent = null;

            if (isHeartbeat)
            {
#if DEBUG
                 Logger.Info("Radar heartbeat");
#endif
            }
            
            if (!isHeartbeat)
            {
                mlbGameEvent = MlbGameEvent.FromJson(jsonString);
            }

            bool isGameEvent = mlbGameEvent != null;
            
            if (isGameEvent)
            {
                MlbGameEventEventArgs mlbGameEventEventArgs = new MlbGameEventEventArgs
                {
                    GameEvent = mlbGameEvent
                };
                OnRadarGameEvent(mlbGameEventEventArgs);
            }
        }

#endregion

#region Game Summary

        public Uri GetGameSummaryUri(Guid gameId)
        {
            // see: https://developer.sportradar.com/docs/read/baseball/MLB_v65#game-summary
            const string format = "xml";
            string mlbGameSummaryUrl = $"{_mlbGameInfoBaseUrl}{gameId}/summary.{format}?api_key={_mlbAuthenticationKey}";
            Uri mlbGameSummaryUri = new Uri(mlbGameSummaryUrl);
            return mlbGameSummaryUri;
        }

        public MlbGameInfo GetGameSummary(Guid gameId)
        {
            // the SportRadar game boxscore and game summary use the same schema and base URL
            Uri gameSummaryUri = GetGameSummaryUri(gameId);
            string gameSummaryString = ReadResponseFromUri(gameSummaryUri);
            StringReader stringReader = new StringReader(gameSummaryString);

            if (GameInfoXmlSerializer == null)
            {
                GameInfoXmlSerializer = new XmlSerializer(typeof(MlbGameInfo));
            }

            MlbGameInfo mlbMlbGameInfo = (MlbGameInfo)GameInfoXmlSerializer.Deserialize(stringReader);
            return mlbMlbGameInfo;
        }

#endregion

#region Liquid XML serialization code
        
        //public MlbGameInfo GetGameInfo(Guid gameId)
        //{
        //    //Test(gameId);
        //    Uri mlbGameInfoUri = GetGameInfoUri(gameId);
        //    LxSerializer<MlbGameInfo> serializer = new LxSerializer<MlbGameInfo>();
        //    string gameInfoXmlString = ReadResponseFromUri(mlbGameInfoUri);
        //    StringReader stringReader = new StringReader(gameInfoXmlString);
        //    MlbGameInfo mlbGameInfo = serializer.Deserialize(stringReader);
        //    return mlbGameInfo;
        //}
        
        //public MlbGameInfo Test(Guid gameId)
        //{
        //    Uri mlbGameInfoUri = GetGameInfoUri(gameId);
        //    LxSerializer<MlbGameInfo> serializer = new LxSerializer<MlbGameInfo>();
        //    string gameInfoXmlString = ReadResponseFromUri(mlbGameInfoUri);
        //    StringReader stringReader = new StringReader(gameInfoXmlString);
        //    MlbGameInfo mlbGameInfo = serializer.Deserialize(stringReader);
        //    return mlbGameInfo;
        //}

#endregion
    }
}
