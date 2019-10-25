using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using log4net;
using SportsIq.Pusher;
using SportsIq.Utilities;

namespace SportsIq.SportsRadar
{
    public interface IRadarBase
    {
        bool IsSimulation { get; set; }
        Task SubscribeToGameEvents();
    }

    public abstract class RadarBase : IRadarBase
    {
        protected IPusherUtil PusherUtil;
        protected static readonly ILog Logger;
        protected XmlSerializer GameInfoXmlSerializer { get; set; }
        public DateTime TimeOfLastRadarGameEventOrHeartbeat { get; set; }
        public bool IsSimulation { get; set; }

        static RadarBase()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(RadarBase));
        }

        public abstract Task SubscribeToGameEvents();

        protected string AdjustGameEventBaseUrlForSimulation(string gameEventBaseUrl)
        {
            if (!IsSimulation)
            {
                return gameEventBaseUrl;
            }

            const string trial = "trial";
            const string production = "production";
            const string simulation = "simulation";

            if (gameEventBaseUrl.Contains(trial))
            {
                gameEventBaseUrl = gameEventBaseUrl.Replace(trial, simulation);
            }

            if (gameEventBaseUrl.Contains(production))
            {
                gameEventBaseUrl = gameEventBaseUrl.Replace(production, simulation);
            }

            return gameEventBaseUrl;
        }

        // todo remove once all code uses new XSD classes
        protected T GetGameInfo<T>(Uri sportRadarUri) where T : class
        {
            string gameInfoXmlString = ReadResponseFromUri(sportRadarUri);

            StringReader stringReader = new StringReader(gameInfoXmlString);
            T gameInfoXml = (T)GameInfoXmlSerializer.Deserialize(stringReader);
            return gameInfoXml;
        }

        protected static string ReadResponseFromUri(Uri uri)
        {
            const int twoMinutes = 2 * 60 * 1000;
            WebRequest webRequest = WebRequest.Create(uri);
            webRequest.Timeout = twoMinutes;
            webRequest.Proxy = null;
            WebResponse webResponse = webRequest.GetResponse();
            Stream stream = webResponse.GetResponseStream();

            if (stream == null)
            {
                const string message = "stream is null";
                Logger.Error(message);
                throw new Exception(message);
            }

            StreamReader streamReader = new StreamReader(stream);
            string responseString = streamReader.ReadToEnd();
            streamReader.Close();

            return responseString;
        }

        protected async Task SubscribeToGameEventsBase(Uri subscriptionUrl, Action<string> raiseRadarGameEvent)
        {
            using (HttpClient httpClient = new HttpClient{Timeout = Timeout.InfiniteTimeSpan})
            {
                //httpClient.
                using (Stream stream = await httpClient.GetStreamAsync(subscriptionUrl))
                {
                    using (StreamReader streamReader = new StreamReader(stream))
                    {
                        try
                        {
                            ulong count = 0;

                            while (true)
                            {
                                string jsonString = streamReader.ReadLine();

                                if (Utils.IsValidJson(jsonString))
                                {
                                    raiseRadarGameEvent(jsonString);

                                    // since Radar heartbeats occur every 5 seconds,
                                    // this will produce a "secondary" heartbeat every 5 * 9 = 45 seconds

                                    if (count % 9 == 0)
                                    {
                                      PusherUtil.SendHeartbeat();
                                    }
                                }
                                else
                                {
                                    Logger.Info($"Invalid JSON = {jsonString}");
                                }

                                count++;
                            }
                        }
                        catch (Exception exception)
                        {
                            Logger.Error($"SubscribeToGameEventsBase() exception = {exception}");
                            throw;
                        }
                    }
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}

