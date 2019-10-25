using log4net;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SportsIq.Analytica.Nba;
using SportsIq.Distributor.Nba;
using SportsIq.NoSqlDataAccess;
using SportsIq.PubSub;
using SportsIq.Pusher;
using SportsIq.SportsRadar.Nba;
using SportsIq.SqlDataAccess.Nba;

namespace SportsIq.DependencyInjection.Nba
{
    public static class DependencyInjectorNba
    {        
        private static readonly ILog Logger;

        static DependencyInjectorNba()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(DependencyInjectorNba));
        }

        public static Container Configure()
        {
            Container dependencyInjectionContainer = new Container();
            dependencyInjectionContainer.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();

            dependencyInjectionContainer.Register<IDataAccessBaseNba, DataAccessNba>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IDataAccessNba, DataAccessNba>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IRadarNba, RadarNba>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IAnalyticaNba, AnalyticaNba>(Lifestyle.Scoped);
            dependencyInjectionContainer.Register<IDistributorNba, DistributorNba>(Lifestyle.Scoped);
            dependencyInjectionContainer.Register<IDatastore, Datastore>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IPusherUtil, PusherUtil>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IPubSubUtil, PubSubUtil>(Lifestyle.Singleton);

            dependencyInjectionContainer.Verify();

            return dependencyInjectionContainer;
        }
    }
}
