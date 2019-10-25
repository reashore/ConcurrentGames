using log4net;
using SportsIq.DependencyInjection.Ncaabb;
using SportsIq.Games.Ncaabb;
using SportsIq.MainConsoleTemplate.Ncaabb;
using Topshelf;
using Container = SimpleInjector.Container;

namespace SportsIq.Ncaabb
{
    public static class Program
    {
        public static Container DependencyInjectionContainer { get; }
        public static readonly ILog Logger;

        static Program()
        {
            // static constructors run before all other code
            Logger = LogManager.GetLogger(typeof(Program));
            DependencyInjectionContainer = DependencyInjectorNcaabb.Configure();
        }

        public static void Main()
        {
            ConfigureNcaabbService();
        }

        private static void ConfigureNcaabbService()
        {
            HostFactory.Run(serviceConfig =>
            {
                serviceConfig.Service<NcaabbWindowsService>(serviceInstance =>
                {
                    serviceInstance.ConstructUsing(() => new NcaabbWindowsService());
                    serviceInstance.WhenStarted(execute => execute.Start());
                    serviceInstance.WhenStopped(execute => execute.Stop());
                });

                serviceConfig.SetServiceName("NcaabbService");
                serviceConfig.SetDisplayName("MLB Service");
                serviceConfig.SetDescription("Service to calculate MLB game odds");
                serviceConfig.StartAutomatically();
            });
        }
    }

    public class NcaabbWindowsService
    {
        private readonly NcaabbMainConsole _mlbMainConsole;

        public NcaabbWindowsService()
        {
            _mlbMainConsole = Program.DependencyInjectionContainer.GetInstance<NcaabbMainConsole>();
            _mlbMainConsole.DiContainer = Program.DependencyInjectionContainer;
            _mlbMainConsole.CreateGameFromGameDto = NcaabbGameConverter.CreateNcaabbGameFromNcaabbGameDto;
            _mlbMainConsole.Logger = Program.Logger;
        }

        public bool Start()
        {
            _mlbMainConsole.Start();
            return true;
        }

        public bool Stop()
        {
            _mlbMainConsole.Stop();
            //Program.DependencyInjectionContainer.Dispose();
            return true;
        }
    }
}
