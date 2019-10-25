using log4net;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SportsIq.Analytica.Wnba;
using SportsIq.Distributor.Wnba;
using SportsIq.NoSqlDataAccess;
using SportsIq.PubSub;
using SportsIq.Pusher;
using SportsIq.SportsRadar.Wnba;
using SportsIq.SqlDataAccess.Wnba;

namespace SportsIq.DependencyInjection.Wnba
{
    public static class DependencyInjectorWnba
    {        
        private static readonly ILog Logger;

        static DependencyInjectorWnba()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(DependencyInjectorWnba));
        }

        public static Container Configure()
        {
            Container dependencyInjectionContainer = new Container();
            dependencyInjectionContainer.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();

            dependencyInjectionContainer.Register<IDataAccessBaseWnba, DataAccessWnba>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IDataAccessWnba, DataAccessWnba>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IRadarWnba, RadarWnba>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IAnalyticaWnba, AnalyticaWnba>(Lifestyle.Scoped);
            dependencyInjectionContainer.Register<IDistributorWnba, DistributorWnba>(Lifestyle.Scoped);
            dependencyInjectionContainer.Register<IDatastore, Datastore>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IPusherUtil, PusherUtil>(Lifestyle.Singleton);
            dependencyInjectionContainer.Register<IPubSubUtil, PubSubUtil>(Lifestyle.Singleton);

            dependencyInjectionContainer.Verify();

            return dependencyInjectionContainer;
        }
    }
}
