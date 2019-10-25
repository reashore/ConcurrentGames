using log4net;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SportsIq.Analytica.Nhl;
using SportsIq.Distributor.Nhl;
using SportsIq.NoSqlDataAccess;
using SportsIq.PubSub;
using SportsIq.Pusher;
using SportsIq.SportsRadar.Nhl;
using SportsIq.SqlDataAccess.Nhl;

namespace SportsIq.DependencyInjection.Nhl
{
    public static class DependencyInjectorNhl
    {        
        private static readonly ILog Logger;

        static DependencyInjectorNhl()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(DependencyInjectorNhl));
        }

        public static Container Configure()
        {
            Container dependencyInjectionContainer = new Container();
            dependencyInjectionContainer.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();

            dependencyInjectionContainer.Register<IDataAccessBaseNhl, DataAccessNhl>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IDataAccessNhl, DataAccessNhl>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IRadarNhl, RadarNhl>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IAnalyticaNhl, AnalyticaNhl>(Lifestyle.Scoped);
            dependencyInjectionContainer.Register<IDistributorNhl, DistributorNhl>(Lifestyle.Scoped);
            dependencyInjectionContainer.Register<IDatastore, Datastore>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IPusherUtil, PusherUtil>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IPubSubUtil, PubSubUtil>(Lifestyle.Singleton);

            dependencyInjectionContainer.Verify();

            return dependencyInjectionContainer;
        }
    }
}
