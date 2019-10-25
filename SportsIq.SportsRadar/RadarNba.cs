using SportsIq.Models.PubSub;
using SportsIq.Models.Queues;
using SportsIq.Pusher;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using SportsIq.Models.GamesOld.Nba.GameEvent;
using SportsIq.Models.GamesOld.Nba.GameInfo;
using SportsIq.Models.SportRadar;

namespace SportsIq.SportsRadar
{
    public interface IRadarNba
    {
        Uri GetGameInfoUri(Guid gameId);
        Uri GetGameEventUri();
        NbaGameInfo GetGameInfo(Guid gameId);
        Task<NbaGameEvent> GetGameEvent();
        Task SubscribeToGameEvents(Queues<NbaGameEvent, PubMessage> queues, Dictionary<Guid, int> gameDictionary);
    }

    public class RadarNba : RadarBase, IRadarNba
    {
        private readonly string _nbaGameInfoBaseUrl;
        private readonly string _nbaGameEventBaseUrl;
        private readonly string _nbaAuthenticationKey;
        private readonly IPusherUtil _pusherUtil;
        private int _heartBeatNumber;

        public RadarNba()
        {
            _nbaGameInfoBaseUrl = ConfigurationManager.AppSettings["sportRadarNbaGameInfoBaseUrl"];
            _nbaGameEventBaseUrl = ConfigurationManager.AppSettings["sportRadarNbaGameEventBaseUrl"];
            _nbaAuthenticationKey = ConfigurationManager.AppSettings["sportRadarNbaAuthenticationKey"];
            _pusherUtil = new PusherUtil();
        }

        //-------------------------------------------------------------

        public Uri GetGameInfoUri(Guid gameId)
        {
            string nbaGameInfoUrl = $"{_nbaGameInfoBaseUrl}{gameId}/boxscore.json?api_key={_nbaAuthenticationKey}";
            Uri nbaGameInfoUri = new Uri(nbaGameInfoUrl);
            return nbaGameInfoUri;
        }

        public Uri GetGameEventUri()
        {
            string queryString = $"?api_key={_nbaAuthenticationKey}";
            string nbaGameEventUrl = $"{_nbaGameEventBaseUrl}{queryString}";
            Uri subscriptionNbaUri = new Uri(nbaGameEventUrl);
            return subscriptionNbaUri;
        }

        //-------------------------------------------------------------

        public NbaGameInfo GetGameInfo(Guid gameId)
        {
            Uri nbaGameInfoUri = GetGameInfoUri(gameId);
            //NbaGameInfo nbaGameInfo = GetGameInfoJson<NbaGameInfo>(nbaGameInfoUri);

            // todo this is faked in order to get code to compile for now *******************
            NbaGameInfo nbaGameInfo = new NbaGameInfo();

            return nbaGameInfo;
        }

        public async Task<NbaGameEvent> GetGameEvent()
        {
            Uri nbaGameEventUri = GetGameEventUri();
            NbaGameEvent nbaGameEvent = await GetGameEvent<NbaGameEvent>(nbaGameEventUri);
            return nbaGameEvent;
        }

        //-------------------------------------------------------------

        public async Task SubscribeToGameEvents(Queues<NbaGameEvent, PubMessage> queues, Dictionary<Guid, int> gameDictionary)
        {
            Uri nbaGameEventUri = GetGameEventUri();
            await SubscribeToGameEvents(nbaGameEventUri, queues, gameDictionary, PushGameEventOnQueue);
        }

        private void PushGameEventOnQueue(string jsonString, Queues<NbaGameEvent, PubMessage> queues, Dictionary<Guid, int> gameDictionary)
        {
            NbaGameEvent nbaGameEvent = null;
            Heartbeat heartbeat = null;
            bool isGameEvent;
            bool isHeartbeat;

            try
            {
                nbaGameEvent = NbaGameEvent.FromJson(jsonString);
                isGameEvent = true;
            }
            catch (NullReferenceException)
            {
                isGameEvent = false;
            }

            try
            {
                heartbeat = Heartbeat.FromJson(jsonString);
                isHeartbeat = true;
            }
            catch (Exception)
            {
                isHeartbeat = false;
            }

            if (!isGameEvent && !isHeartbeat)
            {
                throw new Exception("Failed to deserialize both GameEvent and Heartbeat");
            }

            if (isGameEvent && nbaGameEvent?.Payload != null)
            {
                Guid gameId = nbaGameEvent.Payload.Game.Id;
                int gameIndex = gameDictionary[gameId];

                queues.ScoreQueueList[gameIndex].Push(nbaGameEvent);
            }

            if (isHeartbeat && heartbeat != null)
            {
                _heartBeatNumber++;

                if (_heartBeatNumber > 2)
                {
                    _pusherUtil.SendHeartBeat();
                    _heartBeatNumber = 0;
                }
            }
        }
    }
}
