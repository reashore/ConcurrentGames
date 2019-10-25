using log4net;
using SportsIq.DependencyInjection.Nfl;
using SportsIq.Games.Nfl;
using SportsIq.MainConsoleTemplate.Nfl;
using Topshelf;
using Container = SimpleInjector.Container;

namespace SportsIq.Nfl
{
    public static class Program
    {
        public static Container DependencyInjectionContainer { get; }
        public static readonly ILog Logger;

        static Program()
        {
            // static constructors run before all other code
            Logger = LogManager.GetLogger(typeof(Program));
            DependencyInjectionContainer = DependencyInjectorNfl.Configure();
        }

        public static void Main()
        {
            ConfigureNflService();
        }

        private static void ConfigureNflService()
        {
            HostFactory.Run(serviceConfig =>
            {
                serviceConfig.Service<NflWindowsService>(serviceInstance =>
                {
                    serviceInstance.ConstructUsing(() => new NflWindowsService());
                    serviceInstance.WhenStarted(execute => execute.Start());
                    serviceInstance.WhenStopped(execute => execute.Stop());
                });
                serviceConfig.SetServiceName("NflService");
                serviceConfig.SetDisplayName("NFL Service");
                serviceConfig.SetDescription("Service to calculate NFL game odds");
                serviceConfig.StartAutomatically();
            });
        }
    }

    public class NflWindowsService
    {
        private readonly NflMainConsole _nflMainConsole;

        public NflWindowsService()
        {
            _nflMainConsole = Program.DependencyInjectionContainer.GetInstance<NflMainConsole>();
            _nflMainConsole.DiContainer = Program.DependencyInjectionContainer;
            _nflMainConsole.CreateGameFromGameDto = NflGameConverter.CreateNflGameFromNflGameDto;
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