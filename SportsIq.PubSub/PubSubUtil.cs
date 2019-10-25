using Google.Cloud.PubSub.V1;
using System;
using System.Configuration;
using System.Threading.Tasks;
using log4net;

namespace SportsIq.PubSub
{
    public class PubSubEventArgs : EventArgs
    {
        //public Guid GameId { get; set; }
        public PubsubMessage PubsubMessage { get; set; }
    }

    public interface IPubSubUtil
    {
        event EventHandler<PubSubEventArgs> PubSubEvent;
        Task SubscribeToTopic(string subscriptionId);
    }

    public class PubSubUtil : IPubSubUtil
    {
        private static readonly ILog Logger;
        private readonly string _projectId;
        private readonly string _mode;

        static PubSubUtil()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(PubSubUtil));
        }

        public PubSubUtil()
        {
            _projectId = ConfigurationManager.AppSettings["projectId"];
            _mode = ConfigurationManager.AppSettings["mode"];
        }

        //-------------------------------------------------------------------------------------

        public event EventHandler<PubSubEventArgs> PubSubEvent;

        private void OnPubSubEvent(PubSubEventArgs pubSubEventArgs)
        {
            PubSubEvent?.Invoke(this, pubSubEventArgs);
        }

        //-------------------------------------------------------------------------------------

        public async Task SubscribeToTopic(string subscriptionId)
        {
            SubscriptionName subscriptionName = new SubscriptionName(_projectId, subscriptionId);
            SubscriberClient subscriberClient = await SubscriberClient.CreateAsync(subscriptionName);

            await subscriberClient.StartAsync((pubsubMessage, cancellationToken) =>
            {
                PubSubEventArgs pubSubEventArgs = new PubSubEventArgs
                {
                    PubsubMessage = pubsubMessage
                };
                OnPubSubEvent(pubSubEventArgs);

                return Task.FromResult(SubscriberClient.Reply.Ack);
            });
        }
    }
}
