using log4net;
using SportsIq.DependencyInjection.Wnba;
using SportsIq.Games.Wnba;
using SportsIq.MainConsoleTemplate.Wnba;
using Topshelf;
using Container = SimpleInjector.Container;

namespace SportsIq.Wnba
{
    public static class Program
    {
        public static Container DependencyInjectionContainer { get; }
        public static readonly ILog Logger;

        static Program()
        {
            // static constructors run before all other code
            Logger = LogManager.GetLogger(typeof(Program));
            DependencyInjectionContainer = DependencyInjectorWnba.Configure();
        }

        public static void Main()
        {
            ConfigureWnbaService();
        }

        private static void ConfigureWnbaService()
        {
            HostFactory.Run(serviceConfig =>
            {
                serviceConfig.Service<WnbaPropsWindowsService>(serviceInstance =>
                {
                    serviceInstance.ConstructUsing(() => new WnbaPropsWindowsService());
                    serviceInstance.WhenStarted(execute => execute.Start());
                    serviceInstance.WhenStopped(execute => execute.Stop());
                });
                serviceConfig.SetServiceName("WnbaService");
                serviceConfig.SetDisplayName("WNBA Service");
                serviceConfig.SetDescription("Service to calculate WNBA game odds");
                serviceConfig.StartAutomatically();
            });
        }
    }

    public class WnbaPropsWindowsService
    {
        private readonly WnbaMainConsole _wnbaMainConsole;

        public WnbaPropsWindowsService()
        {
            _wnbaMainConsole = Program.DependencyInjectionContainer.GetInstance<WnbaMainConsole>();
            _wnbaMainConsole.DiContainer = Program.DependencyInjectionContainer;
            _wnbaMainConsole.CreateGameFromGameDto = WnbaGameConverter.CreateWnbaGameFromWnbaGameDto;
            _wnbaMainConsole.Logger = Program.Logger;
        }

        public bool Start()
        {
            _wnbaMainConsole.Start();
            return true;
        }

        public bool Stop()
        {
            _wnbaMainConsole.Stop();
            return true;
        }
    }
}
