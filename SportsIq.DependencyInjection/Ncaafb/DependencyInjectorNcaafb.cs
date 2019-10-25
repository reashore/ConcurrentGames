using log4net;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SportsIq.Analytica.Ncaafb;
using SportsIq.Distributor.Ncaafb;
using SportsIq.NoSqlDataAccess;
using SportsIq.PubSub;
using SportsIq.Pusher;
using SportsIq.SportsRadar.Ncaafb;
using SportsIq.SqlDataAccess.Ncaafb;

namespace SportsIq.DependencyInjection.Ncaafb
{
    public static class DependencyInjectorNcaafb
    {        
        private static readonly ILog Logger;

        static DependencyInjectorNcaafb()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(DependencyInjectorNcaafb));
        }

        public static Container Configure()
        {
            Container dependencyInjectionContainer = new Container();
            dependencyInjectionContainer.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();

            dependencyInjectionContainer.Register<IDataAccessBaseNcaafb, DataAccessNcaafb>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IDataAccessNcaafb, DataAccessNcaafb>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IRadarNcaafb, RadarNcaafb>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IAnalyticaNcaafb, AnalyticaNcaafb>(Lifestyle.Scoped);
            dependencyInjectionContainer.Register<IDistributorNcaafb, DistributorNcaafb>(Lifestyle.Scoped);
            dependencyInjectionContainer.Register<IDatastore, Datastore>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IPusherUtil, PusherUtil>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IPubSubUtil, PubSubUtil>(Lifestyle.Singleton);

            dependencyInjectionContainer.Verify();

            return dependencyInjectionContainer;
        }
    }
}
