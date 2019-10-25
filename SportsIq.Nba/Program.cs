using log4net;
using SportsIq.DependencyInjection.Nba;
using SportsIq.Games.Nba;
using SportsIq.MainConsoleTemplate.Nba;
using Topshelf;
using Container = SimpleInjector.Container;

namespace SportsIq.Nba
{
    public static class Program
    {
        public static Container DependencyInjectionContainer { get; }
        public static readonly ILog Logger;

        static Program()
        {
            // static constructors run before all other code
            Logger = LogManager.GetLogger(typeof(Program));
            DependencyInjectionContainer = DependencyInjectorNba.Configure();
        }

        public static void Main()
        {
            ConfigureNbaService();
        }

        private static void ConfigureNbaService()
        {
            HostFactory.Run(serviceConfig =>
            {
                serviceConfig.Service<NbaPropsWindowsService>(serviceInstance =>
                {
                    serviceInstance.ConstructUsing(() => new NbaPropsWindowsService());
                    serviceInstance.WhenStarted(execute => execute.Start());
                    serviceInstance.WhenStopped(execute => execute.Stop());
                });
                serviceConfig.SetServiceName("NbaService");
                serviceConfig.SetDisplayName("NBA Service");
                serviceConfig.SetDescription("Service to calculate NBA game odds");
                serviceConfig.StartAutomatically();
            });
        }
    }

    public class NbaPropsWindowsService
    {
        private readonly NbaMainConsole _nbaMainConsole;

        public NbaPropsWindowsService()
        {
            _nbaMainConsole = Program.DependencyInjectionContainer.GetInstance<NbaMainConsole>();
            _nbaMainConsole.DiContainer = Program.DependencyInjectionContainer;
            _nbaMainConsole.CreateGameFromGameDto = NbaGameConverter.CreateNbaGameFromNbaGameDto;
            _nbaMainConsole.Logger = Program.Logger;
        }

        public bool Start()
        {
            _nbaMainConsole.Start();
            return true;
        }

        public bool Stop()
        {
            _nbaMainConsole.Stop();
            return true;
        }
    }
}
