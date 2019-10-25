using log4net;
using SportsIq.DependencyInjection.Ncaafb;
using SportsIq.Games.Ncaafb;
using SportsIq.MainConsoleTemplate.Ncaafb;
using Topshelf;
using Container = SimpleInjector.Container;

namespace SportsIq.NCaafb
{
    public static class Program
    {
        public static Container DependencyInjectionContainer { get; }
        public static readonly ILog Logger;

        static Program()
        {
            // static constructors run before all other code
            Logger = LogManager.GetLogger(typeof(Program));
            DependencyInjectionContainer = DependencyInjectorNcaafb.Configure();
        }

        public static void Main()
        {
            ConfigureNcaafbService();
        }

        private static void ConfigureNcaafbService()
        {
            HostFactory.Run(serviceConfig =>
            {
                serviceConfig.Service<NcaafbWindowsService>(serviceInstance =>
                {
                    serviceInstance.ConstructUsing(() => new NcaafbWindowsService());
                    serviceInstance.WhenStarted(execute => execute.Start());
                    serviceInstance.WhenStopped(execute => execute.Stop());
                });
                serviceConfig.SetServiceName("NcaafbService");
                serviceConfig.SetDisplayName("NCAAFB Service");
                serviceConfig.SetDescription("Service to calculate NCAAFB game odds");
                serviceConfig.StartAutomatically();
            });
        }
    }

    public class NcaafbWindowsService
    {
        private readonly NcaafbMainConsole _nflMainConsole;

        public NcaafbWindowsService()
        {
            _nflMainConsole = Program.DependencyInjectionContainer.GetInstance<NcaafbMainConsole>();
            _nflMainConsole.DiContainer = Program.DependencyInjectionContainer;
            _nflMainConsole.CreateGameFromGameDto = NcaafbGameConverter.CreateNcaafbGameFromNcaafbGameDto;
            _nflMainConsole.Logger = Program.Logger;
        }

        public bool Start()
        {
            _nflMainConsole.Start();
            return true;
        }

        public bool Stop()
        {
            _nflMainConsole.Stop();
            //Program.DependencyInjectionContainer.Dispose();
            return true;
        }
    }
}