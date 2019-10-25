using System;
using PusherServer;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using log4net;
using SportsIq.Utilities;

namespace SportsIq.Pusher
{
    public interface IPusherUtil
    {
        void SendOddsMessage(string messageBody, string eventName, string key);
        void SendScoreMessage(string messageBody, string eventName, string key);
        void SendHeartbeat();
    }

    public class PusherUtil : IPusherUtil
    {
        private static readonly ILog Logger;
        private readonly PusherServer.Pusher _pusherServer;
        private readonly Dictionary<string, string> _messageDictionary;
        private readonly string _pusherOddsChannelName;
        private readonly string _pusherScoreChannelName;
        private readonly string _pusherEventName;

        static PusherUtil()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(PusherUtil));
        }

        public PusherUtil()
        {
            string pusherAppId = ConfigurationManager.AppSettings["pusherAppId"];
            string pusherAppKey = ConfigurationManager.AppSettings["pusherAppKey"];
            string pusherAppSecret = ConfigurationManager.AppSettings["pusherAppSecret"];
            _pusherOddsChannelName = ConfigurationManager.AppSettings["pusherOddsChannelName"];
            _pusherScoreChannelName = ConfigurationManager.AppSettings["pusherScoreChannelName"];
            _pusherEventName = ConfigurationManager.AppSettings["pusherEventName"];

            PusherOptions pusherOptions = new PusherOptions
            {
                Cluster = "us2",
                Encrypted = true
            };

            _pusherServer = new PusherServer.Pusher(pusherAppId, pusherAppKey, pusherAppSecret, pusherOptions);
            _messageDictionary = new Dictionary<string, string>();
        }

        #region Public Methods

        public void SendOddsMessage(string messageBody, string eventName, string key)
        {
            SendMessage(key, _pusherOddsChannelName, eventName, messageBody);
        }

        public void SendScoreMessage(string messageBody, string eventName, string key)
        {
            SendMessage(key, _pusherScoreChannelName, eventName, messageBody);
        }

        public void SendHeartbeat()
        {
            const string heartbeatMessage = @"{""heartbeat"": {""interval"": 15000}}";
            Task<ITriggerResult> task =  _pusherServer.TriggerAsync(_pusherOddsChannelName, _pusherEventName, heartbeatMessage);
            ITriggerResult result = task.Result;
        }

        #endregion

        private void SendMessage(string key, string channelName, string eventName, string messageBody)
        {
            try
            {
                bool keyExists = _messageDictionary.ContainsKey(key);
                bool messageBodyIsNew = keyExists && _messageDictionary[key] != messageBody;

                if (!keyExists || messageBodyIsNew)
                {
#if DEBUG
                    //Logger.Info($"sending {eventName}  {key} ");
#endif
                    Task<ITriggerResult> task = _pusherServer.TriggerAsync(channelName, eventName, messageBody);
                    ITriggerResult result = task.Result;
                    HttpStatusCode statusCode = result.StatusCode;
                    string body = result.Body;

                    if (statusCode != HttpStatusCode.OK)
                    {
                        string message = $"SendMessage() failed to return OK status code, statusCode = {statusCode}, body = {body}";
                        Logger.Error(message);
                        throw new Exception(message);
                    }

                    if (!Utils.IsValidJson(body))
                    {
                        Logger.Error($"SendMessage(): Pusher body is not valid JSON, body = {body}");
                    }

                    // ERROR 2019-09-18 17:47:40,781 [22] SendMessage() exception = System.IndexOutOfRangeException: Index was outside the bounds of the array.
                    // at System.Collections.Generic.Dictionary`2.Insert(TKey key, TValue value, Boolean add)
                    // at System.Collections.Generic.Dictionary`2.set_Item(TKey key, TValue value)
                    // at SportsIq.Pusher.PusherUtil.SendMessage(String key, String channelName, String eventName, String messageBody) in D:\workspace\SIQ\SportsIqNew\SportsIq.Pusher\PusherUtil.cs:line 103 

                    if (!keyExists)
                    {
                        _messageDictionary.Add(key,messageBody);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error($"SendMessage() exception = {exception}");
            }
        }
    }
}
