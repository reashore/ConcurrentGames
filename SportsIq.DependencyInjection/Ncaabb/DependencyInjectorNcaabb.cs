using log4net;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SportsIq.Analytica.Ncaabb;
using SportsIq.Distributor.Ncaabb;
using SportsIq.NoSqlDataAccess;
using SportsIq.PubSub;
using SportsIq.Pusher;
using SportsIq.SportsRadar.Ncaabb;
using SportsIq.SqlDataAccess.Ncaabb;

namespace SportsIq.DependencyInjection.Ncaabb
{
    public static class DependencyInjectorNcaabb
    {        
        private static readonly ILog Logger;

        static DependencyInjectorNcaabb()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(DependencyInjectorNcaabb));
        }

        public static Container Configure()
        {
            Container dependencyInjectionContainer = new Container();
            dependencyInjectionContainer.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();

            dependencyInjectionContainer.Register<IDataAccessBaseNcaabb, DataAccessNcaabb>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IDataAccessNcaabb, DataAccessNcaabb>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IRadarNcaabb, RadarNcaabb>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IAnalyticaNcaabb, AnalyticaNcaabb>(Lifestyle.Scoped);
            dependencyInjectionContainer.Register<IDistributorNcaabb, DistributorNcaabb>(Lifestyle.Scoped);
            dependencyInjectionContainer.Register<IDatastore, Datastore>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IPusherUtil, PusherUtil>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IPubSubUtil, PubSubUtil>(Lifestyle.Singleton);

            dependencyInjectionContainer.Verify();

            return dependencyInjectionContainer;
        }
    }
}
