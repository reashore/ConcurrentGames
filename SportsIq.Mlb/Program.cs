using log4net;
using SportsIq.DependencyInjection.Mlb;
using SportsIq.Games.Mlb;
using SportsIq.MainConsoleTemplate.Mlb;
using Topshelf;
using Container = SimpleInjector.Container;

namespace SportsIq.Mlb
{
    public static class Program
    {
        public static Container DependencyInjectionContainer { get; }
        public static readonly ILog Logger;

        static Program()
        {
            // static constructors run before all other code
            Logger = LogManager.GetLogger(typeof(Program));
            DependencyInjectionContainer = DependencyInjectorMlb.Configure();
            //Utils.DeleteLogFile("MLB.log");
            //Utils.DeleteAnalyticaSavedModelsFiles();
        }

        public static void Main()
        {
            ConfigureMlbService();
        }

        private static void ConfigureMlbService()
        {
            HostFactory.Run(serviceConfig =>
            {
                serviceConfig.Service<MlbWindowsService>(serviceInstance =>
                {
                    serviceInstance.ConstructUsing(() => new MlbWindowsService());
                    serviceInstance.WhenStarted(execute => execute.Start());
                    serviceInstance.WhenStopped(execute => execute.Stop());
                });

                serviceConfig.SetServiceName("MlbService");
                serviceConfig.SetDisplayName("MLB Service");
                serviceConfig.SetDescription("Service to calculate MLB game odds");
                serviceConfig.StartAutomatically();
            });
        }
    }

    public class MlbWindowsService
    {
        private readonly MlbMainConsole _mlbMainConsole;

        public MlbWindowsService()
        {
            _mlbMainConsole = Program.DependencyInjectionContainer.GetInstance<MlbMainConsole>();
            _mlbMainConsole.DiContainer = Program.DependencyInjectionContainer;
            _mlbMainConsole.CreateGameFromGameDto = MlbGameConverter.CreateMlbGameFromMlbGameDto;
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
