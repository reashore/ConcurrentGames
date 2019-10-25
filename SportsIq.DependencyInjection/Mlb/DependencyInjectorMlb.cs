using log4net;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SportsIq.Analytica.Mlb;
using SportsIq.Distributor.Mlb;
using SportsIq.NoSqlDataAccess;
using SportsIq.PubSub;
using SportsIq.Pusher;
using SportsIq.SportsRadar.Mlb;
using SportsIq.SqlDataAccess.Mlb;

namespace SportsIq.DependencyInjection.Mlb
{
    public static class DependencyInjectorMlb
    {        
        private static readonly ILog Logger;

        static DependencyInjectorMlb()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(DependencyInjectorMlb));
        }

        public static Container Configure()
        {
            Container dependencyInjectionContainer = new Container();
            dependencyInjectionContainer.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();

            dependencyInjectionContainer.Register<IDataAccessBaseMlb, DataAccessMlb>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IDataAccessMlb, DataAccessMlb>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IRadarMlb, RadarMlb>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IAnalyticaMlb, AnalyticaMlb>(Lifestyle.Scoped);
            dependencyInjectionContainer.Register<IDistributorMlb, DistributorMlb>(Lifestyle.Scoped);
            dependencyInjectionContainer.Register<IDatastore, Datastore>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IPusherUtil, PusherUtil>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IPubSubUtil, PubSubUtil>(Lifestyle.Singleton);

            dependencyInjectionContainer.Verify();

            return dependencyInjectionContainer;
        }
    }
}
