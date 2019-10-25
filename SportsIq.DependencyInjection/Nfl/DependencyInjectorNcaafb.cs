using log4net;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SportsIq.Analytica.Nfl;
using SportsIq.Distributor.Nfl;
using SportsIq.NoSqlDataAccess;
using SportsIq.PubSub;
using SportsIq.Pusher;
using SportsIq.SportsRadar.Nfl;
using SportsIq.SqlDataAccess.Nfl;

namespace SportsIq.DependencyInjection.Nfl
{
    public static class DependencyInjectorNfl
    {        
        private static readonly ILog Logger;

        static DependencyInjectorNfl()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(DependencyInjectorNfl));
        }

        public static Container Configure()
        {
            Container dependencyInjectionContainer = new Container();
            dependencyInjectionContainer.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();

            dependencyInjectionContainer.Register<IDataAccessBaseNfl, DataAccessNfl>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IDataAccessNfl, DataAccessNfl>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IRadarNfl, RadarNfl>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IAnalyticaNfl, AnalyticaNfl>(Lifestyle.Scoped);
            dependencyInjectionContainer.Register<IDistributorNfl, DistributorNfl>(Lifestyle.Scoped);
            dependencyInjectionContainer.Register<IDatastore, Datastore>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IPusherUtil, PusherUtil>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IPubSubUtil, PubSubUtil>(Lifestyle.Singleton);

            dependencyInjectionContainer.Verify();

            return dependencyInjectionContainer;
        }
    }
}
